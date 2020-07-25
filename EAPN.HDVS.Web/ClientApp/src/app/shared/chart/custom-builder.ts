import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Options } from 'highcharts';
import { ChartBase, IChartTypedBase } from '../../core/chart/chart-base';
import { IChartBuilder } from '../../core/chart/chart-builder';
import { ICategorySelector, IChartLabels, ISerieSelector, IValueSelector } from '../../core/chart/types';

@Injectable({
  providedIn: 'root',
})
export class CustomBuilder<T> implements IChartBuilder {
  private chart: IChartTypedBase<T>;
  public categorySelector: ICategorySelector<T>;
  public serieSelector: ISerieSelector<T>;
  public valueSelector: IValueSelector<T>;
  private chartType: string;

  constructor(private translate: TranslateService) {}

  public setLabels(labels: IChartLabels): void {
    this.chart.setLabels(labels);
  }

  public setBaseOptions(options: Options): void {
    this.chart.setOptions(options);
  }

  public async initialize(): Promise<void> {
    this.chart = new ChartBase<T>(this.translate);
    this.chart.clearOptions();
    const options: Options = {
      plotOptions: {
        series: {
          dataLabels: {
            enabled: true,
          },
        },
      },
      chart: {
        type: this.chartType,
      },
      xAxis: {
        type: 'category',
        crosshair: true,
      },
    };

    this.chart.setOptions(options);
  }

  public setCategorySelector(): void {
    this.chart.setCategorySelector(this.categorySelector);
  }

  public setSerieSelector(): void {
    this.chart.setSerieSelector(this.serieSelector);
  }

  public setValueSelector(): void {
    this.chart.setValueSelector(this.valueSelector);
  }

  public setData(data: any[]): void {
    this.chart.setData(data as T[]);
  }

  public getChartOptions(): Options {
    return this.chart.getChartOptions();
  }
}
