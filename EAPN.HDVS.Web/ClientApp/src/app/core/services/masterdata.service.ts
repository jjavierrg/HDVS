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

  public getAsociaciones(): Observable<MasterDataDto[]> {
    return this.apiClient.getAsociacionesAsMasterData();
  }
}
