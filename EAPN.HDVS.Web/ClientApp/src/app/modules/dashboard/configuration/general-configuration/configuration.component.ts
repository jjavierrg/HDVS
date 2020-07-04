import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfiguracionDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { MasterdataService } from 'src/app/core/services/masterdata.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class ConfigurationComponent implements OnInit {
  public configuration: ConfiguracionDto;

  constructor(private masterdataService: MasterdataService, private alertService: AlertService, private translate: TranslateService) {}

  async ngOnInit(): Promise<void> {
    this.configuration = await this.masterdataService.getConfiguracion().toPromise();
  }

  public async onSaveClick(): Promise<void> {
    try {
      await this.masterdataService.saveConfiguracion(this.configuration).toPromise();
      this.alertService.success(this.translate.instant('core.datos-guardados'));
    } catch (err) {
      this.alertService.error(err);
    }
  }
}
