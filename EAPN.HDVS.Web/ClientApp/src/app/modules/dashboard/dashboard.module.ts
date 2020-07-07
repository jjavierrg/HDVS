import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { HeaderModule } from 'src/app/core/header/header.module';
import { FooterComponent } from 'src/app/core/footer/footer/footer.component';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AlertModule } from 'src/app/shared/modules/alert/alert.module';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
      },
      {
        path: 'estadisticas',
        loadChildren: () => import('./stats/stats.module').then((m) => m.StatsModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.stats.access] },
      },
      {
        path: 'usuarios',
        loadChildren: () => import('./user-management/user-management.module').then((m) => m.UserManagementModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.usermanagement.access] },
      },
      {
        path: 'perfiles',
        loadChildren: () => import('./profles/profiles.module').then((m) => m.ProfilesModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.user.superadmin] },
      },
      {
        path: 'perfil',
        loadChildren: () => import('./profile/profile.module').then((m) => m.ProfileModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
      },
      {
        path: 'organizaciones',
        loadChildren: () => import('./partners/partners.module').then((m) => m.PartnersModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.user.superadmin] },
      },
      {
        path: 'fichas',
        loadChildren: () => import('./personal-card/personal-card.module').then((m) => m.PersonalCardModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.personalcard.access] },
      },
      {
        path: 'seguimientos',
        loadChildren: () => import('./personal-indicators/personal-indicators.module').then((m) => m.PersonalIndicatorsModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.personalindicators.access] },
      },
      {
        path: 'busqueda',
        loadChildren: () => import('./search/search.module').then((m) => m.SearchModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
      },
      {
        path: 'configuracion',
        loadChildren: () => import('./configuration/configuration.module').then((m) => m.ConfigurationModule),
        canLoad: [AuthenticatedGuard],
        canActivate: [AuthenticatedGuard],
        data: { allowedPermissions: [Permissions.user.superadmin] },
      },
    ],
  },
];

@NgModule({
  declarations: [DashboardComponent, FooterComponent],
  imports: [CommonModule, HeaderModule, RouterModule.forChild(routes), AlertModule],
  exports: [DashboardComponent],
})
export class DashboardModule {}
