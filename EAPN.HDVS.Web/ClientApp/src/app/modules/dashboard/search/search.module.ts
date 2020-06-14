import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchResultComponent } from './search-result/search-result.component';
import { AdvancedSearchComponent } from './advanced-search/advanced-search.component';
import { Routes, RouterModule } from '@angular/router';
import { SearchResultEntryComponent } from './search-result-entry/search-result-entry.component';
import { CoreModule } from 'src/app/core/core.module';
import { FormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/modules/input/input.module';

const routes: Routes = [
  {
    path: '',
    component: AdvancedSearchComponent,
  }
];

@NgModule({
  declarations: [SearchResultComponent, AdvancedSearchComponent, SearchResultEntryComponent],
  imports: [CommonModule, CoreModule, FormsModule, InputModule, RouterModule.forChild(routes)],
})
export class SearchModule {}
