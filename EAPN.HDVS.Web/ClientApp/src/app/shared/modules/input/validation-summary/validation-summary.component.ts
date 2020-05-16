import { Component, OnInit, Input } from '@angular/core';
import { NgForm, FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-validation-summary',
  templateUrl: './validation-summary.component.html',
  styleUrls: ['./validation-summary.component.scss'],
})
export class ValidationSummaryComponent implements OnInit {
  @Input() public form: NgForm;
  public errors: string[] = [];

  constructor(private translate: TranslateService) {}

  ngOnInit() {
    if (this.form instanceof NgForm === false) {
      throw new Error('You must supply the validation summary with an NgForm.');
    }
    this.form.statusChanges.subscribe(() => {
      this.resetErrorMessages();
      this.generateErrorMessages(this.form.control);
    });
  }

  resetErrorMessages() {
    this.errors.length = 0;
  }

  generateErrorMessages(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((controlName) => {
      const control = formGroup.controls[controlName];
      const errors = control.dirty || control.touched ? control.errors : null;
      if (errors === null || errors.count === 0) {
        return;
      }
      // Handle the 'required' case
      if (errors.required) {
        this.errors.push(this.translate.instant('validaciones.obligatorio', { controlName }));
      }
      // Handle 'minlength' case
      if (errors.minlength) {
        this.errors.push(
          this.translate.instant('validaciones.longitud-minima', { controlName, requiredLength: errors.minlength.requiredLength })
        );
      }
      // Handle 'maxlength' case
      if (errors.maxlength) {
        this.errors.push(
          this.translate.instant('validaciones.longitud-maxima', { controlName, requiredLength: errors.maxlength.requiredLength })
        );
      }
      // Handle 'email' case
      if (errors.email) {
        this.errors.push(this.translate.instant('validaciones.email-no-valido', { controlName }));
      }
      // Handle 'unique email' case
      if (errors.uniqueMail) {
        this.errors.push(this.translate.instant('validaciones.email-no-unico'));
      }

      // Handle password confirmation case
      if (errors.passnotvalid) {
        this.errors.push(this.translate.instant('validaciones.confirmacion-clave'));
      }

      // Handle 'At least one' validation
      if (errors.atLeatOne) {
        this.errors.push(
          this.translate.instant('validaciones.al-menos', { controlNames: errors.controls.join(', '), required: errors.minimunRequired })
        );
      }
    });
  }
}
