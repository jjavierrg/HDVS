import { Directive, Input } from '@angular/core';
import { Validator, AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appPasswordRestrinctions]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PasswordRestrinctionsDirective, multi: true }],
})
export class PasswordRestrinctionsDirective implements Validator {
  @Input() compareTo: AbstractControl;
  constructor() {}
  validate(control: AbstractControl): ValidationErrors {
    if (this.compareTo) {
      const subscription: Subscription = this.compareTo.valueChanges.subscribe((x) => {
        control.updateValueAndValidity();
        subscription.unsubscribe();
      });

      return this.compareTo && this.compareTo.value !== control.value ? { passnotvalid: true } : null;
    }
  }
}
