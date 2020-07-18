import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class DateAdapter extends NgbDateAdapter<Date> {
  fromModel(value: Date | null): NgbDateStruct | null {
    if (typeof value === 'string') {
      return null;
    }

    try {
      return {
        day: value.getDate(),
        month: value.getMonth() + 1,
        year: value.getFullYear(),
      };
    } catch (error) {
      return null;
    }
  }

  toModel(date: NgbDateStruct | null): Date | null {
    return date ? new Date(Date.UTC(date.year, date.month - 1, date.day, 0, 0, 0, 0)) : null;
  }
}
