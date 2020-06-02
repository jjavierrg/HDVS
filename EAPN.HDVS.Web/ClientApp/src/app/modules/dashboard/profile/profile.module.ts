import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { ProfileComponent } from './profile/profile.component';
import { CoreModule } from 'src/app/core/core.module';
import { FormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { ImageModule } from 'src/app/shared/modules/image/image.module';

const routes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
  },
];

@NgModule({
  declarations: [ProfileComponent],
  imports: [CommonModule, CoreModule, FormsModule, InputModule, RouterModule.forChild(routes), ImageModule],
})
export class ProfileModule {}
