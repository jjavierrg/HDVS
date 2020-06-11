import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Card } from 'src/app/shared/models/card';
import { AgGridColumn } from 'ag-grid-angular';
import { TranslateService } from '@ngx-translate/core';
import { SeguimientoViewDto } from 'src/app/core/api/api.client';
import { DateCellComponent } from 'src/app/shared/modules/grid/date-cell/date-cell.component';
import { IndicatorRangeCellComponent } from 'src/app/shared/modules/grid/indicator-range-cell/indicator-range-cell.component';
import { RangeService } from 'src/app/core/services/range.service';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { AlertService } from 'src/app/core/services/alert.service';

@Component({
  selector: 'app-card-review',
  templateUrl: './card-review.component.html',
  styleUrls: ['./card-review.component.scss'],
})
export class CardReviewComponent implements OnInit, OnChanges {
  @Input() public card: Card;
  @Input() public canSaveCard: boolean;

  @Output() public reviewRequired = new EventEmitter<number>();
  @Output() public reviewEditRequired = new EventEmitter<number>();

  public selection: SeguimientoViewDto[] = [];
  public permissions = Permissions;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('ficha.tecnico'),
      field: 'nombreTecnico',
      minWidth: 100,
      filter: 'agTextColumnFilter',
      flex: 2,
    },
    {
      headerName: this.translate.instant('core.fecha'),
      field: 'fecha',
      maxWidth: 150,
      cellRendererFramework: DateCellComponent,
    },
    {
      headerName: this.translate.instant('seguimientos.puntuacion'),
      field: 'puntuacion',
      maxWidth: 150,
      filter: 'agNumberColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.grado'),
      field: 'puntuacion',
      minWidth: 100,
      cellRendererFramework: IndicatorRangeCellComponent,
      flex: 1,
    },
  ];

  constructor(
    private translate: TranslateService,
    private rangeService: RangeService,
    private modalService: NgbModal,
    private indicatorService: IndicatorService,
    private alertService: AlertService
  ) {}

  async ngOnInit() {
    await this.rangeService.forceRefresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes.canSaveCard.firstChange) {
      this.canSaveCard = changes.canSaveCard.currentValue;
    }
  }

  public async onDeleteClick(modal: any): Promise<void> {
    if (!this.selection || !this.selection.length) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.indicatorService.deleteReviews(this.selection);
      this.card.seguimientos = this.card.seguimientos.filter((x) => !this.selection.some((sel) => sel.id === x.id));
      this.selection = [];

      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public onAddReviewClick(): void {
    this.reviewEditRequired.emit(0);
  }

  public onViewReviewClick(): void {
    if (this.selection.length === 1) {
      this.reviewRequired.emit(this.selection[0].id);
    }
  }

  public onEditReviewClick(): void {
    if (this.selection.length === 1) {
      this.reviewEditRequired.emit(this.selection[0].id);
    }
  }
}
