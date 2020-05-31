import { HttpClient, HttpErrorResponse, HttpEventType, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AdjuntoDto } from '../api/api.client';
import { FileUpload, FileUploadStatus } from '../http/file-upload';

@Injectable({
  providedIn: 'root',
})
export class UploadService {
  private _isUploading: boolean;
  private _currentlyUploading: BehaviorSubject<boolean>;
  private _files: FileUpload[] = [];
  private _uploadQueue: BehaviorSubject<FileUpload[]>;

  constructor(private httpClient: HttpClient) {
    this._isUploading = false;
    this._currentlyUploading = new BehaviorSubject(this._isUploading);
    this._uploadQueue = new BehaviorSubject([]);
  }

  public get queue(): Observable<FileUpload[]> {
    return this._uploadQueue.asObservable();
  }

  public addToQueue(file: FileUpload): void {
    if (file.status !== FileUploadStatus.Pending) {
      return;
    }

    file.status = FileUploadStatus.Queued;
    this._files.push(file);
    this._uploadQueue.next(this._files);

    this.checkAndUploadNextFile();
  }

  private checkAndUploadNextFile() {
    if (this._isUploading) {
      return;
    }

    const pending = this._files.find((f) => f.isPending());
    if (pending) {
      this.upload(pending);
    }
  }

  private upload(queuedUploadFile: FileUpload) {
    this.isUploading = true;
    queuedUploadFile.updateProgress(0);

    const request = this.createRequest(queuedUploadFile);
    this.httpClient
      .request<AdjuntoDto>(request)
      .pipe(
        finalize(() => {
          this.isUploading = false;
          this.checkAndUploadNextFile();
        })
      )
      .subscribe(
        (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            const percentDone = Math.round((100 * event.loaded) / event.total);
            queuedUploadFile.updateProgress(percentDone);
          } else if (event instanceof HttpResponse) {
            queuedUploadFile.completed(event.body);
          }

          this._uploadQueue.next(this._files);
        },
        (err: HttpErrorResponse) => {
          if (err.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            queuedUploadFile.failed();
          } else {
            // The backend returned an unsuccessful response code.
            queuedUploadFile.failed();
          }

          this._uploadQueue.next(this._files);
        }
      );
  }

  private set isUploading(value: boolean) {
    this._isUploading = value;
    this._currentlyUploading.next(value);
  }

  private createRequest(queuedUploadFile: FileUpload) {
    const formData = new FormData();
    const dto = {
      tipoId: queuedUploadFile.metadata.TipoId,
      fichaId: queuedUploadFile.metadata.FichaId,
      organizacionId: queuedUploadFile.metadata.OrganizacionId,
      file: queuedUploadFile.file,
    };

    for (const prop in dto) {
      if (!dto.hasOwnProperty(prop) || !dto[prop]) {
        continue;
      }
      formData.append(prop, dto[prop]);
    }

    const request = new HttpRequest('POST', this.getUrl(), formData, {
      reportProgress: true,
    });

    return request;
  }

  private getUrl() {
    return `${environment.apiEndpoint}/api/adjuntos`;
  }
}
