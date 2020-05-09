import { Component, OnInit } from '@angular/core';
import { UserManagementService } from 'src/app/core/services/user-management.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UsuarioDto, PerfilDto, RolDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-user-management-form',
  templateUrl: './user-management-form.component.html',
  styleUrls: ['./user-management-form.component.scss'],
})
export class UserManagementFormComponent implements OnInit {
  public title: string;
  public editing: boolean = false;
  public usuario: UsuarioDto;
  public perfiles: PerfilDto[];
  public roles: RolDto[];

  constructor(
    private service: UserManagementService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    const userId = snapshot.params['id'];

    const [roles, perfiles] = await Promise.all([this.service.getRoles().toPromise(), this.service.getPerfiles().toPromise()]);
    this.perfiles = perfiles;
    this.roles = roles;

    if (!!userId) {
      this.usuario = await this.service.getUsuario(+userId).toPromise();
      this.editing = true;
      this.title = this.usuario ? this.usuario.email : '';

      if (!this.usuario) {
        this.router.navigate(['../'], { relativeTo: this.route }).then(() => this.alertService.error('Usuario no encontrado'));
      }
    } else {
      this.usuario = new UsuarioDto({ activo: true });
      this.editing = false;
      this.title = 'Nuevo Usuario';
    }
  }

  public async onSaveUser(): Promise<void> {
    const success = await this.saveUser(this.usuario);
    if (success) {
      this.router.navigate(['../'], { relativeTo: this.route }).then(() => this.alertService.success('Datos guardados correctamente'));
    }
  }

  private async saveUser(usuario: UsuarioDto): Promise<boolean> {
    if (!usuario) {
      this.alertService.warning('No se han podido leer los datos introducidos');
      return false;
    }

    try {
      if (this.editing) {
        await this.service.updateUsuario(usuario).toPromise();
      } else {
        await this.service.createUsuario(usuario).toPromise();
      }

      return true;
    } catch (error) {
      this.alertService.error(error);
      return false;
    }
  }
}
