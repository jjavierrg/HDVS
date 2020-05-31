import { Injectable } from '@angular/core';
import { ApiClient, AdjuntoDto } from '../api/api.client';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AttachmentService {
  constructor(private apiClient: ApiClient) {}

  public async deleteAdjuntos(adjuntos: AdjuntoDto[]): Promise<boolean> {
    if (!adjuntos || !adjuntos.length) {
      return;
    }

    for (const adjunto of adjuntos) {
      await this.apiClient.deleteAdjunto(adjunto.id).toPromise();
    }

    return true;
  }

  public async downloadAdjunto(adjuntoId: number): Promise<void> {
    const response = await this.apiClient.getFile(adjuntoId).toPromise();
    const url = window.URL.createObjectURL(response.data);
    window.open(url);
  }
}
