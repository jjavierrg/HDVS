import { Injectable } from '@angular/core';
import { ApiClient, DimensionDto } from '../api/api.client';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IndicatorService {

  constructor(private apiClient: ApiClient) { }

  public getDimensions(): Observable<DimensionDto[]> {
    return this.apiClient.getDimensiones();
  }
}
