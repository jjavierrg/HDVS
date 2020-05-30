import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ApiClient } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-secure-image',
  templateUrl: './secure-image.component.html',
  styleUrls: ['./secure-image.component.scss'],
})
export class SecureImageComponent implements OnInit, OnChanges {
  @Input() public adjuntoId: number;
  @Input() public imageClasses: string;
  @Input() public fallbackImagePath: string;

  public dataUrl: Promise<SafeUrl>;
  public showPlaceholder: boolean = false;
  public showFallbackImage: boolean = false;

  constructor(private apiClient: ApiClient, private domSanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.dataUrl = this.loadImage(this.adjuntoId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes.adjuntoId.isFirstChange()) {
      this.dataUrl = this.loadImage(changes.adjuntoId.currentValue);
    }
  }

  private async loadImage(adjuntoId: number): Promise<SafeUrl> {
    if (adjuntoId <= 0) {
      this.showFallbackImage = true;
      return;
    }

      this.showFallbackImage = false;
      this.showPlaceholder = true;
    try {
      const file = await this.apiClient.displayAdjunto(adjuntoId).toPromise();
      return this.domSanitizer.bypassSecurityTrustUrl(URL.createObjectURL(file.data));
    } catch (error) {
      this.showFallbackImage = true;
    } finally {
      this.showPlaceholder = false;
    }
  }
}
