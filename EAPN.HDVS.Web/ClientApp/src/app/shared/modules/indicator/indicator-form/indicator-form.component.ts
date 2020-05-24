import { Component, OnInit, Input } from '@angular/core';
import { FichaDto, DimensionDto } from 'src/app/core/api/api.client';
import { Observable } from 'rxjs';
import { IndicatorService } from 'src/app/core/services/indicator.service';

@Component({
  selector: 'app-indicator-form',
  templateUrl: './indicator-form.component.html',
  styleUrls: ['./indicator-form.component.scss'],
})
export class IndicatorFormComponent implements OnInit {
  @Input() card: FichaDto;

  public dimensions: Observable<DimensionDto[]>;
  public isCollapsed: boolean = true;
  public activeTab: number = 0;

  constructor(private indicatorService: IndicatorService) {
    this.dimensions = indicatorService.getDimensions();
  }

  ngOnInit() {}
}
