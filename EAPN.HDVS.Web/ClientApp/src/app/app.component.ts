import { Component, OnInit } from '@angular/core';
import { LoadingService } from './core/services/loading.service';
import { TranslateService } from '@ngx-translate/core';
import { RangeService } from './core/services/range.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  public showLoader: boolean = false;
  public activeLang: string = 'es';

  constructor(private loadingService: LoadingService, private translate: TranslateService, private rangeService: RangeService) {
    this.translate.setDefaultLang(this.activeLang);
  }

  async ngOnInit() {
    await this.rangeService.forceRefresh();
    this.loadingService.getLoadingObservable().subscribe((x) => (this.showLoader = x));
  }
}
