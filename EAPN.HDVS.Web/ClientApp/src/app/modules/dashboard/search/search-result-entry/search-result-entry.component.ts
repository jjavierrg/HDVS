import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IFichaBusquedaDto } from 'src/app/core/api/api.client';
import { RangeService } from 'src/app/core/services/range.service';
import { IReviewState } from 'src/app/shared/models/reviewState';
import { Permissions } from 'src/app/core/enums/permissions.enum';

@Component({
  selector: 'app-search-result-entry',
  templateUrl: './search-result-entry.component.html',
  styleUrls: ['./search-result-entry.component.scss'],
})
export class SearchResultEntryComponent {
  @Input() data: IFichaBusquedaDto;
  public permissions = Permissions;

  constructor(private rangeService: RangeService, private router: Router) {}

  public getRangeDescription(value: number): string {
    return this.rangeService.getRangeDescriptionByScore(value);
  }

  public onViewReviewClick(id: number): void {
    const state: IReviewState = { readonly: true };
    this.router.navigate(['/seguimientos', id], { state });
  }

  public onViewCardClick(id: number, segment: string): void {
    if (segment) {
      segment = '#' + segment;
    } else {
      segment = '';
    }

    this.router.navigateByUrl(`fichas/${id}${segment}`);
  }
}
