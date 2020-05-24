import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CategoriaDto, IndicadorDto, IndicadorFichaDto } from 'src/app/core/api/api.client';

interface IIndicatorItem {
  category: CategoriaDto;
  indicator: IndicadorFichaDto;
}

@Component({
  selector: 'app-indicator-category-form',
  templateUrl: './indicator-category-form.component.html',
  styleUrls: ['./indicator-category-form.component.scss'],
})
export class IndicatorCategoryFormComponent implements OnInit {
  @Input() categories: CategoriaDto[];
  @Input() cardId: number;
  @Input() selection: IndicadorFichaDto[];
  @Input() enabled: boolean = true;
  @Output() selectionChange = new EventEmitter<IndicadorDto[]>();

  public internalItems: IIndicatorItem[];
  public showCloseAll: boolean = false;

  constructor() {}

  ngOnInit(): void {
    this.internalItems = this.categories.map((x) => <IIndicatorItem> { category: x, indicator: this.getCategorySelection(x) });
  }

  public onChange(): void {
    const values = (this.internalItems || []).filter(x => !!x.indicator.indicadorId);
    values.forEach(x => x.indicator.fichaId ? x.indicator.fichaId : this.cardId);

    this.selectionChange.emit(values.map(x => x.indicator));
  }

  private getCategorySelection(category: CategoriaDto): IndicadorFichaDto {
    const catIndicators = (category.indicadores || []).map((x) => x.id);

    if (!(catIndicators || []).length) {
      return new IndicadorFichaDto({ fichaId: this.cardId, indicadorId: null, observaciones: null });
    }

    const selected: IndicadorFichaDto = (this.selection || []).find((x) => catIndicators.some((c) => c === x.indicadorId));
    return selected ? selected : new IndicadorFichaDto({ fichaId: this.cardId, indicadorId: null, observaciones: null });
  }
}
