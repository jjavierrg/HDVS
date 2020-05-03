import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Alert } from '../alert';
import { timer, Subscription } from 'rxjs';

const step = 100;

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
})
export class AlertComponent implements OnInit, OnDestroy {
  @Input() alert: Alert;
  @Output() close = new EventEmitter<void>();

  public isMouseOver = false;
  public progress: number;

  private progressSuscription: Subscription;
  constructor() {}

  ngOnInit() {
    if (this.alert.displayTime) {
      this.progress = this.alert.displayTime;

      this.progressSuscription = timer(step, step).subscribe((t) => {
        this.performStep();
      });
    }
  }

  ngOnDestroy() {
    this.finalizeSubscription();
  }

  public onClose(): void {
    this.finalizeSubscription();
    this.close.emit();
  }

  private performStep(): void {
    if (this.isMouseOver) {
      return;
    }

    if (this.progress <= 0) {
      this.onClose();
      return;
    }

    this.progress -= step;
  }

  private finalizeSubscription(): void {
    this.progress = 0;
    if (this.progressSuscription) {
      this.progressSuscription.unsubscribe();
      this.progressSuscription = null;
    }
  }
}
