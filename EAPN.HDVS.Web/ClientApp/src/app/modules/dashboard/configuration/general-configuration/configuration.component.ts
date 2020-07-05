import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfiguracionDto, RangoDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';
import { RangeService } from 'src/app/core/services/range.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class ConfigurationComponent implements OnInit {
  public configuration: ConfiguracionDto;
  public ranges: RangoDto[];

  constructor(
    private masterdataService: MasterdataService,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  async ngOnInit(): Promise<void> {
    const [config, ranges] = await Promise.all([
      this.masterdataService.getConfiguracion().toPromise(),
      this.masterdataService.getRangos().toPromise(),
    ]);

    this.configuration = config;
    this.ranges = ranges;
  }

  public async onSaveClick(): Promise<void> {
    try {
      await Promise.all([
        this.masterdataService.saveConfiguracion(this.configuration).toPromise(),
        this.masterdataService.saveRangos(this.ranges).toPromise(),
      ]);

      this.alertService.success(this.translate.instant('core.datos-guardados'));
    } catch (err) {
      this.alertService.error(err);
    }
  }
}
