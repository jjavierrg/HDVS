import { Component, OnInit, Input } from '@angular/core';
import { NgForm, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-validation-summary',
  templateUrl: './validation-summary.component.html',
  styleUrls: ['./validation-summary.component.scss'],
})
export class ValidationSummaryComponent implements OnInit {
  @Input() public form: NgForm;
  public errors: string[] = [];

  constructor() {}

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
        this.errors.push(`El campo <strong>${controlName}</strong> es obligatorio.`);
      }
      // Handle 'minlength' case
      if (errors.minlength) {
        this.errors.push(`El campo <strong>${controlName}</strong> debe contener al menos ${errors.minlength.requiredLength} caracteres.`);
      }
      // Handle 'maxlength' case
      if (errors.maxlength) {
        this.errors.push(
          `El campo <strong>${controlName}</strong> debe contener como máximo ${errors.minlength.requiredLength} caracteres.`
        );
      }
      // Handle 'email' case
      if (errors.email) {
        this.errors.push(`El campo <strong>${controlName}</strong> debe contener un correo electrónico válido.`);
      }
    });
  }
}
