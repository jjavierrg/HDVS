import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CategoriaDto, IndicadorDto, IndicadorSeguimientoDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-indicator-category-form',
  templateUrl: './indicator-category-form.component.html',
  styleUrls: ['./indicator-category-form.component.scss'],
})
export class IndicatorCategoryFormComponent implements OnInit {
  @Input() categories: CategoriaDto[];
  @Input() reviewId: number;
  @Input() selection: IndicadorSeguimientoDto[];
  @Input() enabled: boolean = true;
  @Output() selectionChange = new EventEmitter<IndicadorSeguimientoDto[]>();

  public internalCategories: { observations: string; category: CategoriaDto }[];
  public observations: string;
  public verified: boolean;
  public showCloseAll: boolean = false;

  constructor() {}

  ngOnInit(): void {
    this.internalCategories = this.categories.map((x) => ({ observations: '', category: x }));

    if (!this.selection) {
      this.selection = [];
    }

    this.selection.forEach((x) => {
      const cat = this.internalCategories.find((c) => x.indicador && c.category.id === x.indicador.categoriaId);
      if (cat) {
        cat.observations = x.observaciones;
      }
    });
  }

  public indicatorSelected(indicator: IndicadorDto): boolean {
    if (!indicator || !this.selection) {
      return false;
    }

    return this.selection.some((x) => x.indicadorId === indicator.id);
  }

  public setIndicatorSelected(indicator: IndicadorDto, selected: boolean, allowMultiple: boolean, observations: string): void {
    if (!indicator) {
      return;
    }

    if (!this.selection) {
      this.selection = [];
    }

    if (selected && !this.selection.some((x) => x.indicadorId === indicator.id)) {
      if (!allowMultiple) {
        this.selection = this.selection.filter((x) => x.indicador.categoriaId !== indicator.categoriaId);
      }

      this.selection.push(
        new IndicadorSeguimientoDto({
          indicadorId: indicator.id,
          indicador: indicator,
          seguimientoId: this.reviewId,
          observaciones: observations,
          verificado: false,
        })
      );
    } else if (!selected && this.selection.some((x) => x.indicadorId === indicator.id)) {
      this.selection = this.selection.filter((x) => x.indicadorId !== indicator.id);
    }

    this.selectionChange.emit(this.selection);
  }

  public showVerificationSection(category: CategoriaDto): boolean {
    if (!category) {
      return false;
    }

    return this.selection.some((x) => x.indicador && x.indicador.categoriaId === category.id && x.indicador.verificacion);
  }

  public verificationSelected(indicator: IndicadorDto): boolean {
    return indicator && this.selection && this.selection.some((x) => x.indicadorId === indicator.id && x.verificado);
  }

  public setVerificationSelected(indicator: IndicadorDto, selected: boolean): boolean {
    if (!indicator) {
      return;
    }

    const selection = this.selection.find((x) => x.indicadorId === indicator.id);
    if (selection) {
      selection.verificado = selected;
      this.selectionChange.emit(this.selection);
    }
  }

  public onObservationsChange(cat: { observations: string; category: CategoriaDto }): void {
    if (!cat) {
      return;
    }

    this.selection
      .filter((x) => x.indicador && x.indicador.categoriaId === cat.category.id)
      .forEach((x) => (x.observaciones = cat.observations));

    this.selectionChange.emit(this.selection);
  }

  public onClearSelectionClick(cat: { observations: string; category: CategoriaDto }, event: Event): void {
    event.stopPropagation();

    if (!cat || !this.selection) {
      return;
    }

    cat.observations = '';
    this.selection = this.selection.filter((x) => x.indicador && x.indicador.categoriaId !== cat.category.id);

    this.selectionChange.emit(this.selection);
  }
}
