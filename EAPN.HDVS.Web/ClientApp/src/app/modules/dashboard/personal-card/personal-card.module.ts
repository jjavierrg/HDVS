import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardFinderComponent } from './card-finder/card-finder.component';
import { CoreModule } from 'src/app/core/core.module';
import { InputModule } from 'src/app/shared/modules/input/input.module';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedGuard } from 'src/app/core/guards/authenticate.guard';
import { FormsModule } from '@angular/forms';
import { NgbModalModule, NgbNavModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { GridModule } from 'src/app/shared/modules/grid/grid.module';
import { CardFormComponent } from './card-form/card-form.component';
import { Permissions } from 'src/app/core/enums/permissions.enum';
import { ImageModule } from 'src/app/shared/modules/image/image.module';
import { CardPersonalDataComponent } from './card-personal-data/card-personal-data.component';
import { CardAttachmentsComponent } from './card-attachments/card-attachments.component';
import { AttachmentUploaderModule } from 'src/app/shared/modules/attachment-uploader/attachment-uploader.module';
import { CardReviewComponent } from './card-review/card-review.component';
import { IndicatorModule } from 'src/app/shared/modules/indicator/indicator.module';
import { UiSwitchModule } from 'ngx-ui-switch';

const routes: Routes = [
  {
    path: 'alta',
    component: CardFinderComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.write] },
  },
  {
    path: '',
    component: CardFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.write] },
  },
  {
    path: ':id',
    component: CardFormComponent,
    canLoad: [AuthenticatedGuard],
    canActivate: [AuthenticatedGuard],
    data: { allowedPermissions: [Permissions.personalcard.read] },
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'alta',
  },
];

@NgModule({
  declarations: [CardFinderComponent, CardFormComponent, CardPersonalDataComponent, CardAttachmentsComponent, CardReviewComponent],
  imports: [
    CommonModule,
    CoreModule,
    InputModule,
    FormsModule,
    NgbModalModule,
    GridModule,
    RouterModule.forChild(routes),
    ImageModule,
    NgbNavModule,
    AttachmentUploaderModule,
    IndicatorModule,
    NgbDropdownModule,
    UiSwitchModule,
  ],
})
export class PersonalCardModule {}
