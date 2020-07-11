import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { StatsComponent } from './stats/stats.component';
import { CoreModule } from 'src/app/core/core.module';
import { FormsModule } from '@angular/forms';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { InputModule } from 'src/app/shared/modules/input/input.module';

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
  imports: [CommonModule, CoreModule, InputModule, NgbCollapseModule, RouterModule.forChild(routes), FormsModule],
})
export class StatsModule { }
