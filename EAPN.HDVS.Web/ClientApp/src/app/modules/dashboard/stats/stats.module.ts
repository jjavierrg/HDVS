import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from 'src/app/core/core.module';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { StatsComponent } from './stats/stats.component';

const routes: Routes = [
  {
    path: '',
    component: StatsComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.stats.access] },
  },
];

@NgModule({
  declarations: [StatsComponent],
  imports: [CommonModule, CoreModule, InputModule, NgbAccordionModule, RouterModule.forChild(routes), FormsModule],
})
export class StatsModule { }
