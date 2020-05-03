import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  constructor() { }

  private loadingSubject = new BehaviorSubject<boolean>(false);
  private loadingCount = 0;

  public getLoadingObservable(): Observable<boolean> {
    return this.loadingSubject.asObservable().pipe(debounceTime(250));
  }

  public showLoader(): void {
    this.loadingCount += 1;
    this.loadingSubject.next(true);
  }

  public hideLoader(): void {
    if (this.loadingCount <= 0) {
      this.loadingCount = 0;
      return;
    }

    this.loadingCount -= 1;
    if (this.loadingCount === 0) {
      this.loadingSubject.next(false);
    }
  }

  public getCurrentState(): boolean {
    return this.loadingCount > 0;
  }
}
