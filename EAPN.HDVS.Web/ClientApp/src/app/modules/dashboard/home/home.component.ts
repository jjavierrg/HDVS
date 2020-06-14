import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { Observable } from 'rxjs';
import { IFichaDto } from 'src/app/core/api/api.client';
import { CardService } from 'src/app/core/services/card.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public cards: Observable<IFichaDto[]>;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('comun.nombre'),
      field: 'nombre',
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.apellido1'),
      field: 'apellido1',
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.apellido2'),
      field: 'apellido2',
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('ficha.dni-abrev'),
      field: 'dni',
      filter: 'agTextColumnFilter',
      maxWidth: 200
    },
  ];

  constructor(private cardService: CardService, private translate: TranslateService, private router: Router) {
    this.cards = this.cardService.getAllCards();
  }

  async ngOnInit() { }

  public onEditCard(card: IFichaDto): void {
    if (!card) {
      return;
    }

    this.router.navigate(['fichas', card.id]);
  }
}
