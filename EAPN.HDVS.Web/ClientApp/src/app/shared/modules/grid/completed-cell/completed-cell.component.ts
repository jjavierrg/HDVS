import { Component, OnInit } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
  selector: 'app-completed-cell',
  templateUrl: './completed-cell.component.html',
  styleUrls: ['./completed-cell.component.scss'],
})
export class CompletedCellComponent implements ICellRendererAngularComp {
  public params: ICellRendererParams;

  refresh(params: any): boolean {
    return false;
  }

  agInit(params: ICellRendererParams): void {
    this.params = params;
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}
}
