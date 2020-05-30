import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SecureImageComponent } from './secure-image/secure-image.component';
import { SecureProfileImageComponent } from './secure-profile-image/secure-profile-image.component';
import { NgbPopoverModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { AttachmentUploaderModule } from '../attachment-uploader/attachment-uploader.module';
import { CoreModule } from 'src/app/core/core.module';

@NgModule({
  declarations: [SecureImageComponent, SecureProfileImageComponent],
  imports: [CoreModule, CommonModule, NgbPopoverModule, NgbModalModule, AttachmentUploaderModule],
  exports: [SecureImageComponent, SecureProfileImageComponent],
})
export class ImageModule {}
