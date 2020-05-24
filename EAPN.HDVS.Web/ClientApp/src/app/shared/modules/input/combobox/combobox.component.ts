import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MasterDataDto } from 'src/app/core/api/api.client';

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
export class ComboboxComponent implements ControlValueAccessor {
  @Input() public placeholder: string;
  @Input() public data: Array<MasterDataDto>;
  @Input() public disabled: boolean = false;
  @Output() valueChange = new EventEmitter<number>();

  public selection: number;
  public onChange = (fn: number) => {};
  public onTouched = () => {};

  public notifyChange(): void {
    if (this.disabled) {
      return;
    }

    this.valueChange.emit(+this.selection);
    this.onChange(+this.selection);
  }

  writeValue(value: any): void {
    this.selection = value;
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
