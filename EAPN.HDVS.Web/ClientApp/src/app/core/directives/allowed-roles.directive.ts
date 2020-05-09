import { Directive, TemplateRef, ViewContainerRef, Input, OnInit, OnDestroy } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appAllowedRoles]',
})
export class AllowedRolesDirective implements OnDestroy {
  private currentUserRolesSubs: Subscription;

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef, private authService: AuthenticationService) {}

  ngOnDestroy(): void {
    if (!!this.currentUserRolesSubs) {
      this.currentUserRolesSubs.unsubscribe();
    }
  }

  @Input() set appAllowedRoles(roles: string[]) {
    this.setView(false);
    this.currentUserRolesSubs = this.authService.isAuthorized(roles).subscribe(x => this.setView(x));
  }

  private setView(hasAccess: boolean): void {
    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
