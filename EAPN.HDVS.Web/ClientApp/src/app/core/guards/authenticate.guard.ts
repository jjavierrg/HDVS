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

    return this.isAuthorized(fullPath, route.data ? route.data.allowedRoles : []);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.isAuthorized(state.url, childRoute.data.allowedRoles);
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean | UrlTree> | boolean {
    return this.isAuthorized(state.url, next.data.allowedRoles);
  }

  private isAuthorized(url: string, allowedRoles: string[]): Observable<boolean> {
    if (!this.auth.isAuthenticated) {
      this.router.navigateByUrl('/login');
      return of(false);
    }

    return this.auth.isAuthorized(allowedRoles).pipe(map((isAuthorized) => {
        if (!isAuthorized) {
          this.router.navigate(['']);
          return false;
        }

        return true;
      })
    );
  }
}
