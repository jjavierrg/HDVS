import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Card } from 'src/app/shared/models/card';
import { AgGridColumn } from 'ag-grid-angular';
import { TranslateService } from '@ngx-translate/core';
import { SeguimientoViewDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-card-review',
  templateUrl: './card-review.component.html',
  styleUrls: ['./card-review.component.scss'],
})
export class CardReviewComponent implements OnInit, OnChanges {
  @Input() public card: Card;
  @Input() public canSaveCard: boolean;

  @Output() public reviewRequired = new EventEmitter<number>();

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
      headerName: this.translate.instant('core.fecha'),
      field: 'fecha',
      maxWidth: 150,
    },
    {
      headerName: this.translate.instant('seguimientos.puntuacion'),
      field: 'puntuacion',
      maxWidth: 150,
      filter: 'agNumberColumnFilter',
    },
  ];

  constructor(private translate: TranslateService) {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes.canSaveCard.firstChange) {
      this.canSaveCard = changes.canSaveCard.currentValue;
    }
  }

  public onAddReviewClick(): void {
    this.reviewRequired.emit(0);
  }

  public onEditReviewClick(): void {
    if (this.selection.length === 1) {
      this.reviewRequired.emit(this.selection[0].id);
    }
  }
}
