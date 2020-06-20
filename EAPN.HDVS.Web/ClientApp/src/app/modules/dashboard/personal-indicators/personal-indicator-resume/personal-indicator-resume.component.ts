import { Component, OnInit } from '@angular/core';
import { SeguimientoDto, MasterDataDto, DimensionDto, CategoriaDto } from 'src/app/core/api/api.client';
import { IReviewState } from 'src/app/shared/models/reviewState';
import { Location } from '@angular/common';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { TranslateService } from '@ngx-translate/core';
import { RangeService } from 'src/app/core/services/range.service';
import { Permissions } from 'src/app/core/enums/permissions.enum';

@Component({
  selector: 'app-personal-indicator-resume',
  templateUrl: './personal-indicator-resume.component.html',
  styleUrls: ['./personal-indicator-resume.component.scss'],
})
export class PersonalIndicatorResumeComponent implements OnInit {
  public review: SeguimientoDto;
  public dimensions: DimensionDto[];
  public permissions = Permissions;
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

    await this.rangeService.forceRefresh();
    this.users = await this.masterdataService.getUsuarios().toPromise();

    try {
      if (reviewId) {
        this.review = await this.service.getReview(reviewId).toPromise();
      }
    } catch (error) {
      await this.router.navigate(['/']).then(() => this.alertService.error(error));
      return;
    }

    if (!this.review) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.registro-no-encontrado')));
      return;
    }

    if (this.review.organizacionId !== partnerId) {
      await this.router.navigate(['/']).then(() => this.alertService.error(this.translate.instant('core.no-autorizado')));
      return;
    }

    await this.retrieveData(this.review);
    this.returnUrl = state.returnUrl ? state.returnUrl : '/';
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

  public onEditCardRequired(): void {
    if (!this.review.fichaId) {
      return;
    }

    this.returnUrl = `/fichas/${this.review.fichaId}`;
    this.router.navigateByUrl(this.returnUrl);
  }

  public getDimensionScore(dimension: DimensionDto): number {
    if (!dimension || !dimension.categorias || !dimension.categorias.length) {
      return 0;
    }

    if (!this.review || !this.review.indicadores || !this.review.indicadores.length) {
      return 0;
    }

    const categories = dimension.categorias.map((x) => x.id);
    const indicators = this.review.indicadores.filter((x) => x.indicador && categories.some((cat) => cat === x.indicador.categoriaId));
    return indicators.reduce((result, ind) => (result += ind.indicador.puntuacion), 0);
  }

  public checkedIndicator(categoryId: number): string {
    const indicator = this.review.indicadores.find((x) => x.indicador && x.indicador.categoriaId === categoryId);
    return indicator.verificado ? indicator.indicador.verificacion : '';
  }

  public getCategoryDescription(categoryId: number): string {
    const indicator = this.review.indicadores.find((x) => x.indicador && x.indicador.categoriaId === categoryId).indicador;
    return indicator ? indicator.descripcion : '';
  }

  public getCategoryScore(categoryId: number): number {
    const indicator = this.review.indicadores.find((x) => x.indicador && x.indicador.categoriaId === categoryId).indicador;
    return indicator ? indicator.puntuacion : 0;
  }

  public getCategoryObservations(categoryId: number): string {
    const indicator = this.review.indicadores.find((x) => x.indicador && x.indicador.categoriaId === categoryId);
    return indicator ? indicator.observaciones : '';
  }

  private async retrieveData(review: SeguimientoDto): Promise<void> {
    const onlyUnique = (value: number, index: number, array: number[]): boolean => array.indexOf(value) === index;
    const catIds: number[] = this.review.indicadores.map((x) => (x.indicador ? x.indicador.categoriaId : 0)).filter(onlyUnique);
    this.dimensions = await this.service.getDimensionesByCategorias(catIds).toPromise();
  }
}
