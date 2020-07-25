import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ISearchQuery } from 'src/app/shared/models/search-query';
import { ApiClient, DatosGraficaDTO, GraphQuery, QueryData } from '../api/api.client';
import { BaseFilter, getFilterQuery, IBaseFilter } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';

export class StatsFilters {
  public searchByAllOrganizacion: boolean;
  public searchByFechaAlta: boolean;
  public searchBySexo: boolean;
  public searchByGenero: boolean;
  public searchByNacionalidad: boolean;
  public searchByPaisOrigen: boolean;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idSexo: boolean;
  public idGenero: boolean;
  public idNacionalidad: boolean;
  public idPaisOrigen: boolean;
  public rangos: number[] = [];
}

@Injectable({
  providedIn: 'root',
})
export class GraphService {
  constructor(private apiClient: ApiClient) {}

  public getGraphData(filters: StatsFilters): Observable<DatosGraficaDTO[]> {
    const queryFilters: IBaseFilter[] = [];

    if (filters.searchByFechaAlta) {
      if (filters.fechaDesde) {
        queryFilters.push(new BaseFilter('FechaAlta', filters.fechaDesde, FilterComparison.GreaterThanOrEqual, FilterUnion.And));
      }
      if (filters.fechaHasta) {
        queryFilters.push(new BaseFilter('FechaAlta', filters.fechaHasta, FilterComparison.LessThanOrEqual, FilterUnion.And));
      }
    }

    if (filters.searchBySexo && filters.idSexo) {
      queryFilters.push(new BaseFilter('SexoId', filters.idSexo, FilterComparison.Equal, FilterUnion.And));
    }

    if (filters.searchByGenero && filters.idGenero) {
      queryFilters.push(new BaseFilter('GeneroId', filters.idGenero, FilterComparison.Equal, FilterUnion.And));
    }

    if (filters.searchByNacionalidad && filters.idNacionalidad) {
      queryFilters.push(new BaseFilter('NacionalidadId', filters.idNacionalidad, FilterComparison.Equal, FilterUnion.And));
    }

    if (filters.searchByPaisOrigen && filters.idPaisOrigen) {
      queryFilters.push(new BaseFilter('OrigenId', filters.idPaisOrigen, FilterComparison.Equal, FilterUnion.And));
    }

    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(queryFilters) });
    return this.apiClient.getChartData(new GraphQuery({ globalData: filters.searchByAllOrganizacion, query, rangos: filters.rangos }));
  }
}
