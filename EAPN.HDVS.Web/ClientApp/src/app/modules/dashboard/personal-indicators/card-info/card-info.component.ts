import { Component, OnInit, Input } from '@angular/core';
import { CardService } from 'src/app/core/services/card.service';
import { FichaDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-card-info',
  templateUrl: './card-info.component.html',
  styleUrls: ['./card-info.component.scss']
})
export class CardInfoComponent implements OnInit {
  @Input() cardId: number;

  public card: FichaDto;

  constructor(private cardService: CardService) {
  }

  async ngOnInit() {
    this.card = await this.cardService.getCard(this.cardId).toPromise();
  }

}
