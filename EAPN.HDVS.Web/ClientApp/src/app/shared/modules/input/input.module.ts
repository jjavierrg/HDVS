import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckboxComponent } from './checkbox/checkbox.component';
import { ComboboxComponent } from './combobox/combobox.component';
import { NgbDropdownModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxMultipleComponent } from './combobox-multiple/combobox-multiple.component';
import { ValidationSummaryComponent } from './validation-summary/validation-summary.component';
import { QuillModule } from 'ngx-quill';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { FormsModule } from '@angular/forms';
import { CoreModule } from 'src/app/core/core.module';

@NgModule({
  declarations: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, ValidationSummaryComponent, DatepickerComponent],
  imports: [
    CommonModule,
    CoreModule,
    NgbDropdownModule,
    FormsModule,
    NgbDatepickerModule,
    QuillModule.forRoot({
      modules: {
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }],
          [{ align: [] }],

          ['bold', 'italic', 'underline', 'strike'], // toggled buttons
          ['blockquote', 'code-block'],

          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }], // superscript/subscript
          [{ indent: '-1' }, { indent: '+1' }], // outdent/indent

          [{ color: [] }, { background: [] }], // dropdown with defaults from theme

          ['clean'], // remove formatting button

          ['link'], // link
        ],
      },
    }),
  ],
  exports: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, DatepickerComponent, ValidationSummaryComponent, QuillModule],
})
export class InputModule {}
