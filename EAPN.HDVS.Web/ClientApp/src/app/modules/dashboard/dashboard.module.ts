import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { HeaderModule } from 'src/app/core/header/header.module';
import { FooterComponent } from 'src/app/core/footer/footer/footer.component';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Roles } from 'src/app/core/enums/roles.enum';

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
        path: 'usuarios',
        loadChildren: () => import('./user-management/user-management.module').then((m) => m.UserManagementModule),
        canLoad: [AuthenticatedGuard],
        data: { allowedRoles: [Roles.usermanagement.access] },
      },
    ],
  },
];

@NgModule({
  declarations: [DashboardComponent, FooterComponent],
  imports: [CommonModule, HeaderModule, RouterModule.forChild(routes)],
  exports: [DashboardComponent],
})
export class DashboardModule {}
