import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardFinderComponent } from './card-finder/card-finder.component';
import { CoreModule } from 'src/app/core/core.module';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { FormsModule } from '@angular/forms';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { CardFormComponent } from './card-form/card-form.component';
import { Permissions } from 'src/app/core/enums/permissions.enum';

const routes: Routes = [
  {
    path: 'alta',
    component: CardFinderComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.write] },
  },
  {
    path: '',
    component: CardFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.write] },
  },
  {
    path: ':id',
    component: CardFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.write] },
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'alta',
  },
];

@NgModule({
  declarations: [CardFinderComponent, CardFormComponent],
  imports: [CommonModule, CoreModule, InputModule, FormsModule, NgbModalModule, GridModule, RouterModule.forChild(routes)],
})
export class PersonalCardModule {}
