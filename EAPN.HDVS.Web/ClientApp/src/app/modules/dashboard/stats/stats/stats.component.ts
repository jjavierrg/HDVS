import { Component, OnInit } from '@angular/core';
import { DimensionDto, MasterDataDto, RangoDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { IndicatorService } from 'src/app/core/services/indicator.service';
import { Permissions } from 'src/app/core/enums/permissions.enum';

class StatsFilters {
  public searchByAllOrganizacion: boolean;
  public searchByFechaAlta: boolean;
  public searchBySexo: boolean;
  public searchByGenero: boolean;
  public searchByNacionalidad: boolean;
  public searchByPaisOrigen: boolean;
  public searchByCodPostal: boolean;
  public idOrganizacion: boolean;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idSexo: boolean;
  public idGenero: boolean;
  public idNacionalidad: boolean;
  public idPaisOrigen: boolean;
  public codPostal: boolean;
}

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss'],
})
export class StatsComponent implements OnInit {
  public isCollapsed: boolean = true;
  public filters: StatsFilters = new StatsFilters();
  public sexos: MasterDataDto[];
  public generos: MasterDataDto[];
  public paises: MasterDataDto[];
  public dimensiones: DimensionDto[];
  public rangos: RangoDto[];
  public permissions = Permissions;

  constructor(
    private masterdataService: MasterdataService,
    private indicatorService: IndicatorService,
    private alertService: AlertService
  ) {}

  async ngOnInit(): Promise<void> {
    try {
      const [sexos, paises, dimensiones, rangos] = await Promise.all([
        this.masterdataService.getSexos().toPromise(),
        this.masterdataService.getPaises().toPromise(),
        this.indicatorService.getDimensions().toPromise(),
        this.masterdataService.getRangos().toPromise(),
      ]);

      this.sexos = sexos;
      this.paises = paises;
      this.dimensiones = dimensiones;
      this.rangos = rangos;
    } catch (err) {
      this.alertService.error(err);
    }
  }
}
