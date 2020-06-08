import { Component, OnInit } from '@angular/core';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { SeguimientoDto, MasterDataDto } from 'src/app/core/api/api.client';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { IReviewState } from 'src/app/shared/models/reviewState';
import { Location } from '@angular/common';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { RangeService } from 'src/app/core/services/range.service';

@Component({
  selector: 'app-personal-indicators-form',
  templateUrl: './personal-indicators-form.component.html',
  styleUrls: ['./personal-indicators-form.component.scss'],
})
export class PersonalIndicatorsFormComponent implements OnInit {
  public review: SeguimientoDto;
  public permissions = Permissions;
  public readonly: boolean;
  public users: MasterDataDto[] = [];

  private returnUrl: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: IndicatorService,
    private authService: AuthenticationService,
    private alertService: AlertService,
    private translate: TranslateService,
    private location: Location,
    private masterdataService: MasterdataService,
    private rangeService: RangeService
  ) {}

  async ngOnInit() {
    const partnerId: number = await this.authService.getUserPartnerId().toPromise();
    const snapshot = this.route.snapshot;
    const reviewId = snapshot.params['id'];
    const state: IReviewState = this.location.getState();

    if (
      (state.readonly && !(await this.authService.isAuthorized([this.permissions.personalindicators.read]).toPromise())) ||
      (!state.readonly && !(await this.authService.isAuthorized([this.permissions.personalindicators.write]).toPromise()))
    ) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.no-autorizado')));
      return;
    }

    await this.rangeService.forceRefresh();
    this.masterdataService.getUsuariosByPartnerId(partnerId).subscribe((x) => (this.users = x));

    let review: SeguimientoDto = state.review;
    try {
      if (!review && reviewId) {
        review = await this.service.getReview(reviewId).toPromise();
      }
    } catch (error) {
      await this.router.navigate(['/']).then(() => this.alertService.error(error));
      return;
    }

    if (!review) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.registro-no-encontrado')));
      return;
    }

    if (!review.fecha) {
      review.fecha = new Date();
    }

    if (review.organizacionId !== partnerId) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.no-autorizado')));
      return;
    }

    this.review = new SeguimientoDto(review);
    this.returnUrl = state.returnUrl ? state.returnUrl : '/';
    this.readonly = state.readonly;
  }

  public getPuntuacion(): number {
    if (!this.review || !this.review.indicadores || !this.review.indicadores.length) {
      return 0;
    }

    return this.review.indicadores.reduce((prev, item) => prev + item.indicador.puntuacion, 0);
  }

  public getRange(): string {
    const score: number = this.getPuntuacion();
    return this.rangeService.getRangeDescriptionByScore(score);
  }

  public onEditCardRequired(): Promise<void> {
    if (!this.review.fichaId) {
      return;
    }

    this.returnUrl = `/fichas/${this.review.fichaId}`;
    return this.onSave(true);
  }

  public async onSave(redirect: boolean): Promise<void> {
    try {
      if (!this.readonly) {
        await this.service.saveReview(this.review).toPromise();
      }

      if (redirect) {
        await this.router.navigateByUrl(this.returnUrl);
        if (!this.readonly) {
          this.alertService.success(this.translate.instant('core.datos-guardados'));
        }
      }
    } catch (error) {
      this.alertService.error(error);
    }
  }
}
