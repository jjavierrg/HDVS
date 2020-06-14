import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUpload, UploadType } from 'src/app/core/http/file-upload';
import { UploadService } from 'src/app/core/services/upload.service';
import { AdjuntoDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-secure-profile-image',
  templateUrl: './secure-profile-image.component.html',
  styleUrls: ['./secure-profile-image.component.scss'],
})
export class SecureProfileImageComponent implements OnInit {
  @Input() photoId: number;
  @Input() fichaId: number;
  @Input() organizacionId: number;
  @Input() disableEdit: boolean = false;
  @Input() imageClasses: string = 'thumbnail rounded-circle';
  @Input() fallbackImage: string = 'assets/user.svg';
  @Output() imageChange = new EventEmitter<AdjuntoDto>();

  constructor(private modalService: NgbModal, private uploadService: UploadService) {}

  ngOnInit() {}

  public async onFullImageClick(modal: any): Promise<void> {
    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static', size: 'xl' }).result;
    } catch (error) {
      return;
    }
  }

  public onUpload(file: FileUpload) {
    this.uploadService.addToQueue(file);
    file.registerCallback((adjunto) => {
      this.photoId = adjunto.id;
      this.modalService.dismissAll();
      this.imageChange.emit(adjunto);
    });
  }

  public onFileAdded(files: File[]) {
    if (!files || files.length !== 1) {
      return;
    }

    const uploads = files.map(
      (x) => new FileUpload(x, { TipoId: UploadType.Imagenes, FichaId: this.fichaId, OrganizacionId: this.organizacionId })
    );
    this.onUpload(uploads[0]);
  }
}
