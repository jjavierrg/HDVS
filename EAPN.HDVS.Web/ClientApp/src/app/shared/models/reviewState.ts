import { SeguimientoDto } from 'src/app/core/api/api.client';

export interface IReviewState {
  review?: SeguimientoDto;
  returnUrl?: string;
  readonly?: boolean;
}
