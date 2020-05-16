import { NgModule } from '@angular/core';
import { AllowedPermissionsDirective } from './directives/allowed-permissions.directive';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { UniqueEmailDirective } from './directives/unique-email.directive';
import { PasswordRestrinctionsDirective } from './directives/password-restrictions.directive';
import { AtLeastOneDirective } from './directives/at-least-one.directive';

@NgModule({
  declarations: [AllowedPermissionsDirective, UniqueEmailDirective, PasswordRestrinctionsDirective, AtLeastOneDirective],
  imports: [
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: (http: HttpClient) => {
          return new TranslateHttpLoader(http);
        },
        deps: [HttpClient],
      },
      isolate: false,
    }),
  ],
  exports: [AllowedPermissionsDirective, UniqueEmailDirective, PasswordRestrinctionsDirective, TranslateModule, AtLeastOneDirective],
})
export class CoreModule {}
