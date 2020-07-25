using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EAPN.HDVS.Infrastructure.Core.Queries
{
    /// <summary>
    /// Comparison enumator
    /// </summary>
    public enum Comparison
    {
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual,
        Contains, //for strings  
        StartsWith, //for strings  
        EndsWith, //for strings  
        In,
        Like, // for strings
        IsNull,
        IsNotNull
    }

    public class ExpressionFilter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Comparison Comparison { get; set; }
        public string Union { get; set; }

        public static IEnumerable<ExpressionFilter> ConvertFromString(string filterString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExpressionFilter>>(filterString);
        }
    }

    public static class ExpressionRetriever
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        private static MethodInfo inIdMethod = inIdMethod = typeof(List<object>).GetMethod("Contains", new[] { typeof(object) });
        private static readonly MethodInfo likeMethod = typeof(DbFunctionsExtensions).GetMethod("Like", new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        private static Expression ApplyComparisonFunction(ExpressionFilter filter, MemberExpression member, Expression property)
        {
            switch (filter.Comparison)
            {
                case Comparison.Equal:
                    return Expression.Equal(member, property);
                case Comparison.GreaterThan:
                    return Expression.GreaterThan(member, property);
                case Comparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, property);
                case Comparison.LessThan:
                    return Expression.LessThan(member, property);
                case Comparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, property);
                case Comparison.NotEqual:
                    return Expression.NotEqual(member, property);
                case Comparison.Contains:
                    return Expression.Call(member, containsMethod, property);
                case Comparison.StartsWith:
                    return Expression.Call(member, startsWithMethod, property);
                case Comparison.EndsWith:
                    return Expression.Call(member, endsWithMethod, property);
                case Comparison.In:
                    var converted = Expression.Convert(member, typeof(object));
                    return Expression.Call(property, inIdMethod, converted);
                case Comparison.Like:
                    return Expression.Call(null, likeMethod, Expression.Constant(EF.Functions), member, property);
                case Comparison.IsNull:
                    return Expression.Equal(member, property);
                case Comparison.IsNotNull:
                    return Expression.NotEqual(member, property);
                default:
                    return null;
            }
        }

        private static Expression GetPropertyValue(ExpressionFilter filter, MemberExpression member)
        {
            if (filter.Comparison == Comparison.In)
            {
                return Expression.Property(GetConstantValue(filter, member), "Value");
            }

            if (filter.Comparison == Comparison.IsNull || filter.Comparison == Comparison.IsNotNull)
            {
                return Expression.Constant(null, member.Type);
            }

            return Expression.Convert(Expression.Property(GetConstantValue(filter, member), "Value"), member.Type);
        }

        private static ConstantExpression GetConstantValue(ExpressionFilter filter, MemberExpression member)
        {
            ConstantExpression constant;
            if (filter.Comparison != Comparison.In)
            {
                var propertyType = ((PropertyInfo)member.Member).PropertyType;
                var propertyValue = filter.Value;
                if (!propertyType.IsInstanceOfType(propertyValue))
                {
                    var converter = TypeDescriptor.GetConverter(propertyType);
                    if (!converter.CanConvertFrom(typeof(string)))
                    {
                        throw new NotSupportedException();
                    }

                    propertyValue = converter.ConvertFromInvariantString(filter.Value.ToString());
                }

                constant = Expression.Constant(new { Value = propertyValue });
            }
            else
            {
                List<object> elements = filter.Value.ToString().Split(',')
                    .Select(str => ParseValueMember(member.Type, typeof(string), str.TrimEnd().TrimStart()))
                    .ToList();
                constant = Expression.Constant(new { Value = elements });
            }
            return constant;
        }

        public static MemberExpression GetExpression(ParameterExpression param, string propertyName)
        {

            var properties = propertyName.Split('.');
            MemberExpression expression = Expression.Property(param, properties[0]);
            for (int i = 1; i < properties.Length; i++)
            {
                expression = Expression.Property(expression, properties[i]);
            }
            return expression;
        }
        public static Expression GetExpression(ParameterExpression param, ExpressionFilter filter)
        {
            MemberExpression member = GetExpression(param, filter.PropertyName);
            Expression property = GetPropertyValue(filter, member);
            return ApplyComparisonFunction(filter, member, property);
        }

        private static object ParseValueMember(Type memberType, Type constantType, object filterValue)
        {
            if (memberType == typeof(int) || memberType == typeof(int?))
            {
                if (constantType == typeof(string))
                {
                    return Int32.Parse(filterValue.ToString());
                }
                else if (constantType == typeof(long))
                {
                    return (int)(long)filterValue;
                }

            }
            else if (memberType == typeof(bool))
            {
                return Boolean.Parse(filterValue.ToString());
            }

            return filterValue;
        }


        public static Expression<Func<T, bool>> ConstructAndExpressionTree<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp;

            if (filters.Count == 1)
            {
                exp = GetExpression(param, filters[0]);
            }
            else
            {
                exp = GetExpression(param, filters[0]);
                for (int i = 1; i < filters.Count; i++)
                {
                    if (filters[i].Union?.ToLowerInvariant() == "or")
                    {
                        exp = Expression.Or(exp, GetExpression(param, filters[i]));
                    }
                    else
                    {
                        exp = Expression.And(exp, GetExpression(param, filters[i]));
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public static Expression<Func<T, object>> ContructOrderExpression<T>(string orderField)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var properties = orderField.Split('.');
            MemberExpression expression = Expression.Property(param, properties[0]);
            for (int i = 1; i < properties.Length; i++)
            {
                expression = Expression.Property(expression, properties[i]);
            }

            return Expression.Lambda<Func<T, object>>
                (Expression.Convert(expression, typeof(object)), param);
        }
    }
}

