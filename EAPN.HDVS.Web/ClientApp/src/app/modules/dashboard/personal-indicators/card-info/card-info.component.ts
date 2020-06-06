import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { CardService } from 'src/app/core/services/card.service';
import { DatosFichaDto } from 'src/app/core/api/api.client';
import { Permissions } from 'src/app/core/enums/permissions.enum';

@Component({
  selector: 'app-card-info',
  templateUrl: './card-info.component.html',
  styleUrls: ['./card-info.component.scss']
})
export class CardInfoComponent implements OnInit {
  @Input() cardId: number;
  @Output() editCardRequired = new EventEmitter<number>();

  public card: DatosFichaDto;
  public permissions = Permissions;

  constructor(private cardService: CardService) {
  }

  async ngOnInit() {
    this.card = await this.cardService.getPersonalData(this.cardId).toPromise();
  }

  public onEditCardClick() {
    this.editCardRequired.emit(this.cardId);
  }

}
