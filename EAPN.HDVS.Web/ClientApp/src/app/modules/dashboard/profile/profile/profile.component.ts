import { Component, OnInit } from '@angular/core';
import { DatosUsuarioDto, AdjuntoDto } from 'src/app/core/api/api.client';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  public usuario: DatosUsuarioDto;
  public newpass: string;
  public passconfirm: string;
  public partnerId: Observable<number>;

  constructor(
    private authService: AuthenticationService,
    private alertService: AlertService,
    private translate: TranslateService,
    private router: Router
  ) {
    this.partnerId = this.authService.getUserPartnerId();
  }

  ngOnInit() {
    this.authService.getDatosUsuario().subscribe((x) => (this.usuario = x));
  }

  public async onProfilePictureChanged(foto: AdjuntoDto): Promise<void> {
    if (!foto) {
      return;
    }

    this.usuario.fotoId = foto.id;
    this.saveData('');
  }

  public async onSaveUser(): Promise<void> {
    this.saveData('/');
  }

  private async saveData(redirectTo: string): Promise<void> {
    try {
      this.usuario.nuevaClave = this.newpass;
      const result = await this.authService.updateDatosUsuario(this.usuario).toPromise();

      if (!result) {
        this.alertService.error(this.translate.instant('formulario-perfil.no-guardado'));
        return;
      }

      if (redirectTo) {
        await this.router.navigateByUrl(redirectTo);
      }

      this.alertService.success(this.translate.instant('core.datos-guardados'));
    } catch (error) {}
  }
}
