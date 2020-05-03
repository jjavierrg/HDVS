import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Subject, Observable } from 'rxjs';
import { Alert, AlertType } from '../../shared/components/alert/alert';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private subject = new Subject<Alert>();
  private keepAfterRouteChange = false;

  constructor(private router: Router) {
    // clear alert messages on route change unless 'keepAfterRouteChange' flag is true
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        if (this.keepAfterRouteChange) {
          // only keep for a single route change
          this.keepAfterRouteChange = false;
        } else {
          // clear alert messages
          this.clear();
        }
      }
    });
  }

  public getAlert(): Observable<any> {
    return this.subject.asObservable();
  }

  public success(message: string, closeAfter: number = 5000): void {
    this.alert(AlertType.Success, message, closeAfter);
  }

  public error(message: string, closeAfter: number = 5000): void {
    this.alert(AlertType.Error, message, closeAfter);
  }

  public info(message: string, closeAfter: number = 5000): void {
    this.alert(AlertType.Info, message, closeAfter);
  }

  public warning(message: string, closeAfter: number = 5000): void {
    this.alert(AlertType.Warning, message, closeAfter);
  }

  public alert(type: AlertType, message: string, closeAfter: number = null): void {
    this.subject.next(<Alert>{ type: type, message: message, displayTime: closeAfter});
  }

  clear() {
    // clear alerts
    this.subject.next();
  }
}
