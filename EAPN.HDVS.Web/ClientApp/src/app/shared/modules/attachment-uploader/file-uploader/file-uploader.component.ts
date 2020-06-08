import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUpload, FileUploadStatus } from 'src/app/core/http/file-upload';
import { UploadService } from 'src/app/core/services/upload.service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss'],
})
export class FileUploaderComponent {
  @Input() files: FileUpload[] = [];
  @Input() multiple: boolean;

  @Output() filesAdded = new EventEmitter<File[]>();
  @Output() uploadFinished = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  constructor(private uploadService: UploadService) {
    this.uploadService.queue.subscribe((x) => {
      if (!this.files.length) {
        return;
      }

      if (this.files.every((file) => file.isSuccess() || file.isError())) {
        this.uploadFinished.emit();
      }
    });
  }

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

  public onCancelClick(): void {
    this.cancel.emit();
  }
}
