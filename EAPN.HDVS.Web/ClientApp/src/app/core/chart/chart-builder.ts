import { Options } from 'highcharts';

/**
 * The Builder interface specifies methods for creating chartOption object
 */
export interface IChartBuilder {
  initialize(): Promise<void>;
  setCategorySelector(): void;
  setSerieSelector(): void;
  setValueSelector(): void;
  setData(data: any[]): void;

  getChartOptions(): Options;
}
