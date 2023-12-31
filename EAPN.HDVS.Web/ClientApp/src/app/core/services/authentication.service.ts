import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { sha3_512 } from 'js-sha3';
import { BehaviorSubject, from, Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ApiClient, DatosUsuarioDto, LoginAttempDto, UserTokenDto } from '../api/api.client';

const helper = new JwtHelperService();
const validMinutesOffset = 10;

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private currentTokenSubject: BehaviorSubject<UserTokenDto>;

  constructor(private apiClient: ApiClient, private router: Router) {
    let token = localStorage.getItem(environment.tokenLocalStorageKey);
    if (!token) {
      token = sessionStorage.getItem(environment.tokenLocalStorageKey);
      if (token) {
        localStorage.setItem(environment.tokenLocalStorageKey, token);
      }
    }

    this.currentTokenSubject = new BehaviorSubject<UserTokenDto>(JSON.parse(token));
  }

  public getDatosUsuario(): Observable<DatosUsuarioDto> {
    return this.apiClient.getDatosUsuario();
  }

  public updateDatosUsuario(datos: DatosUsuarioDto): Observable<boolean> {
    const dto = new DatosUsuarioDto({
      ...datos,
      claveActual: datos.claveActual ? sha3_512(datos.claveActual) : '',
      nuevaClave: datos.nuevaClave ? sha3_512(datos.nuevaClave) : '',
    });
    return this.apiClient.putDatosUsuario(dto);
  }

  public getUserObservable(): Observable<any> {
    return this.currentTokenSubject.asObservable().pipe(
      map((token) => {
        return token == null ? null : helper.decodeToken(token.accessToken);
      })
    );
  }

  public login(username: string, password: string): Observable<UserTokenDto> {
    const LoginAttemp: LoginAttempDto = new LoginAttempDto({ password: sha3_512(password), email: username });
    return this.handleTokenRequest(this.apiClient.auth(LoginAttemp));
  }

  public logout(): void {
    // remove user from local storage to log user out
    localStorage.removeItem(environment.tokenLocalStorageKey);
    sessionStorage.removeItem(environment.tokenLocalStorageKey);
    this.currentTokenSubject.next(null);
  }

  public get isAuthenticated(): boolean {
    const currentToken = this.currentTokenSubject.getValue();
    if (currentToken == null) {
      return false;
    }
    if (!currentToken.accessToken) {
      return false;
    }

    return true;
  }

  public isAuthorized(allowedPermissions: string[], requireAll: boolean = false): Observable<boolean> {
    if (!this.isAuthenticated) {
      return of(false);
    }

    return this.getUserPermissions().pipe(
      map((userPermissions) => {
        if (!userPermissions || !userPermissions.length) {
          return false;
        }

        // check if the list of allowed permissions is empty, if empty, authorize the user to access the page
        if (allowedPermissions == null || allowedPermissions.length === 0) {
          return true;
        }

        if (requireAll) {
          return allowedPermissions.every((x) => userPermissions.some((y) => y.toLowerCase() === x.toLowerCase()));
        } else {
          return allowedPermissions.some((x) => userPermissions.some((y) => y.toLowerCase() === x.toLowerCase()));
        }
      })
    );
  }

  public getValidToken(): Observable<string> {
    const currenttoken = this.currentTokenSubject.getValue();
    if (this.isTokenValid(currenttoken)) {
      return of(currenttoken.accessToken);
    }

    return this.refreshToken(currenttoken).pipe(map((token) => (token ? token.accessToken : '')));
  }

  public getUser(): Observable<any> {
    return this.getValidToken().pipe(map((token) => (token ? helper.decodeToken(token) : {})));
  }

  public getUserPartnerId(): Observable<number> {
    return this.getUser().pipe(map((user) => +user['organizacion_id']));
  }

  public getUserId(): Observable<number> {
    return this.getUser().pipe(map((user) => +user['sub']));
  }

  public getUserPermissions(): Observable<string[]> {
    return this.getUser().pipe(
      map((user) => {
        if (!user) {
          return [];
        }

        try {
          const permissionProperty = Object.getOwnPropertyNames(user).find((x) => x.toLowerCase().endsWith('role'));
          if (!permissionProperty) {
            return [];
          }

          return user[permissionProperty];
        } catch (error) {
          return [];
        }
      })
    );
  }

  private isTokenValid(token: UserTokenDto): boolean {
    if (!token) {
      return false;
    }
    if (!token.accessToken) {
      return false;
    }

    try {
      return !helper.isTokenExpired(token.accessToken, validMinutesOffset * 60);
    } catch (error) {
      return false;
    }
  }

  private refreshToken(oldToken: UserTokenDto): Observable<UserTokenDto> {
    const promise = fetch(`${environment.apiEndpoint}/api/Auth/refresh`, {
      method: 'GET',
      credentials: 'include',
      headers: {
        Authorization: `Bearer ${oldToken.accessToken}`,
        'X-FP-API-KEY': 'chaptoken',
        'Content-Type': 'application/json',
      },
    });

    const observable: Observable<UserTokenDto> = from(promise.then(async (x) => UserTokenDto.fromJS(await x.json())));
    return this.handleTokenRequest(observable);
  }

  private handleTokenRequest(request: Observable<UserTokenDto>): Observable<UserTokenDto> {
    return request.pipe(
      catchError((err) => {
        localStorage.removeItem(environment.tokenLocalStorageKey);
        sessionStorage.removeItem(environment.tokenLocalStorageKey);
        this.currentTokenSubject.next(null);
        this.router.navigate(['login']);
        return throwError(err);
      }),
      map((token: UserTokenDto) => {
        localStorage.setItem(environment.tokenLocalStorageKey, JSON.stringify(token));
        sessionStorage.setItem(environment.tokenLocalStorageKey, JSON.stringify(token));
        this.currentTokenSubject.next(token);

        if (!token) {
          this.router.navigate(['login']);
        }

        return token;
      })
    );
  }
}
