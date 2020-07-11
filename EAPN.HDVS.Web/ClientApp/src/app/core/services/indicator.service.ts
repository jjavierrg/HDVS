import { Injectable } from '@angular/core';
import { ApiClient, DimensionDto, SeguimientoDto, CategoriaDto, QueryData } from '../api/api.client';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { IBaseFilter, BaseFilter, getFilterQuery } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';

@Injectable({
  providedIn: 'root',
})
export class IndicatorService {
  constructor(private apiClient: ApiClient) {}

  public getDimensions(): Observable<DimensionDto[]> {
    return this.apiClient.getDimensiones();
  }

  public getDimensionesByCategorias(ids: number[]): Observable<DimensionDto[]> {
    return this.apiClient.getDimensionesByCategorias(ids);
  }

  public getReview(id: number): Observable<SeguimientoDto> {
    return this.apiClient.getSeguimiento(id);
  }

  public getCardReviews(cardId: number): Observable<SeguimientoDto[]> {
    const filters: IBaseFilter[] = [new BaseFilter('FichaId', cardId, FilterComparison.Equal, FilterUnion.And)];
    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters) });

    return this.apiClient.getSeguimientosFiltered(query).pipe(map(x => x.data));
  }

  public saveReview(review: SeguimientoDto): Observable<SeguimientoDto> {
    if (review.id) {
      return this.apiClient.putSeguimiento(review.id, review).pipe(map(() => review));
    } else {
      return this.apiClient.postSeguimiento(review).pipe(tap(x => review.id = x.id));
    }
  }

  public async deleteReviews(reviews: SeguimientoDto[]): Promise<boolean> {
    if (!reviews || !reviews.length) {
      return;
    }

    for (const review of reviews) {
      await this.apiClient.deleteSeguimiento(review.id).toPromise();
    }

    return true;
  }
}
