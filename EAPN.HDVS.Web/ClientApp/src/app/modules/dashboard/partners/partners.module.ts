import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { CoreModule } from 'src/app/core/core.module';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { FormsModule } from '@angular/forms';
import { PartnerListComponent } from './partner-list/partner-list.component';
import { PartnerFormComponent } from './partner-form/partner-form.component';
import { ImageModule } from 'src/app/shared/modules/image/image.module';

const routes: Routes = [
  {
    path: '',
    component: PartnerListComponent,
  },
  {
    path: 'nuevo',
    component: PartnerFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
  {
    path: ':id',
    component: PartnerFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
];

@NgModule({
  declarations: [PartnerListComponent, PartnerFormComponent],
  imports: [CommonModule, CoreModule, GridModule, NgbModalModule, InputModule, FormsModule, ImageModule, RouterModule.forChild(routes)],
})
export class PartnersModule { }
