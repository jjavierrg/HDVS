import { HTTP_INTERCEPTORS, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { mergeMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthenticationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;
    const exclusions: string[] = environment.tokenExcludeEndpoints;
    const includeToken = req.url.startsWith(environment.apiEndpoint) && !exclusions.some((x) => req.url.toLowerCase().includes(x));

    if (!includeToken) {
      return next.handle(authReq);
    }

    return this.authService.getValidToken().pipe(
      mergeMap((token) => {
        if (token) {
          authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, 'Bearer ' + token) });
        }

        return next.handle(authReq);
      })
    );
  }
}
