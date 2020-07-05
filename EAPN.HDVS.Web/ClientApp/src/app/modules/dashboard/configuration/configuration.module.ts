import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from 'src/app/core/core.module';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { LinksComponent } from './links/links.component';
import { ConfigurationComponent } from './general-configuration/configuration.component';
import { RangeComponent } from './range/range.component';

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
  declarations: [ConfigurationComponent, LinksComponent, RangeComponent],
  imports: [CommonModule, CoreModule, NgbNavModule, InputModule, RouterModule.forChild(routes), FormsModule],
})
export class ConfigurationModule {}
