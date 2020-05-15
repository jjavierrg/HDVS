import { ErrorHandler, Injectable } from '@angular/core';
import { AlertService } from '../services/alert.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private alertService: AlertService) {}

  handleError(error: any) {
    this.alertService.error(error);
  }
}
