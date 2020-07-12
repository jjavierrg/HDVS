import { Injectable } from '@angular/core';
import { Options } from 'highcharts';
import { IChartBuilder } from './chart-builder';
import { IChartLabels } from './types';

/**
 * The Director is only responsible for executing the building steps in a
 * particular sequence.
 */
@Injectable({
  providedIn: 'root',
})
export class ChartDirector {
  private builder: IChartBuilder;

  public setBuilder(builder: IChartBuilder): void {
    this.builder = builder;
  }

  public async buildGraph<T>(data: T[], options?: { baseOptions?: Options; labels?: IChartLabels }): Promise<void> {
    await this.builder.initialize();

    if (options && options.baseOptions) {
      this.builder.setBaseOptions(options.baseOptions);
    }

    if (options && options.labels) {
      this.builder.setLabels(options.labels);
    }

    this.builder.setCategorySelector();
    this.builder.setSerieSelector();
    this.builder.setValueSelector();
    this.builder.setData(data);
  }

  public getChartOptions(): Options {
    return this.builder.getChartOptions();
  }
}
