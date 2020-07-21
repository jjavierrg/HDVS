import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { LogEntryDto } from 'src/app/core/api/api.client';
import { AlertService } from 'src/app/core/services/alert.service';
import { LogService } from 'src/app/core/services/log.service';
import { DateCellComponent } from 'src/app/shared/modules/grid/date-cell/date-cell.component';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss'],
})
export class LogComponent implements OnInit {
  public entries: LogEntryDto[];
  public selectedEntry: LogEntryDto;
  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('core.fecha'),
      field: 'date',
      maxWidth: 180,
      filter: 'agDateColumnFilter',
      valueFormatter: this.logDateFormatter.bind(this),
      sort: 'desc',
    },
    {
      headerName: this.translate.instant('comun.usuario'),
      field: 'userName',
      minWidth: 100,
      maxWidth: 200,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.organizacion'),
      field: 'organizacionName',
      minWidth: 100,
      maxWidth: 200,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('logs.level'),
      field: 'level',
      maxWidth: 130,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.descripcion'),
      field: 'message',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    }
  ];

  constructor(
    private logService: LogService,
    private alertService: AlertService,
    private translate: TranslateService,
    private modalService: NgbModal,
    private datePipe: DatePipe,
  ) {}

  async ngOnInit() {
    try {
      this.entries = await this.logService.getLogs().toPromise();
    } catch (error) {
      this.alertService.error(error);
    }
  }

  public async onViewEntry(modal: any): Promise<void> {
    if (!this.selectedEntry) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static', scrollable: true, size: 'xl' }).result;
    } catch (error) {
      return;
    }
  }

  private logDateFormatter(params): string {
    const date = (params || {}).value;
    return this.datePipe.transform(date, 'medium');
  }
}
