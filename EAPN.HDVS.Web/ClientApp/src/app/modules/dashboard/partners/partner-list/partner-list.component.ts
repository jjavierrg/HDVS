import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { AsociacionDto } from 'src/app/core/api/api.client';
import { AgGridColumn } from 'ag-grid-angular';
import { PartnerService } from 'src/app/core/services/partner.service';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { CheckboxCellComponent } from 'src/app/shared/modules/grid/checkbox-cell/checkbox-cell.component';


@Component({
  selector: 'app-partner-list',
  templateUrl: './partner-list.component.html',
  styleUrls: ['./partner-list.component.scss']
})
export class PartnerListComponent implements OnInit  {
  public partners: AsociacionDto[];
  public permissions = Permissions;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('comun.descripcion'),
      field: 'nombre',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.activo'),
      field: 'activa',
      cellRendererFramework: CheckboxCellComponent,
      maxWidth: 100,
    },
    {
      headerName: this.translate.instant('comun.numero-usuarios'),
      field: 'numeroUsuarios',
      maxWidth: 150,
      filter: 'agNumberColumnFilter',
    }
  ];

  constructor(
    private service: PartnerService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.RefreshData();
  }

  public onNewPartner(): void {
    this.router.navigate(['nuevo'], { relativeTo: this.activatedRoute });
  }

  public onEditPartner(asociacion: AsociacionDto): void {
    if (!!asociacion) {
      this.router.navigate([asociacion.id], { relativeTo: this.activatedRoute });
    }
  }

  public async onDeletePartners(asociaciones: AsociacionDto[], modal: any): Promise<void> {
    if (!asociaciones || !asociaciones.length) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.service.deleteAsociaciones(asociaciones);
      this.RefreshData();
      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  private RefreshData(): void {
    this.service.getAsociaciones().subscribe((partners) => (this.partners = partners));
  }
}
