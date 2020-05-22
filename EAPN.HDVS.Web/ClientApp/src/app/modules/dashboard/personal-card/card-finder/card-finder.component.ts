import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CardService } from 'src/app/core/services/card.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VistaPreviaFichaDto } from 'src/app/core/api/api.client';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';

@Component({
  selector: 'app-card-finder',
  templateUrl: './card-finder.component.html',
  styleUrls: ['./card-finder.component.scss'],
})
export class CardFinderComponent implements OnInit {
  public name: string;
  public surname1: string;
  public surname2: string;
  public birth: Date;

  public matching: VistaPreviaFichaDto[];

  @ViewChild('noMatchModal', { static: false }) noMatchModal: ElementRef;
  @ViewChild('matchModal', { static: false }) matchModal: ElementRef;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('comun.nombre'),
      field: 'nombre',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.apellido1'),
      field: 'apellido1',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.apellido2'),
      field: 'apellido2',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.codigo'),
      field: 'codigo',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.organizacion'),
      field: 'nombreOrganizacion',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.telefono'),
      field: 'telefonoOrganizacion',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.email'),
      field: 'emailOrganizacion',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('fichas.tecnico'),
      field: 'nombreTecnico',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
  ];

  constructor(
    private service: CardService,
    private alertService: AlertService,
    private modalService: NgbModal,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
  }

  public async onSubmitQuery(): Promise<boolean> {
    this.matching = [];
    try {
      this.matching = await this.service.findPreviewCards(this.name, this.surname1, this.surname2, this.birth).toPromise();
    } catch (error) {
      this.alertService.error(error);
      return;
    }

    // Fix for modal open and form validation
    const element = document.activeElement as HTMLElement;
    if (!!element) {
      element.blur();
    }

    if (this.matching && this.matching.length) {
      await this.modalService.open(this.matchModal, { centered: true, backdrop: 'static', size: 'xl' }).result.catch(() => {});
    } else {
      await this.modalService.open(this.noMatchModal, { centered: true, backdrop: 'static' }).result.catch(() => {});
    }
  }
}
