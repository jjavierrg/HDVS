import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, AsociacionDto } from '../api/api.client';

@Injectable({
  providedIn: 'root',
})
export class PartnerService {
  constructor(private apiClient: ApiClient) {}

  public getAsociaciones(): Observable<AsociacionDto[]> {
    return this.apiClient.getAsociaciones();
  }

  public getAsociacion(id: number): Observable<AsociacionDto> {
    return this.apiClient.getAsociacion(id);
  }

  public createAsociacion(asociacion: AsociacionDto): Observable<AsociacionDto> {
    if (!asociacion) {
      return;
    }

    return this.apiClient.postAsociacion(asociacion);
  }

  public updateAsociacion(asociacion: AsociacionDto): Observable<void> {
    if (!asociacion) {
      return;
    }

    return this.apiClient.putAsociacion(asociacion.id, asociacion);
  }

  public async deleteAsociaciones(asociaciones: AsociacionDto[]): Promise<boolean> {
    if (!asociaciones || !asociaciones.length) {
      return;
    }

    for (const asociacion of asociaciones) {
      await this.apiClient.deleteAsociacion(asociacion.id).toPromise();
    }

    return true;
  }
}
