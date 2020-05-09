import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
  selector: 'app-array-cell',
  template: '{{ value }}',
  styles: [''],
})
export class ArrayCellComponent implements ICellRendererAngularComp {
  public value: string;

  refresh(params: any): boolean {
    return false;
  }

  agInit(params: ICellRendererParams): void {
    this.setValue(params.value);
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}

  private setValue(data: string[]){
    if (!data || data.length <= 0) {
      this.value = '';
    } else {
      this.value = data.join(', ');
    }
  }
}
