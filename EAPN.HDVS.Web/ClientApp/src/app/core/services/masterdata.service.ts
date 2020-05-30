import { Injectable } from '@angular/core';
import { ApiClient, MasterDataDto } from '../api/api.client';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MasterdataService {
  constructor(private apiClient: ApiClient) {}

  public getPermisos(): Observable<MasterDataDto[]> {
    return this.apiClient.getPermisosAsMasterdata();
  }

  public getPerfiles(): Observable<MasterDataDto[]> {
    return this.apiClient.getPerfilesAsMasterData();
  }

  public getOrganizaciones(): Observable<MasterDataDto[]> {
    return this.apiClient.getOrganizacionesAsMasterData();
  }

  public getGenders(): Observable<MasterDataDto[]> {
    return this.apiClient.getSexosAsMasterData();
  }

  public getCountries(): Observable<MasterDataDto[]> {
    return this.apiClient.getPaisesAsMasterData();
  }

  public getProvincias(): Observable<MasterDataDto[]> {
    return this.apiClient.getProvinciasAsMasterData();
  }

  public getMunicipios(): Observable<MasterDataDto[]> {
    return this.apiClient.getMunicipiosAsMasterData();
  }

  public getMunicipiosByProvincia(provinciaId: number): Observable<MasterDataDto[]> {
    return this.apiClient.getMunicipiosByProvinciaIdAsMasterData(provinciaId);
  }

  public getSituacionesAdministrativas(): Observable<MasterDataDto[]> {
    return this.apiClient.getSituacionesAdministrativasAsMasterData();
  }
}
