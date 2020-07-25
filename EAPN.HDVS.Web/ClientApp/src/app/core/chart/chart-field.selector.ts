import { IMasterDataDto } from '../api/api.client';

interface IChartFieldSelectorOptions<T> {
  name: string;
  valueProperty?: keyof T;
  descriptorProperty?: keyof T;
  emptyDescriptionMessage?: string;
}

export class ChartFieldSelector<T> implements IMasterDataDto {
  public readonly data: IChartFieldSelectorOptions<T>;
  public readonly id: number;
  public readonly descripcion: string;

  public getValue(object: T): T[keyof T] {
    return this.data.valueProperty ? object[this.data.valueProperty] : null;
  }

  public getDescription(object: T): string {
    return this.data.descriptorProperty ? (object[this.data.descriptorProperty] || this.data.emptyDescriptionMessage).toString() : '';
  }

  constructor(id: number, options: IChartFieldSelectorOptions<T>) {
    this.data = options;
    this.id = id;
    this.descripcion = this.data.name;
  }
}
