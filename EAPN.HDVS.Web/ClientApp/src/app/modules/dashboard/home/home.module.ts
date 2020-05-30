import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { IndicatorModule } from 'src/app/shared/modules/indicator/indicator.module';
import { AttachmentUploaderModule } from 'src/app/shared/modules/attachment-uploader/attachment-uploader.module';
import { CoreModule } from 'src/app/core/core.module';
import { ImageModule } from 'src/app/shared/modules/image/image.module';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
];

@NgModule({
  declarations: [HomeComponent],
  imports: [CommonModule, RouterModule.forChild(routes), IndicatorModule, AttachmentUploaderModule, CoreModule, ImageModule],
})
export class HomeModule {}
