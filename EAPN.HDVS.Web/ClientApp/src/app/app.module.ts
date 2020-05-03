import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ApiClient } from './core/api/api.client';
import { environment } from 'src/environments/environment';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { AuthenticatedGuard } from './core/guards/authenticate.guard';
import { JwtModule, JWT_OPTIONS } from '@auth0/angular-jwt';
import { AuthenticationService } from './core/services/authentication.service';
import { LoaderComponent } from './shared/components/loader/loader.component';
import { AlertModule } from './shared/components/alert/alert.module';

export function jwtOptionsFactory(authenticationService: AuthenticationService) {
  return {
    tokenGetter: () => {
      return authenticationService.getValidToken();
    },
  };
}

const apiClientFactory = () => {
  return new ApiClient(environment.apiEndpoint);
};

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
  declarations: [AppComponent, LoaderComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    AlertModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      jwtOptionsProvider: {
        provide: JWT_OPTIONS,
        useFactory: jwtOptionsFactory,
        deps: [AuthenticationService],
      },
    }),
    AlertModule,
  ],
  providers: [
    { provide: ApiClient, useFactory: apiClientFactory },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
