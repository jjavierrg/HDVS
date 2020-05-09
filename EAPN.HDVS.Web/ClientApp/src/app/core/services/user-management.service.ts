import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, UsuarioDto, PerfilDto, RolDto } from '../api/api.client';
import { sha3_512 } from 'js-sha3';

@Injectable({
  providedIn: 'root',
})
export class UserManagementService {
  constructor(private apiClient: ApiClient) {}

  public getUsuarios(): Observable<UsuarioDto[]> {
    return this.apiClient.getUsuarios();
  }

  public getUsuario(id: number): Observable<UsuarioDto> {
    return this.apiClient.getUsuario(id);
  }

  public getPerfiles(): Observable<PerfilDto[]> {
    return this.apiClient.getPerfiles();
  }

  public getRoles(): Observable<RolDto[]> {
    return this.apiClient.getRoles();
  }

  public createUsuario(usuario: UsuarioDto): Observable<UsuarioDto> {
    if (!usuario) {
      return;
    }

    const dto = new UsuarioDto({ ...usuario, id: 0, clave: usuario.clave ? sha3_512(usuario.clave) : '' });
    return this.apiClient.postUsuario(dto);
  }

  public updateUsuario(usuario: UsuarioDto): Observable<void> {
    if (!usuario) {
      return;
    }

    const dto = new UsuarioDto({ ...usuario, clave: usuario.clave ? sha3_512(usuario.clave) : '' });
    return this.apiClient.putUsuario(dto.id, dto);
  }
}
