import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckboxComponent } from './checkbox/checkbox.component';
import { ComboboxComponent } from './combobox/combobox.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxMultipleComponent } from './combobox-multiple/combobox-multiple.component';
import { ValidationSummaryComponent } from './validation-summary/validation-summary.component';

@NgModule({
  declarations: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, ValidationSummaryComponent],
  imports: [CommonModule, NgbDropdownModule],
  exports: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, ValidationSummaryComponent]
})
export class InputModule {}
