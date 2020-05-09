import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from './core/services/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  public showLoader: boolean = false;

  constructor(private loadingService: LoadingService) {
  }

  ngOnInit() {
    this.loadingService.getLoadingObservable().subscribe(x => this.showLoader = x);
  }
}
