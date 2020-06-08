import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Card } from 'src/app/shared/models/card';
import { AgGridColumn } from 'ag-grid-angular';
import { TranslateService } from '@ngx-translate/core';
import { SeguimientoViewDto } from 'src/app/core/api/api.client';
import { DateCellComponent } from 'src/app/shared/modules/grid/date-cell/date-cell.component';
import { IndicatorRangeCellComponent } from 'src/app/shared/modules/grid/indicator-range-cell/indicator-range-cell.component';
import { RangeService } from 'src/app/core/services/range.service';

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
  public forceRefresh: boolean = false;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('ficha.tecnico'),
      field: 'nombreTecnico',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.grado'),
      field: 'puntuacion',
      minWidth: 100,
      cellRendererFramework: IndicatorRangeCellComponent,
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
  ];

  constructor(private translate: TranslateService, private rangeService: RangeService) {}

  async ngOnInit() {
    await this.rangeService.forceRefresh();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes.canSaveCard.firstChange) {
      this.canSaveCard = changes.canSaveCard.currentValue;
    }
  }

  public onAddReviewClick(): void {
    this.reviewRequired.emit(0);
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
