import {
  Component,
  Input,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Artefact } from '../../models/archaeology.models';

@Component({
  selector: 'app-artefact-viewer3d',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="artefact-viewer-container">
      <!-- HEADER & PROVENANCE METADATA -->
      <div class="viewer-header">
        <div class="header-info">
          <div class="badge-row">
            <span class="badge-mode font-mono">
              📷 Authentic Archival Photography (Museum Specimen)
            </span>
          </div>
          <h4 class="artefact-title font-display">{{ artefact?.name || 'Diagnostic Archaeological Find' }}</h4>
          <p class="artefact-meta" *ngIf="artefact">
            <span class="meta-tag material-tag">🏺 {{ artefact.material }}</span>
            <span class="meta-tag date-tag" *ngIf="artefact.approximateYearFormatted">⏳ {{ artefact.approximateYearFormatted }}</span>
            <span class="meta-tag loc-tag">🏛️ {{ artefact.currentLocation }}</span>
          </p>
        </div>

        <div class="viewer-controls" *ngIf="artefact?.imageUrl">
          <!-- Zoom Controls -->
          <div class="zoom-button-group">
            <button
              class="zoom-btn"
              (click)="zoomIn()"
              [disabled]="zoomLevel() >= 2.5"
              title="Zoom In"
            >
              ＋
            </button>
            <button
              class="zoom-btn reset-btn font-mono"
              (click)="resetZoom()"
              title="Reset Zoom"
            >
              {{ Math.round(zoomLevel() * 100) }}%
            </button>
            <button
              class="zoom-btn"
              (click)="zoomOut()"
              [disabled]="zoomLevel() <= 1"
              title="Zoom Out"
            >
              －
            </button>
          </div>
        </div>
      </div>

      <!-- AUTHENTIC PHOTOGRAPHY STAGE -->
      <div class="photo-viewer-stage">
        <div
          class="photo-container"
          [class.zoomed]="zoomLevel() > 1"
          (click)="toggleZoom()"
        >
          <img
            *ngIf="artefact?.imageUrl"
            [src]="artefact!.imageUrl"
            [alt]="artefact!.name"
            class="authentic-photo-img"
            [style.transform]="'scale(' + zoomLevel() + ')'"
            loading="lazy"
            (error)="onPhotoError($event)"
          />
          <div *ngIf="!artefact?.imageUrl" class="no-photo-box">
            <div class="no-photo-icon">🏺</div>
            <p class="no-photo-text">Authentic museum photograph being catalogued from archaeological archive.</p>
          </div>
        </div>
        <div class="photo-zoom-hint" *ngIf="artefact?.imageUrl">
          🔍 {{ zoomLevel() > 1 ? 'Click image to reset • Pan with mouse' : 'Click image or use controls to inspect authentic surface craftsmanship & inscriptions' }}
        </div>
      </div>

      <!-- ARTEFACT STRATIGRAPHIC DISCOVERY CONTEXT & CURATORIAL DETAILS -->
      <div class="artefact-details-footer" *ngIf="artefact">
        <div class="detail-block" *ngIf="artefact.dimensions">
          <span class="detail-label font-mono">SPECIMEN DIMENSIONS</span>
          <span class="detail-value">{{ artefact.dimensions }}</span>
        </div>
        <div class="detail-block" *ngIf="artefact.artefactType">
          <span class="detail-label font-mono">ARCHAEOLOGICAL TYPOLOGY</span>
          <span class="detail-value">{{ artefact.artefactType }}</span>
        </div>
        <div class="detail-block full-width" *ngIf="artefact.discoveryContext">
          <span class="detail-label font-mono">EXCAVATION & DISCOVERY CONTEXT</span>
          <span class="detail-value highlight-value">📍 {{ artefact.discoveryContext }}</span>
        </div>
        <div class="detail-block full-width" *ngIf="artefact.description">
          <span class="detail-label font-mono">CURATORIAL & MATERIAL SIGNIFICANCE</span>
          <p class="detail-desc">{{ artefact.description }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .artefact-viewer-container {
      display: flex;
      flex-direction: column;
      background: #ffffff;
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 16px;
      overflow: hidden;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
      position: relative;
    }

    .viewer-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 18px 24px;
      background: #ffffff;
      border-bottom: 1px solid rgba(0, 0, 0, 0.06);
      gap: 16px;
      flex-wrap: wrap;
    }

    .header-info {
      flex: 1;
      min-width: 260px;
    }

    .badge-row {
      margin-bottom: 6px;
    }

    .badge-mode {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      font-size: 11px;
      letter-spacing: 0.05em;
      text-transform: uppercase;
      font-weight: 700;
      color: #047857;
      background: #ecfdf5;
      padding: 3px 10px;
      border-radius: 20px;
      border: 1px solid #a7f3d0;
    }

    .artefact-title {
      font-size: 20px;
      font-weight: 700;
      color: #0f172a;
      margin: 0 0 6px 0;
      line-height: 1.25;
    }

    .artefact-meta {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
      margin: 0;
      font-size: 12px;
      color: #64748b;
    }

    .meta-tag {
      background: #f1f5f9;
      padding: 3px 10px;
      border-radius: 6px;
      border: 1px solid #e2e8f0;
      font-weight: 500;
    }

    .material-tag {
      background: #fef3c7;
      border-color: #fde68a;
      color: #92400e;
    }

    .date-tag {
      background: #ede9fe;
      border-color: #ddd6fe;
      color: #5b21b6;
    }

    .loc-tag {
      background: #e0f2fe;
      border-color: #bae6fd;
      color: #0369a1;
    }

    .viewer-controls {
      display: flex;
      align-items: center;
      gap: 10px;
    }

    .zoom-button-group {
      display: inline-flex;
      align-items: center;
      background: #f1f5f9;
      border: 1px solid #cbd5e1;
      border-radius: 8px;
      overflow: hidden;
    }

    .zoom-btn {
      background: transparent;
      border: none;
      padding: 6px 12px;
      font-size: 14px;
      font-weight: 700;
      color: #1e293b;
      cursor: pointer;
      transition: background 0.15s ease;
    }

    .zoom-btn:hover:not(:disabled) {
      background: #e2e8f0;
      color: var(--accent-terracotta, #c25e2e);
    }

    .zoom-btn:disabled {
      opacity: 0.35;
      cursor: not-allowed;
    }

    .zoom-btn.reset-btn {
      font-size: 11px;
      padding: 6px 8px;
      border-left: 1px solid #cbd5e1;
      border-right: 1px solid #cbd5e1;
    }

    /* Photographic Stage */
    .photo-viewer-stage {
      position: relative;
      width: 100%;
      height: 440px;
      background: radial-gradient(circle at 50% 40%, #ffffff 0%, #f8fafc 60%, #e2e8f0 100%);
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      overflow: hidden;
      cursor: zoom-in;
    }

    .photo-container {
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      overflow: hidden;
      padding: 24px;
      position: relative;
    }

    .photo-container.zoomed {
      cursor: zoom-out;
    }

    .authentic-photo-img {
      max-width: 100%;
      max-height: 100%;
      object-fit: contain;
      border-radius: 8px;
      box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
      transition: transform 0.25s cubic-bezier(0.2, 0, 0.2, 1);
      user-select: none;
    }

    .photo-zoom-hint {
      position: absolute;
      bottom: 12px;
      left: 50%;
      transform: translateX(-50%);
      background: rgba(15, 23, 42, 0.82);
      color: #f8fafc;
      font-size: 11.5px;
      padding: 5px 14px;
      border-radius: 20px;
      backdrop-filter: blur(8px);
      pointer-events: none;
      white-space: nowrap;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }

    .no-photo-box {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 30px;
      text-align: center;
      color: #94a3b8;
    }

    .no-photo-icon {
      font-size: 48px;
      margin-bottom: 10px;
    }

    /* Curatorial Details Footer */
    .artefact-details-footer {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 14px;
      padding: 20px 24px;
      background: #fbfbfa;
      border-top: 1px solid rgba(0, 0, 0, 0.06);
    }

    .detail-block {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .detail-block.full-width {
      grid-column: 1 / -1;
    }

    .detail-label {
      font-size: 10.5px;
      letter-spacing: 0.08em;
      color: #64748b;
      font-weight: 700;
    }

    .detail-value {
      font-size: 13px;
      color: #1e293b;
      font-weight: 600;
    }

    .highlight-value {
      color: var(--accent-terracotta, #c25e2e);
    }

    .detail-desc {
      margin: 0;
      font-size: 13px;
      line-height: 1.6;
      color: #475569;
    }

    @media (max-width: 640px) {
      .artefact-details-footer {
        grid-template-columns: 1fr;
      }
      .photo-viewer-stage {
        height: 320px;
      }
    }
  `]
})
export class ArtefactViewer3DComponent {
  @Input() artefact: Artefact | null = null;

  protected readonly zoomLevel = signal<number>(1);
  protected readonly Math = Math;

  zoomIn(): void {
    this.zoomLevel.update(z => Math.min(2.5, Math.round((z + 0.35) * 100) / 100));
  }

  zoomOut(): void {
    this.zoomLevel.update(z => Math.max(1, Math.round((z - 0.35) * 100) / 100));
  }

  resetZoom(): void {
    this.zoomLevel.set(1);
  }

  toggleZoom(): void {
    if (this.zoomLevel() > 1) {
      this.resetZoom();
    } else {
      this.zoomLevel.set(1.7);
    }
  }

  onPhotoError(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (img && !img.dataset['fallback']) {
      img.dataset['fallback'] = 'true';
      img.src = '/images/fallback.jpg';
    }
  }
}
