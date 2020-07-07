import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { isDate } from 'util';
import { ApiClient, FichaDto, QueryData, VistaPreviaFichaDto, DatosFichaDto, IVistaPreviaFichaDto, IFichaDto, IDatosFichaDto, ResumenExpedientesDto } from '../api/api.client';
import { BaseFilter, getFilterQuery, IBaseFilter } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';
import { DatePipe } from '@angular/common';
import { ISearchQuery } from 'src/app/shared/models/search-query';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  constructor(private apiClient: ApiClient, private datepipe: DatePipe) {}

  public findPreviewCards(searchData: ISearchQuery): Observable<IVistaPreviaFichaDto[]> {
    const query = this.getQuery(searchData);
    return this.apiClient.getVistaPeviaFichas(query).pipe(map((data) => data.data));
  }

  public getCard(cardId: number): Observable<IFichaDto> {
    return this.apiClient.getFicha(cardId);
  }

  public getAllCards(): Observable<IFichaDto[]> {
    return this.apiClient.getFichas();
  }

  public getPersonalData(cardId: number): Observable<IDatosFichaDto> {
    return this.apiClient.getDatosFicha(cardId);
  }

  public saveCard(card: FichaDto): Observable<FichaDto> {
    if (!card) {
      return of(null);
    }

    card.codigo = this.generateRadCode(card);

    if (card.dni) {
      card.dni = card.dni.replace(/\W/g, '');
    }

    if (card.id) {
      return this.apiClient.putFicha(card.id, card).pipe(map((_) => card));
    } else {
      return this.apiClient.postFicha(card);
    }
  }

  public getDashboad(): Observable<ResumenExpedientesDto> {
    return this.apiClient.getResumen();
  }

  private getQuery(query: ISearchQuery): QueryData {
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

    return new QueryData({ filterParameters: getFilterQuery(filters) });
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
