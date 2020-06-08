import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { isDate } from 'util';
import { ApiClient, FichaDto, QueryData, VistaPreviaFichaDto, DatosFichaDto } from '../api/api.client';
import { BaseFilter, getFilterQuery, IBaseFilter } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  constructor(private apiClient: ApiClient, private datepipe: DatePipe) {}

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

    card.codigo = this.generateRadCode(card);
    card.dni = card.dni.replace(/\W/g, '');

    if (card.id) {
      return this.apiClient.putFicha(card.id, card).pipe(map((_) => card));
    } else {
      return this.apiClient.postFicha(card);
    }
  }

  private generateRadCode(card: FichaDto): string {
    if (!card.fechaNacimiento || !card.apellido1) {
      return '';
    }

    if (!card.apellido2 && !card.nombre) {
      return '';
    }

    const apellido1: string = card.apellido1.replace(/\W/g, '').padEnd(2, 'X').toUpperCase();
    const fechNac: string = this.datepipe.transform(card.fechaNacimiento, 'ddMMyyyy');

    if (card.apellido2) {
      const apellido2: string = card.apellido2.replace(/\W/g, '').padEnd(2, 'X').toUpperCase();
      return `${apellido1.substr(0, 2)}${apellido2.substr(0, 2)}${fechNac}`;
    } else {
      const nombre: string = card.nombre.replace(/\W/g, '').padEnd(2, 'X').toUpperCase();
      return `${nombre.substr(0, 2)}${apellido1.substr(0, 2)}${fechNac}`;
    }
  }
}
