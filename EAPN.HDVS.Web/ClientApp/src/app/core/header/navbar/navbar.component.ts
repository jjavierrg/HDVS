import { Component } from '@angular/core';
import { AlertService } from '../../services/alert.service';
import { Roles } from '../../enums/roles.enum';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  public isMenuCollapsed = true;
  public roles = Roles;

  constructor(private alertService: AlertService) {}
}
