import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { RowNode, SelectionChangedEvent } from 'ag-grid-community';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class GridComponent<T> implements OnInit {
  @Input() public columns: Partial<AgGridColumn>[];
  @Input() public defaultColDef: Partial<AgGridColumn>;
  @Input() public title: string;
  @Input() public data: T[];
  @Input() public pagination: boolean = false;
  @Input() public multiselect: boolean = true;
  @Input() public showToolbar: boolean = true;
  @Input() public showAddButton: boolean = true;
  @Input() public showEditButton: boolean = true;
  @Input() public addButtonText: string;
  @Input() public editButtonText: string;
  @Input() public deleteButtonText: string;
  @Input() public showDeleteButton: boolean = true;
  @Input() public permissionsAddButton: string[];
  @Input() public permissionsEditButton: string[];
  @Input() public permissionsDeleteButton: string[];

  @Output() public selectionChange = new EventEmitter<T[]>();
  @Output() public addItem = new EventEmitter<void>();
  @Output() public editItem = new EventEmitter<T>();
  @Output() public deleteItems = new EventEmitter<T[]>();

  public selectedRows: RowNode[];

  constructor(private translate: TranslateService) {
    this.defaultColDef = {
      resizable: true,
      editable: false,
      flex: 1,
      sortable: true,
    };
  }

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

  public getLocaleTextFunc(): (key: string, defaultValue: string) => string {
    const translateService = this.translate;
    return function (key: string, defaultValue: string): string {
      const translated = translateService.instant(`grid.${key}`);
      return !translated || translated === `grid.${key}` ? defaultValue : translated;
    };
  }
}
