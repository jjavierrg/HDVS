import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, PerfilDto, PermisoDto } from '../api/api.client';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  constructor(private apiClient: ApiClient) {}

  public getPerfiles(): Observable<PerfilDto[]> {
    return this.apiClient.getPerfiles();
  }

  public getPerfil(id: number): Observable<PerfilDto> {
    return this.apiClient.getPerfil(id);
  }

  public createPerfil(perfil: PerfilDto): Observable<PerfilDto> {
    if (!perfil) {
      return;
    }

    return this.apiClient.postPerfil(perfil);
  }

  public updatePerfil(perfil: PerfilDto): Observable<void> {
    if (!perfil) {
      return;
    }

    return this.apiClient.putPerfil(perfil.id, perfil);
  }

  public async deletePerfiles(perfiles: PerfilDto[]): Promise<boolean> {
    if (!perfiles || !perfiles.length) {
      return;
    }

    for (const perfil of perfiles) {
      await this.apiClient.deletePerfil(perfil.id).toPromise();
    }

    return true;
  }
}
