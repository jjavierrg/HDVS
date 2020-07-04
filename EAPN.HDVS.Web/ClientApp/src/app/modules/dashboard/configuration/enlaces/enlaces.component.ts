import { Component, OnInit, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ConfiguracionDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-enlaces',
  templateUrl: './enlaces.component.html',
  styleUrls: ['./enlaces.component.scss'],
})
export class EnlacesComponent implements OnInit, OnChanges {
  @Input() public configuration: ConfiguracionDto;
  @Output() public configurationChange = new EventEmitter<ConfiguracionDto>();

  public links: { description: string; link: string }[] = [];

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    this.getLinks();
  }

  ngOnInit(): void {
    this.getLinks();
  }

  public onDeleteLinkClick(link: { description: string; link: string }): void {
    this.links = this.links.filter((x) => x !== link);
    this.onChange();
  }

  public onAddLinkClick(): void {
    this.links.push({ description: '', link: '' });
  }

  public onChange(): void {
    const links = this.links.filter((x) => !!x.link);
    links.forEach((x) => (x.description = x.description || x.link));
    this.configuration.enlaces = links.map((x) => `${x.description}|${x.link}`).join(';');
  }

  private getLinks(): void {
    if (!this.configuration) {
      this.links = [];
      return;
    }

    const links = this.configuration.enlaces.split(';').filter((x) => x && x.includes('|'));
    this.links = links.map((x) => {
      const parts = x.split('|');
      return { description: parts[0], link: parts[1] };
    });
  }
}
