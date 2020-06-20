import { Component, OnInit, HostListener } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoadingService } from './core/services/loading.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  public showLoader: boolean = false;
  public activeLang: string = 'es';
  public selfDestructing: boolean = false;

  constructor(private loadingService: LoadingService, private translate: TranslateService) {
    this.translate.setDefaultLang(this.activeLang);

    window.addEventListener('storage', (event) => {
      if (this.selfDestructing || event.key !== environment.tokenLocalStorageKey) {
        return;
      }

      // there is another app window open, so put token again in localstorage
      if (event.oldValue && !event.newValue) {
        localStorage.setItem(environment.tokenLocalStorageKey, event.oldValue);
      }
    });
  }

  async ngOnInit() {
    this.loadingService.getLoadingObservable().subscribe((x) => (this.showLoader = x));
  }

  @HostListener('window:beforeunload')
  clearLocalStorageToken() {
    localStorage.removeItem(environment.tokenLocalStorageKey);
  }
}
