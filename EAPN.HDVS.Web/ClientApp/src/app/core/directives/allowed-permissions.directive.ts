import { Directive, TemplateRef, ViewContainerRef, Input, OnInit, OnDestroy } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appAllowedPermissions]',
})
export class AllowedPermissionsDirective implements OnDestroy {
  private currentUserPermissionsSubs: Subscription;

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef, private authService: AuthenticationService) {}

  ngOnDestroy(): void {
    if (!!this.currentUserPermissionsSubs) {
      this.currentUserPermissionsSubs.unsubscribe();
    }
  }

  @Input() set appAllowedPermissions(data: { permissions: string[], requireAll?: boolean }) {
    this.setView(false);
    this.currentUserPermissionsSubs = this.authService.isAuthorized(data.permissions, !!data.requireAll).subscribe(x => this.setView(x));
  }

  private setView(hasAccess: boolean): void {
    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
