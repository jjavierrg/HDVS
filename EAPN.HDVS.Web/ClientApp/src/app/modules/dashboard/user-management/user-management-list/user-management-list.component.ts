import { Component, OnInit } from '@angular/core';
import { UsuarioDto, ApiClient } from 'src/app/core/api/api.client';
import { AgGridColumn } from 'ag-grid-angular';
import { Roles } from '../../../../core/enums/roles.enum';
import { CheckboxCellComponent } from 'src/app/shared/modules/grid/checkbox-cell/checkbox-cell.component';
import { DescriptionArrayCellComponent } from 'src/app/shared/modules/grid/array-cell/description-array-cell.component';
import { Router, ActivatedRoute } from '@angular/router';
import { UserManagementService } from 'src/app/core/services/user-management.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/core/services/alert.service';

@Component({
  selector: 'app-user-management-list',
  templateUrl: './user-management-list.component.html',
  styleUrls: ['./user-management-list.component.scss'],
})
export class UserManagementListComponent implements OnInit {
  public usuarios: UsuarioDto[];
  public roles = Roles;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: 'Email',
      field: 'email',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: 'Apellidos',
      field: 'apellidos',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: 'Nombre',
      field: 'nombre',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: 'Perfiles',
      field: 'perfiles',
      cellRendererFramework: DescriptionArrayCellComponent,
      minWidth: 100,
      sortable: false,
    },
    {
      headerName: 'Permisos',
      field: 'rolesAdicionales',
      cellRendererFramework: DescriptionArrayCellComponent,
      minWidth: 100,
      sortable: false,
    },
    {
      headerName: 'Activo',
      field: 'activo',
      cellRendererFramework: CheckboxCellComponent,
      flex: null,
    },
  ];

  constructor(
    private service: UserManagementService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private alertService: AlertService
  ) {}

  ngOnInit() {
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
      this.alertService.success('Se han eliminado los usuario seleccionados');
    } catch (error) {
      this.alertService.error(error);
    }
  }

  private RefreshData(): void {
    this.service.getUsuarios().subscribe((usuarios) => (this.usuarios = usuarios));
  }
}
