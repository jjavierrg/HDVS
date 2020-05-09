import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertContainerComponent } from './alert-container/alert-container.component';
import { AlertComponent } from './alert/alert.component';
import { NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [AlertComponent, AlertContainerComponent],
  imports: [
    CommonModule, NgbProgressbarModule
  ],
  exports: [AlertContainerComponent]
})
export class AlertModule { }
