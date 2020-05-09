import { NgModule } from '@angular/core';
import { AllowedRolesDirective } from './directives/allowed-roles.directive';

@NgModule({
  declarations: [AllowedRolesDirective],
  imports: [],
  exports: [AllowedRolesDirective],
})
export class CoreModule {}
