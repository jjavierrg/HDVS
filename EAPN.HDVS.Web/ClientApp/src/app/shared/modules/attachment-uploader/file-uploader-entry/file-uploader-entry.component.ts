import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUpload } from 'src/app/core/http/file-upload';

@Component({
  selector: 'app-file-uploader-entry',
  templateUrl: './file-uploader-entry.component.html',
  styleUrls: ['./file-uploader-entry.component.scss'],
})
export class FileUploaderEntryComponent {
  @Input() file: FileUpload;

  constructor() {}

  public formatBytes(bytes: number, decimals?: number) {
    if (bytes === 0) {
      return '0 Bytes';
    }

    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals || 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }
}
