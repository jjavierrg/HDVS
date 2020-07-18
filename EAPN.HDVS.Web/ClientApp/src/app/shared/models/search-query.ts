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
  toJSON?(): string;
}

export class SearchQuery implements ISearchQuery {
  constructor(data?: ISearchQuery) {
    if (data) {
      Object.assign(this, data);
    }
  }

  public idnumber?: string;
  public name?: string;
  public surname1?: string;
  public surname2?: string;
  public rad?: string;
  public birth?: Date;
  public partnerId?: number;
  public hideSearchForm?: boolean;

  static fromJSON(json: string): SearchQuery {
    const data = JSON.parse(json);
    const result = new SearchQuery();
    result.init(data);
    return result;
  }

  public isEmpty(): boolean {
    return !this.idnumber && !this.name && !this.surname1 && !this.surname2 && !this.rad && !this.birth;
  }

  public toJSON(): string {
    const data = {};
    data['idnumber'] = this.idnumber;
    data['name'] = this.name;
    data['surname1'] = this.surname1;
    data['surname2'] = this.surname2;
    data['rad'] = this.rad;
    data['birth'] = this.birth ? this.birth.toISOString() : <any>undefined;

    return JSON.stringify(data);
  }

  private init(_data?: any) {
    if (_data) {
      this.idnumber = _data['idnumber'];
      this.name = _data['name'];
      this.surname1 = _data['surname1'];
      this.surname2 = _data['surname2'];
      this.rad = _data['rad'];
      this.birth = _data['birth'] ? new Date(_data['birth'].toString()) : <any>undefined;
    }
  }
}
