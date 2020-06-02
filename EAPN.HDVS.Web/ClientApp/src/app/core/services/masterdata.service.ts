import { Injectable } from '@angular/core';
import { ApiClient, MasterDataDto, QueryData } from '../api/api.client';
import { Observable } from 'rxjs';
import { IBaseFilter, BaseFilter, getFilterQuery } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';
import { map } from 'rxjs/operators';

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

  public getUsuariosByPartnerId(partnerId: number): Observable<MasterDataDto[]> {
    const filters: IBaseFilter[] = [new BaseFilter('OrganizacionId', +partnerId, FilterComparison.Equal, FilterUnion.And)];
    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters) });
    return this.apiClient.getUsuariosFiltered(query).pipe(map((result) => result.data.map((x) => new MasterDataDto({
      id: x.id,
      descripcion: `${x.nombre} ${x.apellidos}`.trim()
    }))));
  }
}
