import { AdjuntoDto } from '../api/api.client';

export enum FileUploadStatus {
  Pending,
  Queued,
  Success,
  Error,
  Progress,
}

export enum UploadType {
  Imagenes = 1,
  Adjuntos = 2,
  Documentacion = 3,
  Personales_Ficha = 4,
}

export interface IUploadMetadata {
  TipoId: UploadType;
  FichaId?: number;
  OrganizacionId?: number;
}

export class FileUpload {
  public progress: number;
  public status: FileUploadStatus;

  private onComplete: (adjunto: AdjuntoDto) => void;

  constructor(public file: File, public metadata: IUploadMetadata) {
    this.progress = 0;
    this.status = FileUploadStatus.Pending;
    this.onComplete = () => {};
  }

  public updateProgress(progress: number) {
    this.progress = progress;
    this.status = FileUploadStatus.Progress;
  }

  public registerCallback(fn: (adjunto: AdjuntoDto) => void): void {
    this.onComplete = fn;
  }

  public completed(adjunto: AdjuntoDto) {
    this.progress = 100;
    this.status = FileUploadStatus.Success;
    this.onComplete(adjunto);
  }

  public failed() {
    this.progress = 0;
    this.status = FileUploadStatus.Error;
  }

  // statuses
  public isPending = () => this.status === FileUploadStatus.Queued;
  public isSuccess = () => this.status === FileUploadStatus.Success;
  public isError = () => this.status === FileUploadStatus.Error;
  public inProgress = () => this.status === FileUploadStatus.Progress;
}
