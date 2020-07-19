import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient, LogEntryDto } from '../api/api.client';

@Injectable({
  providedIn: 'root',
})
export class LogService {
  constructor(private apiClient: ApiClient) {}

  public getLogs(): Observable<LogEntryDto[]> {
    return this.apiClient.getLogs();
  }
}
