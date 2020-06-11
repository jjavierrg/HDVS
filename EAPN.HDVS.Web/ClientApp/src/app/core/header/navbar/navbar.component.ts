import { Component, ElementRef, HostListener } from '@angular/core';
import { Permissions } from '../../enums/permissions.enum';
import { MasterdataService } from '../../services/masterdata.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  public isMenuCollapsed = true;
  public permissions = Permissions;
  public links: { description: string; link: string }[];

  constructor(private eRef: ElementRef, private masterdataService: MasterdataService) {
    this.masterdataService.getConfiguracion().subscribe((conf) => {
      if (conf && conf.enlaces && conf.mostrarEnlaces) {
        const links = conf.enlaces.split(';').filter((x) => x && x.includes('|'));
        this.links = links.map((x) => {
          const parts = x.split('|');
          return { description: parts[0], link: parts[1] };
        });
      }
    });
  }

  @HostListener('document:click', ['$event'])
  private onClickOutside(event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isMenuCollapsed = true;
    }
  }
}
