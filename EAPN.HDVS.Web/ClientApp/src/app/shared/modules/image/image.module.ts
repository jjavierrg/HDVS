import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SecureImageComponent } from './secure-image/secure-image.component';



@NgModule({
  declarations: [SecureImageComponent],
  imports: [
    CommonModule
  ],
  exports: [SecureImageComponent]
})
export class ImageModule { }
