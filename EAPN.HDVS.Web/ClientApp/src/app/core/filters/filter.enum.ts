export enum FilterComparison {
  Equal = 'Equal',
  LessThan = 'LessThan',
  LessThanOrEqual = 'LessThanOrEqual',
  GreaterThan = 'GreaterThan',
  GreaterThanOrEqual = 'GreaterThanOrEqual',
  NotEqual = 'NotEqual',
  Contains = 'Contains',
  StartsWith = 'StartsWith',
  EndsWith = 'EndsWith',
  In = 'In',
  Like = 'Like',
  IsNull = 'IsNull',
  IsNotNull = 'IsNotNull',
}

export enum FilterUnion {
  And = 'And',
  Or = 'Or',
}
