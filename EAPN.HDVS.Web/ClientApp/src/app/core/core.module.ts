import { NgModule } from '@angular/core';
import { AllowedRolesDirective } from './directives/allowed-roles.directive';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@NgModule({
  declarations: [AllowedRolesDirective],
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
  exports: [AllowedRolesDirective, TranslateModule],
})
export class CoreModule {}
