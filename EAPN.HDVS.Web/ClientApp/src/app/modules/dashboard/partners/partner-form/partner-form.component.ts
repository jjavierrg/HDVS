import { Component, OnInit } from '@angular/core';
import { AlertService } from 'src/app/core/services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { OrganizacionDto, AdjuntoDto } from 'src/app/core/api/api.client';
import { PartnerService } from 'src/app/core/services/partner.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-partner-form',
  templateUrl: './partner-form.component.html',
  styleUrls: ['./partner-form.component.scss'],
})
export class PartnerFormComponent implements OnInit {
  public title: string;
  public editing: boolean = false;
  public partner: OrganizacionDto;
  public permisos: OrganizacionDto[];
  public restricted: boolean;

  constructor(
    private service: PartnerService,
    private authService: AuthenticationService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    let partnerId = snapshot.params['id'];
    this.restricted = snapshot.data['restricted'];

    if (this.restricted) {
      partnerId = await this.authService.getUserPartnerId().toPromise();
    }

    if (!!partnerId) {
      this.partner = await this.service.getOrganizacion(+partnerId).toPromise();
      this.editing = true;
      this.title = this.partner ? this.partner.nombre : '';

      if (!this.partner) {
        await this.router.navigate([this.restricted ? '/' : '../'], { relativeTo: this.route });
        this.alertService.error(this.translate.instant('core.registro-no-encontrado'));
      }
    } else {
      this.partner = new OrganizacionDto({ activa: true });
      this.editing = false;
      this.title = this.translate.instant('formulario-organizacion.nueva-organizacion');
    }
  }

  public async onSavepartner(): Promise<void> {
    const success = await this.savepartner(this.partner);
    if (success) {
      this.router
        .navigate([this.restricted ? '/' : '../'], { relativeTo: this.route })
        .then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate([this.restricted ? '/' : '../'], { relativeTo: this.route });
  }

  public async onProfilePictureChanged(foto: AdjuntoDto): Promise<void> {
    if (!foto) {
      return;
    }

    this.partner.fotoId = foto.id;

    if (this.partner.id) {
      this.savepartner(this.partner);
    }
  }

  private async savepartner(partner: OrganizacionDto): Promise<boolean> {
    if (!partner) {
      this.alertService.warning(this.translate.instant('core.datos-corruptos'));
      return false;
    }

    try {
      if (this.editing) {
        await this.service.updateOrganizacion(partner).toPromise();
      } else {
        await this.service.createOrganizacion(partner).toPromise();
      }

      return true;
    } catch (error) {
      this.alertService.error(error);
      return false;
    }
  }
}
