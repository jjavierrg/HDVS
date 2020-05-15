import { Component, OnInit } from '@angular/core';
import { DatosUsuarioDto } from 'src/app/core/api/api.client';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  public usuario: DatosUsuarioDto;
  public newpass: string;
  public passconfirm: string;

  constructor(
    private authService: AuthenticationService,
    private alertService: AlertService,
    private translate: TranslateService,
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.getDatosUsuario().subscribe((x) => (this.usuario = x));
  }

  public async onSaveUser(): Promise<void> {
    try {
      this.usuario.nuevaClave = this.newpass;
      const result = await this.authService.updateDatosUsuario(this.usuario).toPromise();
      if (result) {
        await this.router.navigate(['/']);
        this.alertService.success(this.translate.instant('core.datos-guardados'));
      } else {
        this.alertService.error(this.translate.instant('formulario-perfil.no-guardado'));
      }
    } catch (error) {}
  }
}
