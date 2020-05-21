import { Injectable } from '@angular/core';
import { ApiClient, VistaPreviaFichaDto, QueryData } from '../api/api.client';
import { Observable } from 'rxjs';
import { IBaseFilter, BaseFilter, getFilterQuery } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';
import { map, filter } from 'rxjs/operators';
import { isDate } from 'util';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  constructor(private apiClient: ApiClient) {}

  public findPreviewCards(name: string, surname1: string, surname2: string, birth: Date): Observable<VistaPreviaFichaDto[]> {
    const filters: IBaseFilter[] = [];

    if (!!name) {
      filters.push(new BaseFilter('Nombre', name, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!surname1) {
      filters.push(new BaseFilter('Apellido1', surname1, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!surname2) {
      filters.push(new BaseFilter('Apellido2', surname2, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!birth && isDate(birth)) {
      filters.push(new BaseFilter('FechaNacimiento', birth.toDateString(), FilterComparison.Equal, FilterUnion.And));
    }

    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters) });
    return this.apiClient.getVistaPeviaFichas(query).pipe(map((data) => data.data));
  }
}