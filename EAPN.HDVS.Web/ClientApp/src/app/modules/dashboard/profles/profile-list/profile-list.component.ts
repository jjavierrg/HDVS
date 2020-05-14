import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { AgGridColumn } from 'ag-grid-angular';
import { PerfilDto } from 'src/app/core/api/api.client';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { AlertService } from 'src/app/core/services/alert.service';
import { ProfileService } from 'src/app/core/services/profile.service';

@Component({
  selector: 'app-profile-list',
  templateUrl: './profile-list.component.html',
  styleUrls: ['./profile-list.component.scss'],
})
export class ProfileListComponent implements OnInit {
  public profiles: PerfilDto[];
  public permissions = Permissions;

  public columns: Partial<AgGridColumn>[] = [
    {
      headerName: this.translate.instant('comun.descripcion'),
      field: 'descripcion',
      minWidth: 100,
      filter: 'agTextColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.permisos'),
      field: 'permisos.length',
      maxWidth: 150,
      filter: 'agNumberColumnFilter',
    },
    {
      headerName: this.translate.instant('comun.numero-usuarios'),
      field: 'numeroUsuarios',
      maxWidth: 150,
      filter: 'agNumberColumnFilter',
    },
  ];

  constructor(
    private service: ProfileService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private alertService: AlertService,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.RefreshData();
  }

  public onNewProfile(): void {
    this.router.navigate(['nuevo'], { relativeTo: this.activatedRoute });
  }

  public onEditProfile(pefil: PerfilDto): void {
    if (!!pefil) {
      this.router.navigate([pefil.id], { relativeTo: this.activatedRoute });
    }
  }

  public async onDeleteProfiles(pefiles: PerfilDto[], modal: any): Promise<void> {
    if (!pefiles || !pefiles.length) {
      return;
    }

    try {
      await this.modalService.open(modal, { centered: true, backdrop: 'static' }).result;
    } catch (error) {
      return;
    }

    try {
      await this.service.deletePerfiles(pefiles);
      this.RefreshData();
      this.alertService.success(this.translate.instant('core.elementos-eliminados'));
    } catch (error) {
      this.alertService.error(error);
    }
  }

  private RefreshData(): void {
    this.service.getPerfiles().subscribe((profiles) => (this.profiles = profiles));
  }
}
