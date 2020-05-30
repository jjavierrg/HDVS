import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUpload, FileUploadStatus } from 'src/app/core/http/file-upload';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss'],
})
export class FileUploaderComponent {
  @Input() files: FileUpload[] = [];
  @Input() showUploadButton: boolean;
  @Input() showClearButton: boolean;
  @Input() multiple: boolean;

  @Output() filesAdded = new EventEmitter<File[]>();
  @Output() fileCancel = new EventEmitter<FileUpload>();
  @Output() upload = new EventEmitter<FileUpload[]>();

  constructor() {}

  public uploadFile(fileList: FileList): void {
    const files: File[] = [];

    for (let i = 0; i < fileList.length; i++) {
      if (fileList[i].size > 0) {
        files.push(fileList[i]);
        if (!this.multiple) {
          break;
        }
      }
    }

    this.filesAdded.emit(files);
  }

  public onCancelAttachment(file: FileUpload): void {
    this.fileCancel.emit(file);
  }

  public onUploadFilesClick(): void {
    const files = this.files.filter((x) => x.status === FileUploadStatus.Pending);
    this.upload.emit(files);
  }

  public onClearFiles(): void {
    this.files = this.files.filter(x => x.status !== FileUploadStatus.Error && x.status !== FileUploadStatus.Success);
  }
}
