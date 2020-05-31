import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { FichaDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { CardService } from 'src/app/core/services/card.service';
import { Card } from 'src/app/shared/models/card';

@Component({
  selector: 'app-card-form',
  templateUrl: './card-form.component.html',
  styleUrls: ['./card-form.component.scss'],
})
export class CardFormComponent implements OnInit {
  public card: Card;
  public cardValid: boolean;

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

    await this.ensureRequiredCardFields(card);
    this.card = new Card(card);
  }

  public async onSaveCard(): Promise<void> {
    const success = await this.service.saveCard(this.card).toPromise();
    if (success) {
      this.router.navigate(['/']).then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate(['/']);
  }

  public onProfilePictureChanged(foto: FichaDto): void {
    if (foto) {
      this.card.fotoId = foto.id;
    }
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
