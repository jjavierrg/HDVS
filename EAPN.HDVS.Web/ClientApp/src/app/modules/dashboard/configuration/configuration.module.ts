import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from 'src/app/core/core.module';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { EnlacesComponent } from './enlaces/enlaces.component';
import { ConfigurationComponent } from './general-configuration/configuration.component';
import { FormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/modules/input/input.module';

const routes: Routes = [
  {
    path: '',
    component: ConfigurationComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
];

@NgModule({
  declarations: [ConfigurationComponent, EnlacesComponent],
  imports: [CommonModule, CoreModule, NgbNavModule, InputModule, RouterModule.forChild(routes), FormsModule],
})
export class ConfigurationModule {}
