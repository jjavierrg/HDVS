import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardFinderComponent } from './card-finder/card-finder.component';
import { CoreModule } from 'src/app/core/core.module';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: 'alta',
    component: CardFinderComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'alta',
  },
];

@NgModule({
  declarations: [CardFinderComponent],
  imports: [CommonModule, CoreModule, InputModule, FormsModule, RouterModule.forChild(routes)],
})
export class PersonalCardModule {}
