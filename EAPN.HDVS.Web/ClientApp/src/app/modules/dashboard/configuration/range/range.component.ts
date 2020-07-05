import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, OnChanges } from '@angular/core';
import { RangoDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-range',
  templateUrl: './range.component.html',
  styleUrls: ['./range.component.scss'],
})
export class RangeComponent {
  @Input() public ranges: RangoDto[];
  @Output() public rangesChange = new EventEmitter<RangoDto[]>();

  constructor() {}

  public onDeleteRangeClick(range: RangoDto): void {
    this.ranges = this.ranges.filter((x) => x !== range);
    this.rangesChange.emit(this.ranges);
  }

  public onAddRangeClick(): void {
    this.ranges.push(new RangoDto());
    this.rangesChange.emit(this.ranges);
  }
}
