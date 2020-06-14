import { Component, Input } from '@angular/core';
import { IFichaBusquedaDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss'],
})
export class SearchResultComponent {
  @Input() public results: IFichaBusquedaDto[];

  constructor() {}
}
