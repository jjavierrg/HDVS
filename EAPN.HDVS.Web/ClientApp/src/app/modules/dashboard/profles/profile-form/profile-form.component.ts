import { Component, OnInit } from '@angular/core';
import { PerfilDto, PermisoDto, MasterDataDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ProfileService } from 'src/app/core/services/profile.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';

@Component({
  selector: 'app-profile-form',
  templateUrl: './profile-form.component.html',
  styleUrls: ['./profile-form.component.scss'],
})
export class ProfileFormComponent implements OnInit {
  public title: string;
  public editing: boolean = false;
  public profile: PerfilDto;
  public permisos: MasterDataDto[];

  constructor(
    private service: ProfileService,
    private masterdataService: MasterdataService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    const profileId = snapshot.params['id'];

    this.permisos = await this.masterdataService.getPermisos().toPromise();

    if (!!profileId) {
      this.profile = await this.service.getPerfil(+profileId).toPromise();
      this.editing = true;
      this.title = this.profile ? this.profile.descripcion : '';

      if (!this.profile) {
        this.router
          .navigate(['../'], { relativeTo: this.route })
          .then(() => this.alertService.error(this.translate.instant('core.registro-no-encontrado')));
      }
    } else {
      this.profile = new PerfilDto();
      this.editing = false;
      this.title = this.translate.instant('formulario-perfil.nuevo-perfil');
    }
  }

  public async onSaveProfile(): Promise<void> {
    const success = await this.saveProfile(this.profile);
    if (success) {
      this.router
        .navigate(['../'], { relativeTo: this.route })
        .then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate(['../'], { relativeTo: this.route });
  }

  private async saveProfile(profile: PerfilDto): Promise<boolean> {
    if (!profile) {
      this.alertService.warning(this.translate.instant('core.datos-corruptos'));
      return false;
    }

    try {
      if (this.editing) {
        await this.service.updatePerfil(profile).toPromise();
      } else {
        await this.service.createPerfil(profile).toPromise();
      }

      return true;
    } catch (error) {
      this.alertService.error(error);
      return false;
    }
  }
}
