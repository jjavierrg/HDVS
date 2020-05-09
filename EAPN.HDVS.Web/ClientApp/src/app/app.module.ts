import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ApiClient, apiEndpoint } from './core/api/api.client';
import { AuthenticatedGuard } from './core/guards/authenticate.guard';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { CoreModule } from './core/core.module';
import { environment } from 'src/environments/environment';
import { LoaderModule } from './shared/modules/loader/loader.module';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () => import('./modules/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: '',
    loadChildren: () => import('./modules/dashboard/dashboard.module').then((m) => m.DashboardModule),
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
    CoreModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    RouterModule.forRoot(routes),
    LoaderModule,
  ],
  providers: [
    ApiClient,
    {
      provide: apiEndpoint,
      useValue: environment.apiEndpoint,
    },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
