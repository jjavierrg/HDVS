import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IndicatorFormComponent } from './indicator-form/indicator-form.component';
import { CoreModule } from 'src/app/core/core.module';
import { NgbNavModule, NgbCollapseModule, NgbAccordionModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { IndicatorCategoryFormComponent } from './indicator-category-form/indicator-category-form.component';
import { InputModule } from '../input/input.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [IndicatorFormComponent, IndicatorCategoryFormComponent],
  imports: [CommonModule, CoreModule, NgbNavModule, NgbCollapseModule, NgbAccordionModule, InputModule, FormsModule, NgbTooltipModule],
  exports: [IndicatorFormComponent],
})
export class IndicatorModule {}
