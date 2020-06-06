import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
  selector: 'app-description-array-cell',
  template: '{{ value }}',
  styles: [''],
})
export class DescriptionArrayCellComponent implements ICellRendererAngularComp {
  public value: string;

  refresh(params: any): boolean {
    return false;
  }

  agInit(params: ICellRendererParams): void {
    this.setValue(params.value);
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}

  private setValue(data: { descripcion: string }[]) {
    if (!data || data.length <= 0) {
      this.value = '';
    } else {
      this.value = data.map(x => x.descripcion).join(', ');
    }
  }
}
