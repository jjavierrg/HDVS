import { Options } from 'highcharts';
import { ICategory, ICategorySelector, ISerie, ISerieSelector, IValueSelector } from './Types';

export interface IChartBase {
  getChartOptions(): Highcharts.Options;
}

export interface IChartTypedBase<T> extends IChartBase {
  setCategorySelector(selector: ICategorySelector<T>): void;
  setSerieSelector(selector: ISerieSelector<T>): void;
  setValueSelector(selector: IValueSelector<T>): void;
  setData(data: T[]): void;
}

export class ChartBase<T> implements IChartBase {
  private data: T[];
  private categorySelector: ICategorySelector<T>;
  private serieSelector: ISerieSelector<T>;
  private valueSelector: IValueSelector<T>;

  public setData(data: T[]): void {
    this.data = data;
  }

  public setCategorySelector(selector: ICategorySelector<T>): void {
    this.categorySelector = selector;
  }

  public setSerieSelector(selector: ISerieSelector<T>): void {
    this.serieSelector = selector;
  }

  public setValueSelector(selector: IValueSelector<T>): void {
    this.valueSelector = selector;
  }

  public getChartOptions(): Options {
    const mappers: ICategory[] = this.categorySelector(this.data);
    if (!mappers) {
      throw new Error('No category found');
    }

    const series = this.groupBy(this.data, this.serieSelector);
    if (!series) {
      throw new Error('No series found');
    }

    const categories: string[] = mappers.map((x) => x.value);
    const chartSeries: Highcharts.SeriesOptionsType[] = [];

    series.forEach((values, key) => {
      const data: any[] = [];
      mappers.forEach((x) => data.push(this.valueSelector(values, x)));
      chartSeries.push({ type: key.type as any, name: key.value, data });
    });

    const options: Highcharts.Options = {
      xAxis: {
        categories: categories,
        crosshair: true,
      },
      series: chartSeries,
    };

    return options;
  }

  private groupBy(list: T[], keyGetter: ISerieSelector<T>): Map<ISerie, T[]> {
    const map = new Map<any, T[]>();

    list.forEach((item: T) => {
      const key = keyGetter(item);

      if (!map.has(key)) {
        map.set(key, []);
      }

      map.get(key).push(item);
    });

    return map;
  }
}
