import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatosUsuarioDto } from '../../api/api.client';
import { Permissions } from '../../enums/permissions.enum';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  public usuario: DatosUsuarioDto;
  public permissions = Permissions;

  constructor(private authService: AuthenticationService, private router: Router) {}

  ngOnInit() {
    this.authService.getDatosUsuario().subscribe((x) => (this.usuario = x));
  }

  public onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
