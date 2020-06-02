import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { AdjuntoDto } from 'src/app/core/api/api.client';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { FileUpload, UploadType } from 'src/app/core/http/file-upload';
import { AlertService } from 'src/app/core/services/alert.service';
import { AttachmentService } from 'src/app/core/services/attachments.service';
import { UploadService } from 'src/app/core/services/upload.service';
import { Card } from 'src/app/shared/models/card';

@Component({
  selector: 'app-card-attachments',
  templateUrl: './card-attachments.component.html',
  styleUrls: ['./card-attachments.component.scss'],
})
export class CardAttachmentsComponent implements OnInit {
  @Input() card: Card;
  public permissions = Permissions;
  public selected: AdjuntoDto[] = [];
  public uploadFiles: FileUpload[] = [];
  public forceRefresh: boolean = false;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('adjuntos.fichero'),
      field: 'nombreOriginal',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('core.tamano'),
      field: 'tamano',
      maxWidth: 150,
      valueFormatter: this.sizeFormatter,
    },
  ];

  constructor(
    private uploadService: UploadService,
    private translate: TranslateService,
    private modalService: NgbModal,
    private attachmentService: AttachmentService,
    private alertService: AlertService
  ) {}

  async ngOnInit() {}

  public async onDowloadFiles(): Promise<void> {
    if (!this.selected.length) {
      return;
    }

    for (const adjunto of this.selected) {
      try {
        this.attachmentService.downloadAdjunto(adjunto.id);
      } catch (error) {
        this.alertService.error(error);
      }
    }
  }

  public onFileAdded(files: File[]): void {
    if (!files || files.length === 0) {
      return;
    }

    this.uploadFiles = files.map((x) => {
      const upload = new FileUpload(x, {
        TipoId: UploadType.Personales_Ficha,
        FichaId: this.card.id,
        OrganizacionId: this.card.organizacionId,
      });

      upload.registerCallback((adjunto) => this.card.adjuntos.push(adjunto));
      this.uploadService.addToQueue(upload);
      return upload;
    });
  }

  public onUploadFinished(): void {
    this.uploadFiles = [];
    this.modalService.dismissAll();
    this.forceRefresh = true;
    setTimeout(() => (this.forceRefresh = false), 10);
  }

  public async onAddNewClick(modal: any): Promise<void> {
    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static', size: 'xl' }).result;
    } catch (error) {
      return;
    }
  }

  public async onDeleteClick(modal: any): Promise<void> {
    if (!this.selected || !this.selected.length) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.attachmentService.deleteAdjuntos(this.selected);
      this.card.adjuntos = this.card.adjuntos.filter((x) => !this.selected.some((sel) => sel.id === x.id));
      this.selected = [];

      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  private sizeFormatter(params): string {
    const bytes = (params || {}).value;
    if (bytes === 0) {
      return '0 Bytes';
    }

    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}
