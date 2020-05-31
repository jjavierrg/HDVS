import { FichaDto, IFichaDto } from 'src/app/core/api/api.client';

export class Card extends FichaDto {
  constructor(data?: IFichaDto) {
    super(data);
  }

  public get rad(): string {
    return '';
  }

  public get edadCalculada(): number {
    if (!this.fechaNacimiento) {
      return;
    }

    try {
      const now: Date = new Date();
      const minutesOffset: number = now.getTimezoneOffset();
      const diff: number = now.getUTCMilliseconds() - this.fechaNacimiento.getTime() + (minutesOffset * 60 * 1000);
      const age: Date = new Date(diff);
      return Math.abs(age.getUTCFullYear() - 1970);
    } catch (error) {
      return;
    }
  }
}
