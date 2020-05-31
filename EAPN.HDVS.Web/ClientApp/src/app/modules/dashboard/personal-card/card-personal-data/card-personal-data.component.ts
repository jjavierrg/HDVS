import { Component, OnInit, Input, ViewChild, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { Card } from 'src/app/shared/models/card';
import { MasterDataDto } from 'src/app/core/api/api.client';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-card-personal-data',
  templateUrl: './card-personal-data.component.html',
  styleUrls: ['./card-personal-data.component.scss'],
})
export class CardPersonalDataComponent implements OnInit, AfterViewInit {
  @ViewChild(NgForm, { static: false }) form: NgForm;

  @Input() card: Card;
  @Output() cardValid = new EventEmitter<boolean>();

  public genders: MasterDataDto[];
  public situacAdminis: MasterDataDto[];
  public countries: MasterDataDto[];
  public provincias: MasterDataDto[];
  public municipios: MasterDataDto[] = [];

  constructor(private masterdataService: MasterdataService) {}

  async ngOnInit() {
    // Load Data
    const [genders, countries, provincias, situacAdminis] = await Promise.all([
      this.masterdataService.getGenders().toPromise(),
      this.masterdataService.getCountries().toPromise(),
      this.masterdataService.getProvincias().toPromise(),
      this.masterdataService.getSituacionesAdministrativas().toPromise(),
    ]);

    this.genders = genders;
    this.countries = countries;
    this.provincias = provincias;
    this.situacAdminis = situacAdminis;

    if (this.card.provinciaId) {
      this.municipios = await this.masterdataService.getMunicipiosByProvincia(this.card.provinciaId).toPromise();
    }
  }

  public async onProvinciaChange(provinciaId: number): Promise<void> {
    this.municipios = await this.masterdataService.getMunicipiosByProvincia(provinciaId).toPromise();
  }

  ngAfterViewInit(): void {
    this.form.statusChanges.subscribe(() => {
      this.cardValid.emit(this.form.valid);
    });
  }
}
