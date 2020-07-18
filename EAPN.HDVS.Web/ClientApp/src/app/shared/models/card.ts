import { FichaDto, IFichaDto } from 'src/app/core/api/api.client';

export class Card extends FichaDto {
  constructor(data?: IFichaDto) {
    super(data);
  }

  public get edadCalculada(): number {
    if (!this.fechaNacimiento) {
      return;
    }

    try {
      const now: Date = new Date();
      const age: number = now.getFullYear() - this.fechaNacimiento.getFullYear();
      if (now.getMonth() > this.fechaNacimiento.getMonth()) {
        return age;
      } else if (now.getMonth() < this.fechaNacimiento.getMonth()) {
        return age - 1;
      } else if (now.getDate() < this.fechaNacimiento.getDate()) {
        return age - 1;
      } else {
        return age;
      }
    } catch (error) {
      return;
    }
  }
}
