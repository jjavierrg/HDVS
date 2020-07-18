import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  private refreshingParams: boolean = false;

  constructor(
    private service: SearchService,
    private authService: AuthenticationService,
    private rangeService: RangeService,
    private alertService: AlertService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  async ngOnInit(): Promise<void> {
    await this.rangeService.forceRefresh();

    this.activatedRoute.queryParams.subscribe((params) => {
      if (params.q && !this.refreshingParams) {
        this.query = SearchQuery.fromJSON(params.q);
        this.onSearch(false);
      }
    });
  }

  public async onSearch(updateUrl: boolean = true): Promise<void> {
    try {
      if (updateUrl) {
        this.refreshingParams = true;
        this.router.navigate([], { relativeTo: this.activatedRoute, queryParams: { q: this.query.toJSON() } });
        this.refreshingParams = false;
      }

      const partnerId: number = await this.authService.getUserPartnerId().toPromise();
      this.query.partnerId = partnerId;

      this.results = await this.service.findCards(this.query).toPromise();
    } catch (error) {
      this.alertService.error(error);
    }
  }
}
