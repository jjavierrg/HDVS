import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoadingService } from './core/services/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  public showLoader: boolean = false;
  public activeLang: string = 'es';

  constructor(private loadingService: LoadingService, private translate: TranslateService) {
    this.translate.setDefaultLang(this.activeLang);
  }

  async ngOnInit() {
    this.loadingService.getLoadingObservable().subscribe((x) => (this.showLoader = x));
  }
}
