import { Component, HostListener, ElementRef } from '@angular/core';
import { AlertService } from '../../services/alert.service';
import { Permissions } from '../../enums/permissions.enum';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  public isMenuCollapsed = true;
  public permissions = Permissions;

  constructor(private alertService: AlertService, private eRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  private onClickOutside(event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isMenuCollapsed = true;
    }
  }
}
