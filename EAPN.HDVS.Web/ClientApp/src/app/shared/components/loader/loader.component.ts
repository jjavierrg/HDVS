import { Component, OnInit } from '@angular/core';
import { LoadingService } from 'src/app/core/services/loading.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {
  public showLoader = false;
  constructor(private loadingServie: LoadingService) {}

  ngOnInit() {
    this.showLoader = this.loadingServie.getCurrentState();
    this.loadingServie
      .getLoadingObservable()
      .subscribe(show => (this.showLoader = show));
  }
}
