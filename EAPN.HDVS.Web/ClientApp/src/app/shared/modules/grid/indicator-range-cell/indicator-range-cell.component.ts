import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';
import { RangeService } from 'src/app/core/services/range.service';

@Component({
  selector: 'app-indactor-range-cell',
  template: '{{ value }}',
  styles: [''],
})
export class IndicatorRangeCellComponent implements ICellRendererAngularComp {
  public value: string;

  constructor(private rangeService: RangeService) { }

  refresh(params: any): boolean {
    return false;
  }

  agInit(params: ICellRendererParams): void {
    this.value = this.rangeService.getRangeDescriptionByScore(+params.value);
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}
}
