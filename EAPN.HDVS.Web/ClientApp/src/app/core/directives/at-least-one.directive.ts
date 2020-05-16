import { Directive, Input } from '@angular/core';
import { AbstractControl, ValidationErrors, NG_VALIDATORS, Validator } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appAtLeastOne]',
  providers: [{ provide: NG_VALIDATORS, useExisting: AtLeastOneDirective, multi: true }],
})
export class AtLeastOneDirective implements Validator {
  @Input() compareTo: AbstractControl[];
  @Input() controlNames: string[];
  @Input() requiredNumber: number = 1;

  validate(control: AbstractControl): ValidationErrors {
    if (this.compareTo && this.compareTo.length) {
      this.compareTo.forEach((c) => {
        const subscription: Subscription = c.valueChanges.subscribe((x) => {
          control.updateValueAndValidity();
          subscription.unsubscribe();
        });
      });

      let completed: number = !!control.value ? 1 : 0;
      if (this.compareTo && this.compareTo.length) {
        completed += this.compareTo.filter((x) => !!x.value).length;
      }

      return completed < this.requiredNumber
        ? { atLeatOne: true, controls: this.controlNames, minimunRequired: this.requiredNumber }
        : null;
    }
  }
}
