import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ISearchQuery } from 'src/app/shared/models/search-query';
import { ApiClient, DatosGraficaDTO, QueryData } from '../api/api.client';
import { getFilterQuery, IBaseFilter } from '../filters/basefilter';

@Injectable({
  providedIn: 'root'
})
export class GraphService {

  constructor(private apiClient: ApiClient) { }

  public getGraphData(searchData: ISearchQuery): Observable<DatosGraficaDTO[]> {
    // const filters: IBaseFilter[] = [new BaseFilter('FichaId', cardId, FilterComparison.Equal, FilterUnion.And)];
    const filters: IBaseFilter[] = [];
    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters) });

    return this.apiClient.getChartData(query);
  }
}
