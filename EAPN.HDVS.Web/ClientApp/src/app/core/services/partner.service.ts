import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, OrganizacionDto } from '../api/api.client';

@Injectable({
  providedIn: 'root',
})
export class PartnerService {
  constructor(private apiClient: ApiClient) {}

  public getOrganizaciones(): Observable<OrganizacionDto[]> {
    return this.apiClient.getOrganizaciones();
  }

  public getOrganizacion(id: number): Observable<OrganizacionDto> {
    return this.apiClient.getOrganizacion(id);
  }

  public createOrganizacion(organizacion: OrganizacionDto): Observable<OrganizacionDto> {
    if (!organizacion) {
      return;
    }

    return this.apiClient.postOrganizacion(organizacion);
  }

  public updateOrganizacion(organizacion: OrganizacionDto): Observable<void> {
    if (!organizacion) {
      return;
    }

    return this.apiClient.putOrganizacion(organizacion.id, organizacion);
  }

  public async deleteOrganizaciones(organizaciones: OrganizacionDto[]): Promise<boolean> {
    if (!organizaciones || !organizaciones.length) {
      return;
    }

    for (const organizacion of organizaciones) {
      await this.apiClient.deleteOrganizacion(organizacion.id).toPromise();
    }

    return true;
  }
}
