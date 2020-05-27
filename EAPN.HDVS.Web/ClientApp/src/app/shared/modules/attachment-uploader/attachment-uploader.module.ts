import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploaderComponent } from './file-uploader/file-uploader.component';
import { CoreModule } from 'src/app/core/core.module';
import { NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';
import { FileUploaderEntryComponent } from './file-uploader-entry/file-uploader-entry.component';

@NgModule({
  declarations: [FileUploaderComponent, FileUploaderEntryComponent],
  imports: [CommonModule, CoreModule, NgbProgressbarModule],
  exports: [FileUploaderComponent]
})
export class AttachmentUploaderModule { }
