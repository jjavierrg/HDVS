import { Component, OnInit } from '@angular/core';
import { UsuarioDto, ApiClient } from 'src/app/core/api/api.client';
import { AgGridColumn } from 'ag-grid-angular';
import { Roles } from '../../../../core/enums/roles.enum';
import { CheckboxCellComponent } from 'src/app/shared/modules/grid/checkbox-cell/checkbox-cell.component';
import { DescriptionArrayCellComponent } from 'src/app/shared/modules/grid/array-cell/description-array-cell.component';
import { Router, ActivatedRoute } from '@angular/router';
import { UserManagementService } from 'src/app/core/services/user-management.service';

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
      resizable: true,
      flex: 1,
      minWidth: 150,
    },
    {
      headerName: 'Perfiles',
      resizable: true,
      flex: 1,
      field: 'perfiles',
      cellRendererFramework: DescriptionArrayCellComponent,
      minWidth: 150,
    },
    {
      headerName: 'Permisos',
      resizable: true,
      flex: 1,
      field: 'rolesAdicionales',
      cellRendererFramework: DescriptionArrayCellComponent,
      minWidth: 150,
    },
    {
      headerName: 'Activo',
      field: 'activo',
      cellRendererFramework: CheckboxCellComponent,
    },
    {
      headerName: 'Bloqueado',
      field: 'finBloqueo',
    },
  ];

  constructor(private service: UserManagementService, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.service.getUsuarios().subscribe((usuarios) => (this.usuarios = usuarios));
  }

  public onNewUser(): void {
    this.router.navigate(['nuevo'], { relativeTo: this.activatedRoute });
  }

  public onEditUser(usuario: UsuarioDto): void {
    if (!!usuario) {
      this.router.navigate([usuario.id], { relativeTo: this.activatedRoute });
    }
  }
}
