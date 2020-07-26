import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { AdjuntoDto, IFichaDto, SeguimientoDto, SeguimientoViewDto } from 'src/app/core/api/api.client';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AlertService } from 'src/app/core/services/alert.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { CardService } from 'src/app/core/services/card.service';
import { Card } from 'src/app/shared/models/card';
import { IReviewState } from 'src/app/shared/models/reviewState';

@Component({
  selector: 'app-card-form',
  templateUrl: './card-form.component.html',
  styleUrls: ['./card-form.component.scss'],
})
export class CardFormComponent implements OnInit {
  public card: Card;
  public cardValid: boolean;
  public permissions = Permissions;
  public activeTab: string = '';

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
    private translate: TranslateService,
    private modalService: NgbModal
  ) {}

  async ngOnInit() {
    const snapshot = this.route.snapshot;
    const cardId = snapshot.params['id'];
    const { navigationId: number, ...others } = window.history.state;
    const hash = snapshot.fragment;

    let card: IFichaDto;
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

    this.activeTab = hash || 'datos';
  }

  public async onSaveCard(): Promise<void> {
    const isnew: boolean = !this.card.id;
    const createNewSeg: boolean =
      isnew &&
      (await this.authService
        .isAuthorized([this.permissions.personalindicators.access, this.permissions.personalindicators.write], true)
        .toPromise());

    try {
      if (!createNewSeg) {
        const saved = await this.service.saveCard(this.card).toPromise();
        this.card.id = saved.id;

        if (saved) {
          this.alertService.success(this.translate.instant('core.datos-guardados'));
        }
      } else {
        await this.onReviewRequired(0, false);
      }
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public async onReviewRequired(reviewId: number, readonly: boolean): Promise<boolean> {
    try {
      const saved = await this.service.saveCard(this.card).toPromise();

      if (!saved) {
        return false;
      }

      this.card.id = saved.id;
      const state: IReviewState = { returnUrl: `fichas/${this.card.id}#seguimientos` };
      const reviewUrl: string = `/seguimientos${readonly ? '/resumen' : ''}`;
      if (reviewId) {
        return this.router.navigate([reviewUrl, reviewId], { state });
      }

      const review: SeguimientoDto = new SeguimientoDto({
        fichaId: this.card.id,
        organizacionId: this.card.organizacionId,
        id: reviewId,
        usuarioId: this.card.usuarioId,
      });

      state.review = review;
      return this.router.navigate(['/seguimientos'], { state });
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public async onCancel(): Promise<void> {
    await this.router.navigate(['/']);
  }

  public async onDeleteClick(modal: any): Promise<void> {
    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.service.deleteCard(this.card.id).toPromise();
      await this.router.navigate(['/']);
      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public async onProfilePictureChanged(foto: AdjuntoDto): Promise<void> {
    if (!foto) {
      return;
    }

    this.card.fotoId = foto.id;
    if (this.card.id) {
      await this.service.saveCard(this.card).toPromise();
    }
  }

  private async ensureRequiredCardFields(card: IFichaDto) {
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
    card.adjuntos = card.adjuntos || new Array<AdjuntoDto>();
    card.seguimientos = card.seguimientos || new Array<SeguimientoViewDto>();
  }
}
