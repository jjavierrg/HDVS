import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, ReplaySubject, throwError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map, catchError, retry } from 'rxjs/operators';
import { ApiClient, LoginAttempDto, UserTokenDto, RefreshTokenAttempDto } from '../api/api.client';
import { JwtHelperService } from '@auth0/angular-jwt';
import { sha3_512 } from 'js-sha3';

const helper = new JwtHelperService();
const validMinutesOffset = 2;

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private currentTokenSubject: BehaviorSubject<UserTokenDto>;

  constructor(private apiClient: ApiClient) {
    this.currentTokenSubject = new BehaviorSubject<UserTokenDto>(JSON.parse(localStorage.getItem(environment.tokenLocalStorageKey)));
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

  public isAuthorized(allowedRoles: string[]): Observable<boolean> {
    if (!this.isAuthenticated) {
      return of(false);
    }

    return this.getValidToken().pipe(
      map((accessToken) => {
        const user = helper.decodeToken(accessToken);
        const userRoles: string[] = user.roles;

        // check if the list of allowed roles is empty, if empty, authorize the user to access the page
        if (allowedRoles == null || allowedRoles.length === 0) {
          return true;
        }

        return allowedRoles.some((x) => userRoles.some((y) => y.toLowerCase() === x.toLowerCase()));
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
    const currenttoken = this.currentTokenSubject.getValue();
    if (this.isTokenValid(currenttoken)) {
      return of(currenttoken.accessToken);
    }

    return this.refreshToken(currenttoken).pipe(map((token) => (token ? token.accessToken : '')));
  }

  private getUserIdFromToken(token: UserTokenDto): number {
    if (!token) {
      return -1;
    }

    try {
      const decoded = helper.decodeToken(token.accessToken);
      return +decoded.sub;
    } catch (error) {
      return -1;
    }
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
    const refreshToken = oldToken ? oldToken.refreshToken : '';
    const userId = this.getUserIdFromToken(oldToken);
    const request: RefreshTokenAttempDto = new RefreshTokenAttempDto({ refreshToken, userId });
    return this.handleTokenRequest(this.apiClient.refresh(request));
  }

  private handleTokenRequest(request: Observable<UserTokenDto>): Observable<UserTokenDto> {
    return request.pipe(
      catchError((err) => {
        localStorage.removeItem(environment.tokenLocalStorageKey);
        this.currentTokenSubject.next(null);
        return null;
      }),
      map((token: UserTokenDto) => {
        localStorage.setItem(environment.tokenLocalStorageKey, JSON.stringify(token));
        this.currentTokenSubject.next(token);
        return token;
      })
    );
  }
}
