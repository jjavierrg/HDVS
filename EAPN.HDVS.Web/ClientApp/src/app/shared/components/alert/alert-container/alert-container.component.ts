import { Component, OnInit } from '@angular/core';

import { AlertService } from '../../../../core/services/alert.service';
import { Alert } from '../alert';

@Component({
  selector: 'app-alert-container',
  styles: [':host { position: fixed; top: 0; right: 0; margin: .5em; z-index: 1200; display: flex; flex-direction: column; align-items: flex-end;}'],
  template: ` <app-alert *ngFor="let alert of alerts" [alert]="alert" (close)="onRemoveAlert(alert)"></app-alert> `,
})
export class AlertContainerComponent implements OnInit {
  alerts: Alert[] = [];

  constructor(private alertService: AlertService) {}

  ngOnInit() {
    this.alertService.getAlert().subscribe((alert: Alert) => {
      if (!alert) {
        // clear alerts when an empty alert is received
        this.alerts = [];
        return;
      }
      this.alerts.push(alert);
    });
  }

  onRemoveAlert(alert: Alert) {
    this.alerts = this.alerts.filter((x) => x !== alert);
  }
}
