import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  /**
   * URLs for which the loading screen should not be enabled
   */
  skippUrls = ['/auth'];

  constructor(private loadingService: LoadingService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let displayLoadingScreen = true;

    for (const skippUrl of this.skippUrls) {
      if (new RegExp(skippUrl.toLowerCase()).test(request.url.toLowerCase())) {
        displayLoadingScreen = false;
        break;
      }
    }

    if (displayLoadingScreen) {
      this.loadingService.showLoader();

      return next.handle(request).pipe(
        finalize(() => this.loadingService.hideLoader())
      );
    } else {
      return next.handle(request);
    }
  }
}
