import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Options, SeriesOptionsType, XAxisOptions } from 'highcharts';
import { DimensionDto, SeguimientoDto } from 'src/app/core/api/api.client';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { ChartBase, IChartTypedBase } from '../../core/chart/chart-base';
import { IChartBuilder } from '../../core/chart/chart-builder';
import { ICategory, IChartLabels, ISerieSelector, IValueSelector } from '../../core/chart/types';

@Injectable({
  providedIn: 'root',
})
export class ResumeBuilder implements IChartBuilder {
  private dimensions: DimensionDto[];
  private chart: IChartTypedBase<SeguimientoDto>;
  private pipe = new DatePipe('es-ES');

  constructor(private indicatorService: IndicatorService, private translate: TranslateService) {
    this.chart = new ChartBase<SeguimientoDto>(translate);
    const options: Options = {
      chart: {
        type: 'column',
      },
      xAxis: {
        type: 'category',
        crosshair: true,
      },
    };

    this.chart.setOptions(options);
  }

  public setLabels(labels: IChartLabels): void {
    this.chart.setLabels(labels);
  }

  public setBaseOptions(options: Options): void {
    this.chart.setOptions(options);
  }

  public async initialize(): Promise<void> {
    this.dimensions = await this.indicatorService.getDimensions().toPromise();
  }

  public setCategorySelector(): void {
    const dimensions = this.dimensions;
    const selector = function (data: SeguimientoDto[]): ICategory[] {
      return dimensions.map((x) => ({ id: x.id, value: x.descripcion }));
    };

    this.chart.setCategorySelector(selector);
  }

  public setSerieSelector(): void {
    const serieSelector: ISerieSelector<SeguimientoDto> = (data: SeguimientoDto) => {
      return { id: data.fecha, type: 'column', value: this.pipe.transform(data.fecha, 'fullDate') };
    };

    this.chart.setSerieSelector(serieSelector);
  }

  public setValueSelector(): void {
    const dimensions = this.dimensions;

    const valueSelector: IValueSelector<SeguimientoDto> = (data: SeguimientoDto[], category: ICategory): any => {
      if ((data || []).length !== 1) {
        return;
      }

      const seguimiento = data[0];
      const categorias = (dimensions.find((x) => x.id === category.id) || { categorias: [] }).categorias;
      const indicadores = seguimiento.indicadores.filter((i) => categorias.some((c) => c.id === i.indicador.categoriaId));
      if (!indicadores) {
        return 0;
      }

      const value = indicadores.reduce((prv, item) => (prv += item.indicador.puntuacion), 0);
      return { name: category.value, y: value, drilldown: `${category.id}-${seguimiento.id}` };
    };

    this.chart.setValueSelector(valueSelector);
  }

  public setData(data: any[]): void {
    const seguimientos = data as SeguimientoDto[];
    this.chart.setData(seguimientos);

    // set drilldown data
    const series: SeriesOptionsType[] = [];
    seguimientos.forEach((s) => {
      this.dimensions.forEach((d) => {
        const categories = d.categorias || [];
        const serieData = categories.map((c) => {
          const indicators = (s.indicadores || []).filter((i) => i.indicador && c.id === i.indicador.categoriaId);
          const value = indicators.reduce((prv, item) => (prv += item.indicador.puntuacion), 0);
          return [c.descripcion, value];
        });

        series.push({
          id: `${d.id}-${s.id}`,
          type: 'column',
          name: `${d.descripcion} ${s.fecha.toDateString()}`,
          data: serieData,
        });
      });
    });

    const options: Options = {
      drilldown: { series },
    };

    this.chart.setOptions(options);
  }

  public getChartOptions(): Options {
    const options = this.chart.getChartOptions();
    (options.xAxis as XAxisOptions).categories = undefined;

    return options;
  }
}
