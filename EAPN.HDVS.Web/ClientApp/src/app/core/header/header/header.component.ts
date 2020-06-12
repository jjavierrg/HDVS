import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatosUsuarioDto, OrganizacionDto } from '../../api/api.client';
import { Permissions } from '../../enums/permissions.enum';
import { AuthenticationService } from '../../services/authentication.service';
import { PartnerService } from '../../services/partner.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  public usuario: DatosUsuarioDto;
  public partner: OrganizacionDto;
  public permissions = Permissions;

  constructor(private authService: AuthenticationService, private partnerService: PartnerService, private router: Router) {}

  ngOnInit() {
    this.authService.getDatosUsuario().subscribe((x) => (this.usuario = x));
    this.authService.getUserPartnerId().subscribe((x) => {
      this.partnerService.getOrganizacion(x).subscribe((p) => (this.partner = p));
    });
  }

  public onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
