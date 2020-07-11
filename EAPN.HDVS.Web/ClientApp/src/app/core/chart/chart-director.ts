import { Injectable } from '@angular/core';
import { Options } from 'highcharts';
import { IChartBuilder } from './chart-builder';

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

  public async buildGraph<T>(data: T[]): Promise<void> {
    await this.builder.initialize();
    this.builder.setCategorySelector();
    this.builder.setSerieSelector();
    this.builder.setValueSelector();
    this.builder.setData(data);
  }

  public getChartOptions(): Options {
    return this.builder.getChartOptions();
  }
}
