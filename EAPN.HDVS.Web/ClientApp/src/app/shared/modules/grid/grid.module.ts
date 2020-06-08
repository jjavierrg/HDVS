import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from './grid/grid.component';
import { AgGridModule } from 'ag-grid-angular';
import { CoreModule } from 'src/app/core/core.module';
import { CheckboxCellComponent } from './checkbox-cell/checkbox-cell.component';
import { ArrayCellComponent } from './array-cell/array-cell.component';
import { DescriptionArrayCellComponent } from './array-cell/description-array-cell.component';
import { InputModule } from '../input/input.module';
import { DateCellComponent } from './date-cell/date-cell.component';
import { IndicatorRangeCellComponent } from './indicator-range-cell/indicator-range-cell.component';

@NgModule({
  declarations: [
    GridComponent,
    CheckboxCellComponent,
    ArrayCellComponent,
    DescriptionArrayCellComponent,
    DateCellComponent,
    IndicatorRangeCellComponent,
  ],
  imports: [CommonModule, CoreModule, AgGridModule.withComponents([]), InputModule],
  exports: [GridComponent],
  entryComponents: [
    CheckboxCellComponent,
    ArrayCellComponent,
    DescriptionArrayCellComponent,
    DateCellComponent,
    IndicatorRangeCellComponent,
  ],
})
export class GridModule {}
