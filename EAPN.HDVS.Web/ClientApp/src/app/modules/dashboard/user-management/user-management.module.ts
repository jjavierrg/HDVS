import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreModule } from 'src/app/core/core.module';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { UserManagementListComponent } from './user-management-list/user-management-list.component';
import { UserManagementFormComponent } from './user-management-form/user-management-form.component';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { FormsModule } from '@angular/forms';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

const routes: Routes = [
  {
    path: '',
    component: UserManagementListComponent,
  },
  {
    path: 'asociacion/:partnerId',
    component: UserManagementListComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
  {
    path: 'asociacion/:partnerId/nuevo',
    component: UserManagementFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
  {
    path: 'asociacion/:partnerId/:id',
    component: UserManagementFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
  {
    path: 'nuevo',
    component: UserManagementFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.usermanagement.write] },
  },
  {
    path: ':id',
    component: UserManagementFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.usermanagement.write] },
  },
];

@NgModule({
  declarations: [UserManagementListComponent, UserManagementFormComponent],
  imports: [CoreModule, CommonModule, GridModule, RouterModule.forChild(routes), InputModule, FormsModule, NgbModalModule],
})
export class UserManagementModule {}
