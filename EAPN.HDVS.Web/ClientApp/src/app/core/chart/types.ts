export interface IChartLabels { chartTitle?: string; xAxisTitle?: string; yAxisTitle?: string; }
export interface ICategory { id: any; value: string; }
export interface ISerie { id: any; value: string; type: string; }

export type ICategorySelector<T> = (data: T[]) => ICategory[];
export type ISerieSelector<T> = (item: T) => ISerie;
export type IValueSelector<T> = (data: T[], category: ICategory) => any;
