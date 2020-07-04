export interface ISearchQuery {
  idnumber?: string;
  name?: string;
  surname1?: string;
  surname2?: string;
  rad?: string;
  birth?: Date;
  partnerId?: number;
  hideSearchForm?: boolean;
  isEmpty?(): boolean;
}

export class SearchQuery implements ISearchQuery {
  public idnumber?: string;
  public name?: string;
  public surname1?: string;
  public surname2?: string;
  public rad?: string;
  public birth?: Date;
  public partnerId?: number;
  public hideSearchForm?: boolean;

  constructor(data?: ISearchQuery) {
    if (data) {
      Object.assign(this, data);
    }
  }
  public isEmpty(): boolean {
    return !this.idnumber && !this.name && !this.surname1 && !this.surname2 && !this.rad && !this.birth && !this.partnerId;
  }
}
