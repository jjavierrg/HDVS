import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
  selector: 'app-dat-cell',
  template: '{{ value | defaultDate}}',
  styles: [''],
})
export class DateCellComponent implements ICellRendererAngularComp {
  public value: Date;

  refresh(params: any): boolean {
    return false;
  }

  agInit(params: ICellRendererParams): void {
    this.value = params.value;
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}
}
