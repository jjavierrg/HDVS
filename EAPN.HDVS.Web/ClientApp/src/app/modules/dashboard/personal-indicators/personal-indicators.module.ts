import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { PersonalIndicatorsFormComponent } from './personal-indicators-form/personal-indicators-form.component';
import { IndicatorModule } from 'src/app/shared/modules/indicator/indicator.module';
import { CoreModule } from 'src/app/core/core.module';
import { CardInfoComponent } from './card-info/card-info.component';
import { NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { ImageModule } from 'src/app/shared/modules/image/image.module';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { FormsModule } from '@angular/forms';
import { PersonalIndicatorResumeComponent } from './personal-indicator-resume/personal-indicator-resume.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { UiSwitchModule } from 'ngx-ui-switch';

const routes: Routes = [
  {
    path: '',
    component: PersonalIndicatorsFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalindicators.write] },
  },
  {
    path: 'resumen/:id',
    component: PersonalIndicatorResumeComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalindicators.read] },
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
  declarations: [PersonalIndicatorsFormComponent, CardInfoComponent, PersonalIndicatorResumeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    CoreModule,
    IndicatorModule,
    NgbAccordionModule,
    ImageModule,
    InputModule,
    FormsModule,
    HighchartsChartModule,
    UiSwitchModule,
  ],
  exports: [PersonalIndicatorsFormComponent],
})
export class PersonalIndicatorsModule {}
