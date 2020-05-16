import { Injectable } from '@angular/core';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

// Define custom service providing the months and weekdays translations
@Injectable()
export class DatepickerTranslation extends NgbDatepickerI18n {
  private months: string[];
  private days: string[];

  constructor(private translate: TranslateService) {
    super();
    this.months = translate.instant('core.calendario.meses').split(',');
    this.days = translate.instant('core.calendario.dias').split(',');
  }

  getWeekdayShortName(weekday: number): string {
    return this.days[weekday - 1].substr(0, 2);
  }
  getMonthShortName(month: number): string {
    return this.getMonthFullName(month).substr(0, 3);
  }
  getMonthFullName(month: number): string {
    return this.months[month - 1];
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}
