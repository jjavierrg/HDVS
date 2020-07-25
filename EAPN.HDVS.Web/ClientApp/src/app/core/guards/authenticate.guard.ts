import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
  CanActivateChild,
  CanLoad,
  Route,
  UrlSegment,
  Data,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticatedGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private auth: AuthenticationService, private router: Router) {}

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    const fullPath = segments.reduce((path, currentSegment) => {
      return `${path}/${currentSegment.path}`;
    }, '');

    const params = this.getDataParams(route.data);
    return this.isAuthorized(fullPath, params.allowedPermissions, params.requireAll);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const params = this.getDataParams(childRoute.data);
    return this.isAuthorized(state.url, params.allowedPermissions, params.requireAll);
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean | UrlTree> | boolean {
    const params = this.getDataParams(next.data);
    return this.isAuthorized(state.url, params.allowedPermissions, params.requireAll);
  }

  private getDataParams(data: Data): { allowedPermissions: string[]; requireAll: boolean } {
    const result = { allowedPermissions: [], requireAll: false };
    if (!!data) {
      result.allowedPermissions = (data.allowedPermissions || []);
      result.requireAll = (data.requireAll || false);
    }
    return result;
  }

  private isAuthorized(url: string, allowedPermissions: string[], requireAll: boolean): Observable<boolean> {
    if (!this.auth.isAuthenticated) {
      this.router.navigate(['login'], { queryParams: { returnUrl: url }});
      return of(false);
    }

    return this.auth.isAuthorized(allowedPermissions, requireAll).pipe(
      map((isAuthorized) => {
        if (!isAuthorized) {
          this.router.navigate(['']);
          return false;
        }

        return true;
      })
    );
  }
}
