import { Injectable } from '@angular/core';
import { ApiClient, RangoDto } from '../api/api.client';

@Injectable({
  providedIn: 'root',
})
export class RangeService {
  constructor(private apiClient: ApiClient) {}

  private ranges: RangoDto[];

  public async forceRefresh(): Promise<void> {
    this.ranges = await this.apiClient.getRangos().toPromise();
  }

  public getRangeByScore(score: number): RangoDto {
    if (!this.ranges || !this.ranges.length) {
      return;
    }

    return this.ranges.find((x) => x.minimo <= score && (!x.maximo || x.maximo >= score));
  }

  public getRangeDescriptionByScore(score: number): string {
    const range = this.getRangeByScore(score);
    return range ? range.descripcion : '';
  }
}
