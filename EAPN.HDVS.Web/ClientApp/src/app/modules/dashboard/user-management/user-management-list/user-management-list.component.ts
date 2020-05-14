import { Component, OnInit } from '@angular/core';
import { UsuarioDto, ApiClient } from 'src/app/core/api/api.client';
import { AgGridColumn } from 'ag-grid-angular';
import { Permissions } from '../../../../core/enums/permissions.enum';
import { CheckboxCellComponent } from 'src/app/shared/modules/grid/checkbox-cell/checkbox-cell.component';
import { DescriptionArrayCellComponent } from 'src/app/shared/modules/grid/array-cell/description-array-cell.component';
import { Router, ActivatedRoute } from '@angular/router';
import { UserManagementService } from 'src/app/core/services/user-management.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-user-management-list',
  templateUrl: './user-management-list.component.html',
  styleUrls: ['./user-management-list.component.scss'],
})
export class UserManagementListComponent implements OnInit {
  public users: UsuarioDto[];
  public permissions = Permissions;

  private partnerId?: number;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('comun.email'),
      field: 'email',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.apellidos'),
      field: 'apellidos',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.nombre'),
      field: 'nombre',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.perfiles'),
      field: 'perfiles',
      cellRendererFramework: DescriptionArrayCellComponent,
      minWidth: 100,
      sortable: false,
    },
    {
      headerName: this.translate.instant('comun.activo'),
      field: 'activo',
      cellRendererFramework: CheckboxCellComponent,
      maxWidth: 100,
    },
    {
      headerName: this.translate.instant('comun.asociacion'),
      field: 'asociacion.nombre',
      filter: 'agTextColumnFilter',
    },
  ];

  constructor(
    private service: UserManagementService,
    private router: Router,
    private route: ActivatedRoute,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    this.partnerId = snapshot.params['partnerId'];

    this.RefreshData();
  }

  public onNewUser(): void {
    this.router.navigate(['nuevo'], { relativeTo: this.activatedRoute });
  }

  public onEditUser(usuario: UsuarioDto): void {
    if (!!usuario) {
      this.router.navigate([usuario.id], { relativeTo: this.activatedRoute });
    }
  }

  public async onDeleteUsers(usuarios: UsuarioDto[], modal: any): Promise<void> {
    if (!usuarios || !usuarios.length) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.service.deleteUsuarios(usuarios);
      this.RefreshData();
      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  private RefreshData(): void {
    if (this.partnerId) {
      this.service.getUsuariosByPartnerId(this.partnerId).subscribe((users) => (this.users = users));
    } else {
      this.service.getUsuarios().subscribe((users) => (this.users = users));
    }
  }
}
