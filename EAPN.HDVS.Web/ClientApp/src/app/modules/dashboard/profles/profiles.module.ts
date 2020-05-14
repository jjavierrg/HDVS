import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileListComponent } from './profile-list/profile-list.component';
import { ProfileFormComponent } from './profile-form/profile-form.component';
import { CoreModule } from 'src/app/core/core.module';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/modules/input/input.module';

const routes: Routes = [
  {
    path: '',
    component: ProfileListComponent,
  },
  {
    path: 'nuevo',
    component: ProfileFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
  {
    path: ':id',
    component: ProfileFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
];

@NgModule({
  declarations: [ProfileListComponent, ProfileFormComponent],
  imports: [CommonModule, CoreModule, GridModule, NgbModalModule, InputModule, FormsModule, RouterModule.forChild(routes)],
})
export class ProfilesModule {}
