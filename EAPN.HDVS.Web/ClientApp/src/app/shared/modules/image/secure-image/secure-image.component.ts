import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ApiClient } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-secure-image',
  templateUrl: './secure-image.component.html',
  styleUrls: ['./secure-image.component.scss'],
})
export class SecureImageComponent implements OnInit {
  @Input() public adjuntoId: number;
  @Input() public imageClasses: string;

  public dataUrl: Promise<SafeUrl>;
  public showPlaceholder: boolean = false;

  constructor(private apiClient: ApiClient, private domSanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.dataUrl = this.loadImage(this.adjuntoId);
  }

  private async loadImage(adjuntoId: number): Promise<SafeUrl> {
    this.showPlaceholder = true;
    try {
      const file = await this.apiClient.displayAdjunto(adjuntoId).toPromise();
      return this.domSanitizer.bypassSecurityTrustUrl(URL.createObjectURL(file.data));
    } catch (error) {
    } finally {
      this.showPlaceholder = false;
    }
  }
}
