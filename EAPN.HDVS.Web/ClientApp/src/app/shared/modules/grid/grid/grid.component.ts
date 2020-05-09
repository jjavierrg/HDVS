import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { AgGridColumn } from 'ag-grid-angular';
import { RowNode, SelectionChangedEvent } from 'ag-grid-community';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class GridComponent<T> implements OnInit {
  @Input() public columns: Partial<AgGridColumn>[];
  @Input() public data: T[];
  @Input() public multiselect: boolean = true;
  @Input() public showToolbar: boolean = true;
  @Input() public showAddButton: boolean = true;
  @Input() public showEditButton: boolean = true;
  @Input() public showDeleteButton: boolean = true;
  @Input() public rolesAddButton: string[];
  @Input() public rolesEditButton: string[];
  @Input() public rolesDeleteButton: string[];

  @Output() public selectionChange = new EventEmitter<T[]>();
  @Output() public addItem = new EventEmitter<void>();
  @Output() public editItem = new EventEmitter<T>();
  @Output() public deleteItems = new EventEmitter<T[]>();

  public selectedRows: RowNode[];

  ngOnInit(): void {
    if (this.multiselect && !!this.columns) {
      this.columns = [
        {
          headerCheckboxSelection: true,
          headerCheckboxSelectionFilteredOnly: true,
          checkboxSelection: true,
          width: 50,
          pinned: true,
        },
        ...this.columns,
      ];
    }
  }

  public onSelectionChanged(event: SelectionChangedEvent): void {
    this.selectedRows = event.api.getSelectedNodes();
    const items = this.selectedRows.map((x) => <T>x.data);
    this.selectionChange.emit(items);
  }

  public onAddItem(): void {
    this.addItem.emit();
  }

  public onEditItem(): void {
    if (!this.selectedRows || this.selectedRows.length !== 1) {
      return;
    }

    const items = this.selectedRows.map((x) => <T>x.data);
    this.editItem.emit(items[0]);
  }

  public onDeleteItem(): void {
    if (!this.selectedRows || this.selectedRows.length === 0) {
      return;
    }

    const items = this.selectedRows.map((x) => <T>x.data);
    this.deleteItems.emit(items);
  }
}
