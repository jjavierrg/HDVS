import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { HeaderModule } from 'src/app/core/header/header.module';
import { FooterComponent } from 'src/app/core/footer/footer/footer.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
];

@NgModule({
  declarations: [HomeComponent, FooterComponent],
  imports: [CommonModule, HeaderModule, RouterModule.forChild(routes)],
})
export class HomeModule {}
