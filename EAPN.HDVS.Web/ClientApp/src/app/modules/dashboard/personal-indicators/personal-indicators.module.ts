import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { PersonalIndicatorsFormComponent } from './personal-indicators-form/personal-indicators-form.component';
import { IndicatorModule } from 'src/app/shared/modules/indicator/indicator.module';
import { CoreModule } from 'src/app/core/core.module';
import { CardInfoComponent } from './card-info/card-info.component';

const routes: Routes = [
  {
    path: '',
    component: PersonalIndicatorsFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalindicators.write] },
  },
  {
    path: ':id',
    component: PersonalIndicatorsFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalindicators.write] },
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '',
  },
];

@NgModule({
  declarations: [PersonalIndicatorsFormComponent, CardInfoComponent],
  imports: [CommonModule, RouterModule.forChild(routes), CoreModule, IndicatorModule],
  exports: [PersonalIndicatorsFormComponent],
})
export class PersonalIndicatorsModule {}
