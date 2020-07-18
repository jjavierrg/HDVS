import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { ISearchQuery, SearchType } from 'src/app/shared/models/search-query';
import { ApiClient, IFichaBusquedaDto, QueryData } from '../api/api.client';
import { BaseFilter, getFilterQuery, IBaseFilter } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';

@Injectable({ providedIn: 'root' })
export class SearchService {
  private querySubject: Subject<ISearchQuery> = new Subject<ISearchQuery>();

  constructor(private apiClient: ApiClient) {}

  public getQueryObservable(): Observable<ISearchQuery> {
    return this.querySubject.asObservable();
  }

  public submitQuery(query: ISearchQuery): void {
    this.querySubject.next(query);
  }

  public findCards(searchData: ISearchQuery): Observable<IFichaBusquedaDto[]> {
    const filters = this.getQueryFilters(searchData);
    return this.findCardsByQueryFilters(filters);
  }

  public findCardsBySearchType(type: SearchType, userId?: number, partnerId?: number): Observable<IFichaBusquedaDto[]> {
    if (!type) {
      return;
    }

    const filters: IBaseFilter[] = [];

    if (userId) {
      filters.push(new BaseFilter('UsuarioId', userId, FilterComparison.Equal, FilterUnion.And));
    }

    if (partnerId) {
      filters.push(new BaseFilter('OrganizacionId', partnerId, FilterComparison.Equal, FilterUnion.And));
    }

    switch (type) {
      case SearchType.user_completed:
        filters.push(new BaseFilter('Completa', true, FilterComparison.Equal, FilterUnion.And));
        break;
      case SearchType.user_incompleted:
        filters.push(new BaseFilter('Completa', false, FilterComparison.Equal, FilterUnion.And));
        break;
      case SearchType.user_outdated:
        const today = new Date();
        const dateValue = new Date(Date.UTC(today.getFullYear() - 1, today.getMonth(), today.getDate(), 0, 0, 0, 0));
        filters.push(new BaseFilter('FechaUltimaModificacion', dateValue.toISOString(), FilterComparison.LessThanOrEqual, FilterUnion.And));
        break;
    }

    return this.findCardsByQueryFilters(filters);
  }

  public findCardsByQueryFilters(filters: IBaseFilter[]): Observable<IFichaBusquedaDto[]> {
    const query = new QueryData({ filterParameters: getFilterQuery(filters) });
    return this.apiClient.getFichasFiltered(query).pipe(map((data) => data.data));
  }

  private getQueryFilters(query: ISearchQuery): IBaseFilter[] {
    const filters: IBaseFilter[] = [];

    if (!!query.idnumber) {
      filters.push(new BaseFilter('Dni', query.idnumber, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!query.rad) {
      filters.push(new BaseFilter('Codigo', query.rad, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!query.partnerId) {
      filters.push(new BaseFilter('OrganizacionId', query.partnerId, FilterComparison.Equal, FilterUnion.And));
    }

    if (!!query.name) {
      filters.push(new BaseFilter('Nombre', query.name, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!query.surname1) {
      filters.push(new BaseFilter('Apellido1', query.surname1, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!query.surname2) {
      filters.push(new BaseFilter('Apellido2', query.surname2, FilterComparison.Contains, FilterUnion.And));
    }

    if (!!query.birth) {
      filters.push(new BaseFilter('FechaNacimiento', query.birth.toDateString(), FilterComparison.Equal, FilterUnion.And));
    }

    return filters;
  }
}
