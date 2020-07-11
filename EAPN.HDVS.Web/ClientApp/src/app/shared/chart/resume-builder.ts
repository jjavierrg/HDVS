import { Injectable } from '@angular/core';
import { Options } from 'highcharts';
import { DimensionDto, SeguimientoDto } from 'src/app/core/api/api.client';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { ChartBase, IChartTypedBase } from '../../core/chart/chart-base';
import { IChartBuilder } from '../../core/chart/chart-builder';
import { ICategory, ISerieSelector, IValueSelector } from '../../core/chart/Types';

@Injectable({
  providedIn: 'root',
})
export class ResumeBuilder implements IChartBuilder {
  private dimensions: DimensionDto[];
  private chart: IChartTypedBase<SeguimientoDto>;

  constructor(private indicatorService: IndicatorService) {}

  async initialize(): Promise<void> {
    this.dimensions = await this.indicatorService.getDimensions().toPromise();
    this.chart = new ChartBase<SeguimientoDto>();
  }

  setCategorySelector(): void {
    const dimensions = this.dimensions;
    const selector = function (data: SeguimientoDto[]): ICategory[] {
      return dimensions.map((x) => ({ id: x.id, value: x.descripcion }));
    };

    this.chart.setCategorySelector(selector);
  }

  setSerieSelector(): void {
    const serieSelector: ISerieSelector<SeguimientoDto> = (data: SeguimientoDto) => {
      return { id: data.fecha, type: 'column', value: data.fecha.toDateString() };
    };

    this.chart.setSerieSelector(serieSelector);
  }

  setValueSelector(): void {
    const dimensions = this.dimensions;

    const valueSelector: IValueSelector<SeguimientoDto> = (data: SeguimientoDto[], category: ICategory): any[] => {
      const categorias = (dimensions.find((x) => x.id === category.id) || { categorias: [] }).categorias;
      return data.map((x) => {
        const indicadores = x.indicadores.filter((i) => categorias.some((c) => c.id === i.indicador.categoriaId));
        if (!indicadores) {
          return 0;
        }

        return indicadores.reduce((prv, item) => (prv += item.indicador.puntuacion), 0);
      });
    };

    this.chart.setValueSelector(valueSelector);
  }

  setData(data: any[]): void {
    this.chart.setData(data as SeguimientoDto[]);
  }

  getChartOptions(): Options {
    return this.chart.getChartOptions();
  }
}
