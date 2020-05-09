import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

interface SelectableItem<T> {
  selected: boolean;
  item: T;
}

@Component({
  selector: 'app-combobox-multiple',
  templateUrl: './combobox-multiple.component.html',
  styles: [''],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ComboboxMultipleComponent),
      multi: true,
    },
  ],
})
export class ComboboxMultipleComponent<T> implements ControlValueAccessor {
  @Input() public placeholder: string;
  @Input() public valueField: string;
  @Input() public displayField: string;

  @Input()
  public get data(): T[] {
    return this.selectableItems.map((x) => x.item);
  }
  public set data(value: T[]) {
    const items = !value ? [] : value;
    this.selectableItems = items.map((x) => <SelectableItem<T>>{ selected: false, item: x });
  }

  @Input() public disabled: boolean = false;
  @Input() public separator: string = ', ';

  @Output() selectionChange = new EventEmitter<T[]>();
  @Output() valueChange = new EventEmitter<any[]>();

  public selectableItems: SelectableItem<T>[] = [];
  public onChange = (fn: any) => {};
  public onTouched = () => {};

  public getControlText(): string {
    const selected = this.selectableItems.filter((x) => x.selected).map((x) => x.item);
    if (!selected || !selected.length) {
      return this.placeholder;
    }

    if (!this.displayField) {
      return `${selected.join(this.separator)}`;
    } else {
      return `${selected.map((x) => x[this.displayField]).join(this.separator)}`;
    }
  }

  public getItemText(item: SelectableItem<T>): string {
    if (!item || !item.item) {
      return '';
    } else if (!this.displayField) {
      return `${item.item}`;
    } else {
      return `${item.item[this.displayField]}`;
    }
  }

  public onValueChanged(item: SelectableItem<T>) {
    if (this.disabled || !item) {
      return;
    }

    item.selected = !item.selected;

    const selected = this.selectableItems.filter((x) => x.selected).map((x) => x.item);
    let value: any[];
    if (!this.valueField) {
      value = selected;
    } else {
      value = selected.map((x) => x[this.valueField]);
    }

    this.onChange(value);
    this.selectionChange.emit(selected);
    this.valueChange.emit(value);
  }

  writeValue(value: any[]): void {
    if (!value) {
      value = [];
    }

    if (!this.data) {
      this.data = [];
    }

    if (!this.valueField) {
      this.selectableItems.forEach((x) => (x.selected = value.some((y) => JSON.stringify(x.item) === JSON.stringify(y))));
    } else if (this.data && this.data.length) {
      this.selectableItems.forEach((x) => (x.selected = value.some((y) => x.item[this.valueField] === y)));
    } else {
      this.selectableItems.forEach((x) => (x.selected = false));
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
