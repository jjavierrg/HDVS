import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { isDate } from 'util';
import { ApiClient, FichaDto, QueryData, VistaPreviaFichaDto, DatosFichaDto } from '../api/api.client';
import { BaseFilter, getFilterQuery, IBaseFilter } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';

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

  public getCard(cardId: number): Observable<FichaDto> {
    return this.apiClient.getFicha(cardId);
  }

  public getPersonalData(cardId: number): Observable<DatosFichaDto> {
    return this.apiClient.getDatosFicha(cardId);
  }

  public saveCard(card: FichaDto): Observable<FichaDto> {
    if (!card) {
      return of(null);
    }

    if (card.id) {
      return this.apiClient.putFicha(card.id, card).pipe(map((_) => card));
    } else {
      return this.apiClient.postFicha(card);
    }
  }
}
