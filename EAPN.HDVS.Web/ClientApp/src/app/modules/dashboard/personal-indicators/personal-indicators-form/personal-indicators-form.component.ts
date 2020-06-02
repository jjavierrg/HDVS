import { Component, OnInit } from '@angular/core';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { SeguimientoDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-personal-indicators-form',
  templateUrl: './personal-indicators-form.component.html',
  styleUrls: ['./personal-indicators-form.component.scss'],
})
export class PersonalIndicatorsFormComponent implements OnInit {
  public review: SeguimientoDto;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: IndicatorService,
    private authService: AuthenticationService,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  async ngOnInit() {
    const partnerId: number = await this.authService.getUserPartnerId().toPromise();
    const snapshot = this.route.snapshot;
    const reviewId = snapshot.params['id'];
    const { navigationId: number, ...others } = window.history.state;

    let review: SeguimientoDto;
    if (Object.keys(others).length) {
      review = others;
    }

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

    if (review.organizacionId !== partnerId) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.no-autorizado')));
      return;
    }

    this.review = new SeguimientoDto(review);
  }

  public async onSaveReview(): Promise<void> {
    try {
      const success = await this.service.saveReview(this.review).toPromise();
      if (success) {
        this.router.navigate(['/']).then(() => this.alertService.success(this.translate.instant('core.datos-guardados')));
      }
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public async onSave(redirect: boolean): Promise<void> {
    try {
      await this.service.saveReview(this.review).toPromise();
    } catch (error) {
      this.alertService.error(error);
    }
  }
}
