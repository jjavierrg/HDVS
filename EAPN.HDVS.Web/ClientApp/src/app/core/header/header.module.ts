import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './header/header.component';
import { CoreModule } from '../core.module';

@NgModule({
  declarations: [NavbarComponent, HeaderComponent],
  imports: [CommonModule, RouterModule, NgbCollapseModule, NgbDropdownModule, CoreModule],
  exports: [NavbarComponent, HeaderComponent],
})
export class HeaderModule {
}
