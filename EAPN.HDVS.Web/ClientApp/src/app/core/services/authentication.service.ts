import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, ReplaySubject, throwError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map, catchError, retry } from 'rxjs/operators';
import { ApiClient, LoginAttempDto, UserTokenDto, RefreshTokenAttempDto } from '../api/api.client';
import { JwtHelperService } from '@auth0/angular-jwt';
import { sha3_512 } from 'js-sha3';
import { decode } from 'punycode';

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

  public async login(username: string, password: string): Promise<UserTokenDto> {
    const LoginAttemp: LoginAttempDto = new LoginAttempDto({ password: sha3_512(password), email: username });
    return await this.handleTokenRequest(this.apiClient.auth(LoginAttemp));
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

  public async isAuthorized(allowedRoles: string[]): Promise<boolean> {
    if (!this.isAuthenticated) {
      return false;
    }

    try {
      const user = await this.getUser();
      const userRoles: string[] = user.roles;

      if (allowedRoles == null || allowedRoles.length === 0) {
        return true;
      }

      return allowedRoles.some((x) => userRoles.some((y) => y.toLowerCase() === x.toLowerCase()));
    } catch (error) {
      return false;
    }
  }

  public async getValidToken(): Promise<string> {
    let currenttoken = this.currentTokenSubject.getValue();
    if (!this.isTokenValid(currenttoken)) {
      currenttoken = await this.refreshToken(currenttoken);
    }

    return !currenttoken ? '' : currenttoken.accessToken;
  }

  public async getUser(): Promise<any> {
    let currenttoken = this.currentTokenSubject.getValue();
    if (!this.isTokenValid(currenttoken)) {
      currenttoken = await this.refreshToken(currenttoken);
    }

    try {
      return helper.decodeToken(currenttoken.accessToken);
    } catch (error) {
      return null;
    }
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

  private async refreshToken(oldToken: UserTokenDto): Promise<UserTokenDto> {
    const refreshToken = oldToken ? oldToken.refreshToken : '';
    const userId = this.getUserIdFromToken(oldToken);
    const request: RefreshTokenAttempDto = new RefreshTokenAttempDto({ refreshToken, userId });
    return await this.handleTokenRequest(this.apiClient.refresh(request));
  }

  private async handleTokenRequest(request: Promise<UserTokenDto>): Promise<UserTokenDto> {
    try {
      const token = await request;
      localStorage.setItem(environment.tokenLocalStorageKey, JSON.stringify(token));
      this.currentTokenSubject.next(token);
      return token;
    } catch (error) {
      localStorage.removeItem(environment.tokenLocalStorageKey);
      this.currentTokenSubject.next(null);
      throw error;
    }
  }
}
