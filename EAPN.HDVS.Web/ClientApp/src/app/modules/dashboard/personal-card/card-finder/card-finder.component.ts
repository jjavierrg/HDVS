import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { FichaDto, IVistaPreviaFichaDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { CardService } from 'src/app/core/services/card.service';
import { ISearchQuery, SearchQuery } from 'src/app/shared/models/search-query';

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

  public matching: IVistaPreviaFichaDto[];

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
      headerName: this.translate.instant('ficha.tecnico'),
      field: 'nombreTecnico',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
  ];

  constructor(
    private service: CardService,
    private alertService: AlertService,
    private modalService: NgbModal,
    private translate: TranslateService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  async ngOnInit() {}

  public async onSubmitQuery(): Promise<boolean> {
    this.matching = [];
    try {
      const query: ISearchQuery = new SearchQuery({ name: this.name, surname1: this.surname1, surname2: this.surname2, birth: this.birth });
      this.matching = await this.service.findPreviewCards(query).toPromise();
    } catch (error) {
      this.alertService.error(error);
      return;
    }

    // Fix for modal open and form validation
    const element = document.activeElement as HTMLElement;
    if (!!element) {
      element.blur();
    }

    let result: boolean;
    if (this.matching && this.matching.length) {
      result = await this.modalService.open(this.matchModal, { centered: true, backdrop: 'static', size: 'xl' }).result.catch(() => {});
    } else {
      result = await this.modalService.open(this.noMatchModal, { centered: true, backdrop: 'static' }).result.catch(() => {});
    }

    if (result) {
      await this.onContinue();
    }
  }

  public async onContinue(): Promise<void> {
    const ficha: FichaDto = new FichaDto({
      nombre: this.name,
      apellido1: this.surname1,
      apellido2: this.surname2,
      fechaNacimiento: this.birth,
    });

    await this.router.navigate(['../'], { relativeTo: this.route, state: ficha });
  }
}
