import { Component, OnInit, Input } from '@angular/core';
import { FichaDto, DimensionDto, SeguimientoDto } from 'src/app/core/api/api.client';
import { Observable } from 'rxjs';
import { IndicatorService } from 'src/app/core/services/indicator.service';

@Component({
  selector: 'app-indicator-form',
  templateUrl: './indicator-form.component.html',
  styleUrls: ['./indicator-form.component.scss'],
})
export class IndicatorFormComponent implements OnInit {
  @Input() review: SeguimientoDto;
  @Input() enabled: boolean = true;

  public dimensions: DimensionDto[] = [];
  public isCollapsed: boolean = true;
  public activeTab: number = 0;

  constructor(private indicatorService: IndicatorService) { }

  async ngOnInit() {
    this.dimensions = await this.indicatorService.getDimensions().toPromise();
  }

  public isLastPage(): boolean {
    return this.activeTab >= this.dimensions.length - 1;
  }

  public moveNext(): void {
    if (!this.isLastPage()) {
      this.activeTab += 1;
    }
  }

  public movePrevious(): void {
    if (this.activeTab > 0) {
      this.activeTab -= 1;
    }
  }
}
