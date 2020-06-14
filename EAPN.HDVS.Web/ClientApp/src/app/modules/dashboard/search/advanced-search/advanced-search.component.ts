import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IFichaBusquedaDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { RangeService } from 'src/app/core/services/range.service';
import { SearchService } from 'src/app/core/services/search.service';
import { ISearchQuery, SearchQuery } from 'src/app/shared/models/search-query';

@Component({
  selector: 'app-advanced-search',
  templateUrl: './advanced-search.component.html',
  styleUrls: ['./advanced-search.component.scss'],
})
export class AdvancedSearchComponent implements OnInit {
  public query: ISearchQuery = new SearchQuery();
  public results: IFichaBusquedaDto[];

  constructor(
    private service: SearchService,
    private authService: AuthenticationService,
    private rangeService: RangeService,
    private alertService: AlertService
  ) {
    this.service.getQueryObservable().subscribe((query) => {
      this.query = new SearchQuery(query);
      this.onSearch();
    });
  }

  async ngOnInit(): Promise<void> {
    await this.rangeService.forceRefresh();
  }

  public async onSearch(): Promise<void> {
    try {
      const partnerId: number = await this.authService.getUserPartnerId().toPromise();
      this.query.partnerId = partnerId;

      this.results = await this.service.findCards(this.query).toPromise();
    } catch (error) {
      this.alertService.error(error);
    }
  }
}
