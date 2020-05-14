import { FilterComparison, FilterUnion } from './filter.enum';

export interface IBaseFilter {
  PropertyName: string;
  Value: any;
  Comparison: FilterComparison;
  Union: FilterUnion;
}

export class BaseFilter implements IBaseFilter {
  public constructor(public PropertyName: string, public Value: any, public Comparison: FilterComparison, public Union: FilterUnion) {}
}

export const getFilterQuery = (filters: IBaseFilter[]) => {
  return JSON.stringify(filters);
};
