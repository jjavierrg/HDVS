import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ApiClient, apiEndpoint } from './core/api/api.client';
import { environment } from 'src/environments/environment';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { AuthenticatedGuard } from './core/guards/authenticate.guard';
import { JwtModule, JWT_OPTIONS } from '@auth0/angular-jwt';
import { AuthenticationService } from './core/services/authentication.service';

export function jwtOptionsFactory(authenticationService: AuthenticationService) {
  return {
    tokenGetter: () => {
      return authenticationService.getValidToken().toPromise();
    },
  };
}

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () => import('./modules/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: '',
    loadChildren: () => import('./modules/home/home.module').then((m) => m.HomeModule),
    canLoad: [AuthenticatedGuard],
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'dashboard',
  },
];

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      jwtOptionsProvider: {
        provide: JWT_OPTIONS,
        useFactory: jwtOptionsFactory,
        deps: [AuthenticationService],
      },
    }),
  ],
  providers: [
    ApiClient,
    {
      provide: apiEndpoint,
      useValue: environment.apiEndpoint,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
