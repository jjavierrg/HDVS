import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateParserFormatter, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { DateParserFormatter } from './helpers/date-parse-formatter';
import { DatepickerTranslation } from './helpers/datepicker-translation';
import { DateAdapter } from './helpers/date-adapter';

const adapter = DateAdapter;

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'],
  providers: [
    { provide: NgbDatepickerI18n, useClass: DatepickerTranslation },
    { provide: NgbDateAdapter, useClass: adapter },
    { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatepickerComponent),
      multi: true,
    },
  ],
})
export class DatepickerComponent implements ControlValueAccessor {
  @Input() public disabled: boolean = false;
  @Input() public placeHolder: string = 'dd-mm-yyyy';

  private _value: Date;
  public get value(): Date {
    return this._value;
  }
  public set value(v: Date) {
    this._value = v;
    this.onChange(v);
  }

  public onChange = (value: Date) => {};
  public onTouched = () => {};

  constructor(private ngbCalendar: NgbCalendar) {}

  public setToday(): void {
    this.value = new Date();
  }

  writeValue(value: Date): void {
    this.value = value;
  }

  registerOnChange(fn: (checked: Date) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
