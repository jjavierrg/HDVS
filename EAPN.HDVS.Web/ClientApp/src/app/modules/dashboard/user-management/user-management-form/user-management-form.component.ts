import { Component, OnInit } from '@angular/core';
import { UserManagementService } from 'src/app/core/services/user-management.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UsuarioDto, PerfilDto, PermisoDto, OrganizacionDto, MasterDataDto, AdjuntoDto } from 'src/app/core/api/api.client';
import { TranslateService } from '@ngx-translate/core';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';

@Component({
  selector: 'app-user-management-form',
  templateUrl: './user-management-form.component.html',
  styleUrls: ['./user-management-form.component.scss'],
})
export class UserManagementFormComponent implements OnInit {
  public title: string;
  public editing: boolean = false;
  public usuario: UsuarioDto;
  public perfiles: MasterDataDto[];
  public organizaciones: MasterDataDto[];
  public permisos: MasterDataDto[];
  public permissions = Permissions;

  constructor(
    private service: UserManagementService,
    private masterdataService: MasterdataService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService,
    private authService: AuthenticationService
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    const userId = snapshot.params['id'];
    let partnerId = snapshot.params['partnerId'];

    const [permisos, perfiles, organizaciones] = await Promise.all([
      this.masterdataService.getPermisos().toPromise(),
      this.masterdataService.getPerfiles().toPromise(),
      this.masterdataService.getOrganizaciones().toPromise(),
    ]);
    this.perfiles = perfiles;
    this.permisos = permisos;
    this.organizaciones = organizaciones;

    // only superadmin manage other partners users
    if (!partnerId || !this.authService.isAuthorized([Permissions.user.superadmin])) {
      partnerId = await this.authService.getUserPartnerId().toPromise();
    }

    if (!!userId) {
      this.usuario = await this.service.getUsuario(+userId).toPromise();
      this.editing = true;
      this.title = this.usuario ? this.usuario.email : '';

      if (!this.usuario) {
        this.router
          .navigate(['../'], { relativeTo: this.route })
          .then(() => this.alertService.error(this.translate.instant('core.registro-no-encontrado')));
      }
    } else {
      this.usuario = new UsuarioDto({ activo: true, organizacionId: +partnerId });
      this.editing = false;
      this.title = this.translate.instant('formulario-usuarios.nuevo-usuario');
    }
  }

  public async onSaveUser(): Promise<void> {
    const success = await this.saveUser(this.usuario);
    if (success) {
      this.router
        .navigate(['../'], { relativeTo: this.route })
        .then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate(['../'], { relativeTo: this.route });
  }

  public async onProfilePictureChanged(foto: AdjuntoDto): Promise<void> {
    if (!foto) {
      return;
    }

    this.usuario.fotoId = foto.id;

    if (this.usuario.id) {
      this.saveUser(this.usuario);
    }
  }

  private async saveUser(usuario: UsuarioDto): Promise<boolean> {
    if (!usuario) {
      this.alertService.warning(this.translate.instant('core.datos-corruptos'));
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
