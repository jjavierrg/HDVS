import { Component, OnInit } from '@angular/core';
import { UsuarioDto, ApiClient } from 'src/app/core/api/api.client';
import { AgGridColumn } from 'ag-grid-angular';
import { Roles } from '../../../../core/enums/roles.enum';
import { CheckboxCellComponent } from 'src/app/shared/modules/grid/checkbox-cell/checkbox-cell.component';
import { DescriptionArrayCellComponent } from 'src/app/shared/modules/grid/array-cell/description-array-cell.component';
import { Router, ActivatedRoute } from '@angular/router';

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

  constructor(private apiclient: ApiClient, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.apiclient.getUsarios().subscribe((usuarios) => (this.usuarios = usuarios));
  }

  public onNewUser(): void {
    this.router.navigate(['nuevo'], { relativeTo: this.activatedRoute });
  }
}
