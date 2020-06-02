import { Injectable } from '@angular/core';
import { ApiClient, DimensionDto, SeguimientoDto } from '../api/api.client';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class IndicatorService {
  constructor(private apiClient: ApiClient) {}

  public getDimensions(): Observable<DimensionDto[]> {
    return this.apiClient.getDimensiones();
  }

  public getReview(id: number): Observable<SeguimientoDto> {
    return this.apiClient.getSeguimiento(id);
  }

  public saveReview(review: SeguimientoDto): Observable<SeguimientoDto> {
    if (review.id) {
      return this.apiClient.putSeguimiento(review.id, review).pipe(map(() => review));
    } else {
      return this.apiClient.postSeguimiento(review);
    }
  }
}
