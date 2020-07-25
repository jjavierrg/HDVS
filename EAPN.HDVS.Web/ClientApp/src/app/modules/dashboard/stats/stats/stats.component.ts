import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import * as Highcharts from 'highcharts';
import HC_drilldown from 'highcharts/modules/drilldown';
import HC_exporting from 'highcharts/modules/exporting';
import { DatosGraficaDTO, DimensionDto, MasterDataDto, RangoDto } from 'src/app/core/api/api.client';
import { ChartDirector } from 'src/app/core/chart/chart-director';
import { ChartFieldSelector } from 'src/app/core/chart/chart-field.selector';
import { ICategory, ICategorySelector, IChartLabels, ISerie, ISerieSelector, IValueSelector } from 'src/app/core/chart/types';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AlertService } from 'src/app/core/services/alert.service';
import { GraphService } from 'src/app/core/services/graph.service';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { CustomBuilder } from 'src/app/shared/chart/custom-builder';
import { SelectorsService } from '../selectors/selectors.service';

class StatsFilters {
  public searchByAllOrganizacion: boolean;
  public searchByFechaAlta: boolean;
  public searchBySexo: boolean;
  public searchByGenero: boolean;
  public searchByNacionalidad: boolean;
  public searchByPaisOrigen: boolean;
  public searchByCodPostal: boolean;
  public idOrganizacion: boolean;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idSexo: boolean;
  public idGenero: boolean;
  public idNacionalidad: boolean;
  public idPaisOrigen: boolean;
  public codPostal: boolean;
}

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss'],
})
export class StatsComponent implements OnInit {
  public isCollapsed: boolean = true;
  public filters: StatsFilters = new StatsFilters();
  public sexos: MasterDataDto[];
  public generos: MasterDataDto[];
  public paises: MasterDataDto[];
  public dimensiones: DimensionDto[];
  public rangos: RangoDto[];
  public permissions = Permissions;
  public highcharts: typeof Highcharts = Highcharts;
  public chartOptions: Highcharts.Options;
  public selectors: ChartFieldSelector<DatosGraficaDTO>[];
  public category: number = 0;
  public serie: number = 0;

  constructor(
    private masterdataService: MasterdataService,
    private indicatorService: IndicatorService,
    private graphService: GraphService,
    private alertService: AlertService,
    private charDirector: ChartDirector,
    private translate: TranslateService,
    private selectorsService: SelectorsService
  ) {
    HC_exporting(Highcharts);
    HC_drilldown(Highcharts);

    this.selectors = this.selectorsService.getSelectors();
  }

  async ngOnInit(): Promise<void> {
    try {
      const [sexos, paises, dimensiones, rangos] = await Promise.all([
        this.masterdataService.getSexos().toPromise(),
        this.masterdataService.getPaises().toPromise(),
        this.indicatorService.getDimensions().toPromise(),
        this.masterdataService.getRangos().toPromise(),
      ]);

      this.sexos = sexos;
      this.paises = paises;
      this.dimensiones = dimensiones;
      this.rangos = rangos;

      this.isCollapsed = false;
    } catch (err) {
      this.alertService.error(err);
    }
  }

  public async generateChart(): Promise<void> {
    this.chartOptions = null;
    const category = this.selectors.find((x) => x.id === this.category);
    const serie = this.selectors.find((x) => x.id === this.serie);
    const title = category.id === 0 || serie.id === 0 ? 'titulo-grafica' : 'titulo-grafica-compuesta';

    const items = await this.graphService.getGraphData(null).toPromise();
    const builder = new CustomBuilder<DatosGraficaDTO>(this.translate);
    const labels: IChartLabels = {
      chartTitle: this.translate.instant(`estadisticas.${title}`, {
        categoria: category && category.id > 0 ? category.descripcion : '',
        serie: serie && serie.id > 0 ? serie.descripcion : '',
      }),
      xAxisTitle: category && category.id > 0 ? category.descripcion : '',
      yAxisTitle: this.translate.instant('comun.fichas'),
    };

    this.charDirector.setBuilder(builder);
    builder.serieSelector = this.getSerieSelector(serie);
    builder.categorySelector = this.getCategorySelector(category);
    builder.valueSelector = this.getValueSelector(category);

    await this.charDirector.buildGraph(items, { labels });
    this.chartOptions = this.charDirector.getChartOptions();
  }

  private getCategorySelector(category: ChartFieldSelector<DatosGraficaDTO>): ICategorySelector<DatosGraficaDTO> {
    if (!category) {
      return null;
    }

    return function (data: DatosGraficaDTO[]): ICategory[] {
      return data
        .map((x) => ({ id: category.getValue(x), value: category.getDescription(x) }))
        .filter((thing, i, arr) => {
          return arr.indexOf(arr.find((t) => t.id === thing.id)) === i;
        });
    };
  }

  private getSerieSelector(serie: ChartFieldSelector<DatosGraficaDTO>): ISerieSelector<DatosGraficaDTO> {
    if (!serie) {
      return null;
    }

    return function (data: DatosGraficaDTO): ISerie {
      return { id: serie.getValue(data), type: 'column', value: serie.getDescription(data) };
    };
  }

  private getValueSelector(cat: ChartFieldSelector<DatosGraficaDTO>): IValueSelector<DatosGraficaDTO> {
    if (!cat) {
      return null;
    }

    return function (data: DatosGraficaDTO[], category: ICategory): any {
      return { name: category.value, y: data.filter((x) => cat.getValue(x) === category.id).length };
    };
  }
}
