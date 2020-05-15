import { Directive, Input } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { UserManagementService } from '../services/user-management.service';
import { AsyncValidator, AbstractControl, ValidationErrors, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, tap, finalize } from 'rxjs/operators';

@Directive({
  selector: '[appUniqueEmail]',
  providers: [{ provide: NG_ASYNC_VALIDATORS, useExisting: UniqueEmailDirective, multi: true }],
})
export class UniqueEmailDirective implements AsyncValidator {
  @Input() userId?: number;
  private onChange: () => void;

  constructor(private userManagementService: UserManagementService) {}

  validate(control: AbstractControl): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.userManagementService
      .emailTaken(control.value, this.userId)
      .pipe(map((taken) => (taken ? { uniqueMail: true } : null)));
  }
  registerOnValidatorChange?(fn: () => void): void {
    this.onChange = fn;
  }
}
