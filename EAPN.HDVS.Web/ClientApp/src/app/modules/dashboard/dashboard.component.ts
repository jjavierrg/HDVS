import { Component, OnInit } from '@angular/core';
import { RangeService } from 'src/app/core/services/range.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(private rangeService: RangeService) { }

  async ngOnInit() {
    await this.rangeService.forceRefresh();
  }

}
