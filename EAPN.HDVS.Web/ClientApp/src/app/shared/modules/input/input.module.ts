import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckboxComponent } from './checkbox/checkbox.component';
import { ComboboxComponent } from './combobox/combobox.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxMultipleComponent } from './combobox-multiple/combobox-multiple.component';
import { ValidationSummaryComponent } from './validation-summary/validation-summary.component';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, ValidationSummaryComponent],
  imports: [
    CommonModule,
    NgbDropdownModule,
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

          ['link', 'image', 'video'], // link and image, video
        ],
      },
    }),
  ],
  exports: [CheckboxComponent, ComboboxComponent, ComboboxMultipleComponent, ValidationSummaryComponent, QuillModule],
})
export class InputModule {}
