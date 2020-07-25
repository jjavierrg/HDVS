import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DatosGraficaDTO } from 'src/app/core/api/api.client';
import { ChartFieldSelector } from 'src/app/core/chart/chart-field.selector';

@Injectable({ providedIn: 'root' })
export class SelectorsService {
  constructor(private translate: TranslateService) {}

  public getSelectors(): ChartFieldSelector<DatosGraficaDTO>[] {
    const noDataMessage: string = this.translate.instant('core.no-especificado');
    return [
      new ChartFieldSelector<DatosGraficaDTO>(0, {
        name: 'Sin determinar',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(1, {
        name: this.translate.instant('ficha.sexo'),
        descriptorProperty: 'sexo',
        valueProperty: 'sexoId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(2, {
        name: this.translate.instant('ficha.genero'),
        descriptorProperty: 'genero',
        valueProperty: 'generoId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(3, {
        name: this.translate.instant('ficha.municipio'),
        descriptorProperty: 'municipio',
        valueProperty: 'municipioId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(4, {
        name: this.translate.instant('ficha.provincia'),
        descriptorProperty: 'provincia',
        valueProperty: 'provinciaId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(5, {
        name: this.translate.instant('ficha.nacionalidad'),
        descriptorProperty: 'nacionalidad',
        valueProperty: 'nacionalidadId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(6, {
        name: this.translate.instant('ficha.pais-origen'),
        descriptorProperty: 'origen',
        valueProperty: 'origenId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(7, {
        name: this.translate.instant('ficha.empadronamiento'),
        descriptorProperty: 'padron',
        valueProperty: 'padronId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(8, {
        name: this.translate.instant('ficha.situacion-administrativa'),
        descriptorProperty: 'situacionAdministrativa',
        valueProperty: 'situacionAdministrativaId',
        emptyDescriptionMessage: noDataMessage,
      }),
      new ChartFieldSelector<DatosGraficaDTO>(9, {
        name: this.translate.instant('comun.grados'),
        descriptorProperty: 'rango',
        valueProperty: 'rangoId',
        emptyDescriptionMessage: noDataMessage,
      }),
    ];
  }
}
