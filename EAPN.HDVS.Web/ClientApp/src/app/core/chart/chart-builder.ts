import { Options } from 'highcharts';
import { IChartLabels } from './Types';

/**
 * The Builder interface specifies methods for creating chartOption object
 */
export interface IChartBuilder {
  initialize(): Promise<void>;
  setLabels(labels: IChartLabels): void;
  setBaseOptions(options: Options): void;

  setCategorySelector(): void;
  setSerieSelector(): void;
  setValueSelector(): void;
  setData(data: any[]): void;

  getChartOptions(): Options;
}
