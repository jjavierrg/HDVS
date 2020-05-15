import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ApiClient, UsuarioDto, PerfilDto, PermisoDto, QueryData, AsociacionDto } from '../api/api.client';
import { sha3_512 } from 'js-sha3';
import { map, filter } from 'rxjs/operators';
import { BaseFilter, IBaseFilter, getFilterQuery } from '../filters/basefilter';
import { FilterComparison, FilterUnion } from '../filters/filter.enum';

@Injectable({
  providedIn: 'root',
})
export class UserManagementService {
  constructor(private apiClient: ApiClient) {}

  public getUsuarios(): Observable<UsuarioDto[]> {
    return this.apiClient.getUsuarios();
  }

  public getUsuariosByPartnerId(partnerId: number): Observable<UsuarioDto[]> {
    const filters: IBaseFilter[] = [new BaseFilter('AsociacionId', +partnerId, FilterComparison.Equal, FilterUnion.And)];
    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters) });
    return this.apiClient.getUsuariosFiltered(query).pipe(map((result) => result.data));
  }

  public getUsuario(id: number): Observable<UsuarioDto> {
    return this.apiClient.getUsuario(id);
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

  public async deleteUsuarios(usuarios: UsuarioDto[]): Promise<boolean> {
    if (!usuarios || !usuarios.length) {
      return;
    }

    for (const usuario of usuarios) {
      await this.apiClient.deleteUsuario(usuario.id).toPromise();
    }

    return true;
  }

  public emailTaken(email: string, id?: number): Observable<boolean> {
    const filters: IBaseFilter[] = [new BaseFilter('Email', email, FilterComparison.Equal, FilterUnion.And)];

    if (id) {
      filters.push(new BaseFilter('Id', id, FilterComparison.NotEqual, FilterUnion.And));
    }

    const query: QueryData = new QueryData({ filterParameters: getFilterQuery(filters), pageSize: 1 });
    return this.apiClient.getUsuariosFiltered(query).pipe(map((result) => result.total > 0));
  }
}
