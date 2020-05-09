export enum AlertType {
  Success = 'success',
  Error = 'danger',
  Info = 'info',
  Warning = 'warning',
}

export class Alert {
  type: AlertType;
  message: string;
  displayTime?: number;
}
