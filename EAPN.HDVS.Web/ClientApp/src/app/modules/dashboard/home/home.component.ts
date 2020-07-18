import { Component, OnInit } from '@angular/core';
import { IDatosUsuarioDto, IResumenExpedientesDto } from 'src/app/core/api/api.client';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { CardService } from 'src/app/core/services/card.service';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { SearchType } from 'src/app/shared/models/search-query';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public user: IDatosUsuarioDto;
  public dashboard: IResumenExpedientesDto;
  public permissions = Permissions;
  public searchType = SearchType;

  constructor(private authService: AuthenticationService, private cardService: CardService) {
    this.authService.getDatosUsuario().subscribe(x => this.user = x);
    this.cardService.getDashboad().subscribe(x => this.dashboard = x);
  }

  async ngOnInit() { }
}
