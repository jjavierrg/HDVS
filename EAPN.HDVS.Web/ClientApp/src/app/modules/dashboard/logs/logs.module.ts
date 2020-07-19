import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoreModule } from 'src/app/core/core.module';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { LogComponent } from './log/log.component';

const routes: Routes = [
  {
    path: '',
    component: LogComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.user.superadmin] },
  },
];

@NgModule({
  declarations: [LogComponent],
  imports: [CommonModule, CoreModule, GridModule, RouterModule.forChild(routes)],
})
export class LogsModule {}
