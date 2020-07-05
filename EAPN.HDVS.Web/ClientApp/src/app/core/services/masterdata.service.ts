import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, ConfiguracionDto, MasterDataDto, RangoDto } from '../api/api.client';

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

  public getSexos(): Observable<MasterDataDto[]> {
    return this.apiClient.getSexosAsMasterData();
  }

  public getPaises(): Observable<MasterDataDto[]> {
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

  public getEmpadronamientos(): Observable<MasterDataDto[]> {
    return this.apiClient.getEmpadronamientosAsMasterData();
  }

  public getUsuarios(): Observable<MasterDataDto[]> {
    return this.apiClient.getUsuariosAsMasterdata();
  }

  public getConfiguracion(): Observable<ConfiguracionDto> {
    return this.apiClient.getConfiguracion(1);
  }

  public saveConfiguracion(configuracion: ConfiguracionDto): Observable<void> {
    return this.apiClient.putConfiguracion(1, configuracion);
  }

  public getRangos(): Observable<RangoDto[]> {
    return this.apiClient.getRangos();
  }

  public saveRangos(rangos: RangoDto[]): Observable<void> {
    return this.apiClient.postRangos(rangos);
  }
}
