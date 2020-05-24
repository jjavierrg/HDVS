import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { FichaDto, MasterDataDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { CardService } from 'src/app/core/services/card.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-card-form',
  templateUrl: './card-form.component.html',
  styleUrls: ['./card-form.component.scss'],
})
export class CardFormComponent implements OnInit {
  public card: FichaDto;
  public genders: MasterDataDto[];
  public countries: MasterDataDto[];
  public provincias: MasterDataDto[];
  public municipios: MasterDataDto[] = [];

  public get fullName(): string {
    if (!this.card) {
      return '';
    }

    const name: string = this.card.nombre || '';
    const surname1: string = this.card.apellido1 || '';
    const surname2: string = this.card.apellido2 || '';

    return `${name} ${surname1} ${surname2}`.trim();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: CardService,
    private authService: AuthenticationService,
    private masterdataService: MasterdataService,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    const cardId = snapshot.params['id'];
    const { navigationId: number, ...others } = window.history.state;

    let card: FichaDto;
    if (Object.keys(others).length) {
      card = others;
    }

    try {
      if (!card && cardId) {
        card = await this.service.getCard(cardId).toPromise();
      }
    } catch (error) {
      await this.router.navigate(['/']).then(() => this.alertService.error(error));
      return;
    }

    if (!card) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.registro-no-encontrado')));
      return;
    }

    // Load Data
    const [genders, countries, provincias] = await Promise.all([
      this.masterdataService.getGenders().toPromise(),
      this.masterdataService.getCountries().toPromise(),
      this.masterdataService.getProvincias().toPromise(),
    ]);

    this.genders = genders;
    this.countries = countries;
    this.provincias = provincias;

    if (card.provinciaId) {
      this.municipios = await this.masterdataService.getMunicipiosByProvincia(card.provinciaId).toPromise();
    }

    await this.ensureRequiredCardFields(card);
    this.card = card;
  }

  public async onSaveCard(): Promise<void> {
    const success = await this.service.saveCard(this.card).toPromise();
    if (success) {
      this.router
        .navigate(['/'])
        .then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate(['/']);
  }

  public async onProvinciaChange(provinciaId: number) {
    this.municipios = await this.masterdataService.getMunicipiosByProvincia(provinciaId).toPromise();
  }

  private async ensureRequiredCardFields(card: FichaDto) {
    const [partnerId, userId] = await Promise.all([
      this.authService.getUserPartnerId().toPromise(),
      this.authService.getUserId().toPromise(),
    ]);

    card.nombre = card.nombre || '';
    card.codigo = card.codigo || '';
    card.fotocopiaDNI = card.fotocopiaDNI || false;
    card.politicaFirmada = card.politicaFirmada || false;
    card.apellido1 = card.apellido1 || '';
    card.apellido2 = card.apellido2 || '';
    card.organizacionId = card.organizacionId || partnerId;
    card.usuarioId = card.usuarioId || userId;
  }
}
