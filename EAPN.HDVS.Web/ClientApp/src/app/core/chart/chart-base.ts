import { TranslateService } from '@ngx-translate/core';
import { Options } from 'highcharts';
import { ICategorySelector, ISerieSelector, IValueSelector, ICategory, ISerie, IChartLabels } from './types';

export interface IChartBase {
  getChartOptions(): Options;
  setOptions(options: Options): void;
  clearOptions(): void;
  setLabels(labels: IChartLabels): void;
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
  private baseOptions: Options = {};

  constructor(private translate: TranslateService) {}

  public setData(data: T[]): void {
    this.data = data;
  }

  public setOptions(options: Options): void {
    this.baseOptions = this.combineObjects(options, this.baseOptions);
  }

  public clearOptions(): void {
    this.baseOptions = {};
  }

  public setLabels(labels: IChartLabels): void {
    const options: Options = {
      title: {
        text: labels.chartTitle,
      },
      xAxis: {
        title: {
          text: labels.xAxisTitle,
        },
      },
      yAxis: {
        title: {
          text: labels.yAxisTitle,
        },
      },
    };

    this.setOptions(options);
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
      mappers.forEach((x) => data.push(this.valueSelector(values.data, x)));
      chartSeries.push({ type: values.serie.type as any, name: values.serie.value, data });
    });

    const options: Options = {
      xAxis: {
        categories: categories,
      },
      series: chartSeries,
      lang: this.translate.instant('chart'),
    };

    this.setOptions(options);
    return this.baseOptions;
  }

  private groupBy(list: T[], keyGetter: ISerieSelector<T>): Map<number, { serie: ISerie; data: T[] }> {
    const map = new Map<number, { serie: ISerie; data: T[] }>();

    list.forEach((item: T) => {
      const serie = keyGetter(item);

      if (!map.has(serie.id)) {
        map.set(serie.id, { serie, data: [] });
      }

      map.get(serie.id).data.push(item);
    });

    return map;
  }

  private combineObjects<TType>(target: TType, source: TType): TType {
    Object.entries(source).forEach(([key, value]) => {
      if (value && typeof value === 'object' && !Array.isArray(value)) {
        this.combineObjects((target[key] = target[key] || {}), value);
        return;
      }
      target[key] = value;
    });

    return target;
  }
}
