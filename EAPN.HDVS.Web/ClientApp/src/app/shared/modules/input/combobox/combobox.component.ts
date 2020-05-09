import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-combobox',
  templateUrl: './combobox.component.html',
  styles: [''],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ComboboxComponent),
      multi: true,
    },
  ],
})
export class ComboboxComponent<T> implements ControlValueAccessor {
  @Input() public placeholder: string;
  @Input() public valueField: string;
  @Input() public displayField: string;
  @Input() public data: Array<T>;
  @Input() public disabled: boolean = false;

  @Output() selectionChange = new EventEmitter<T>();
  @Output() valueChange = new EventEmitter<any>();

  public selection: T;
  public onChange = (fn: any) => {};
  public onTouched = () => {};

  public getControlText(): string {
    if (!this.selection) {
      return this.placeholder;
    } else if (!this.displayField) {
      return `${this.selection}`;
    } else {
      return `${this.selection[this.displayField]}`;
    }
  }

  public getItemText(item: T): string {
    if (!item) {
      return '';
    } else if (!this.displayField) {
      return `${item}`;
    } else {
      return `${item[this.displayField]}`;
    }
  }

  public onValueChanged(item: T) {
    if (this.disabled) {
      return;
    }

    this.selection = item;
    let value: any;
    if (!this.valueField) {
      value = this.selection;
    } else {
      value = this.selection[this.valueField];
    }

    this.onChange(value);
    this.selectionChange.emit(item);
    this.valueChange.emit(value);
  }

  writeValue(value: any): void {
    if (!this.valueField) {
      this.selection = value;
    } else if (this.data && this.data.length) {
      this.selection = this.data.find((x) => x[this.valueField] === value);
    } else {
      this.selection = null;
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
