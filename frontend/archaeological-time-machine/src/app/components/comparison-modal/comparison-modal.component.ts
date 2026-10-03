import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  inject,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  SiteDetail,
  SiteSummary,
  SiteComparisonResult
} from '../../models/archaeology.models';
import { ArchaeologyApiService } from '../../services/archaeology-api.service';

@Component({
  selector: 'app-comparison-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-backdrop" *ngIf="isOpen" (click)="close()">
      <div class="modal-card" (click)="$event.stopPropagation()">
        <!-- Header -->
        <header class="modal-header">
          <div>
            <span class="badge-compare">Analytical GIS Comparator</span>
            <h3 class="modal-title font-display">Side-by-Side Archaeological Comparison</h3>
          </div>
          <button class="close-btn" (click)="close()" title="Close Comparator">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </header>

        <!-- Selector for Site 2 -->
        <div class="selector-bar">
          <div class="selected-site-label">
            Primary: <strong>{{ site1?.name }}</strong>
          </div>
          <div class="site-picker">
            <label for="compare-site-select">Compare Against:</label>
            <select
              id="compare-site-select"
              class="site-select"
              [ngModel]="selectedSite2Id()"
              (ngModelChange)="onSite2Selected($event)"
            >
              <option *ngFor="let s of allSites" [value]="s.id" [disabled]="s.id === site1?.id">
                {{ s.name }} ({{ s.country }})
              </option>
            </select>
          </div>
        </div>

        <!-- Comparative Metrics Banner (Zoom Earth HUD Style) -->
        <div class="metrics-banner" *ngIf="comparison">
          <div class="metric-card">
            <span class="metric-label">Geodesic Distance</span>
            <span class="metric-val font-mono">{{ comparison.geodesicDistanceKm | number:'1.1-1' }} km</span>
            <span class="metric-sub">Ellipsoidal Spatial Arc</span>
          </div>

          <div class="metric-card" [class.highlight-overlap]="comparison.hasTemporalOverlap">
            <span class="metric-label">Chronological Overlap</span>
            <span class="metric-val font-mono">
              {{ comparison.hasTemporalOverlap ? comparison.temporalOverlapYears + ' Years' : 'None (Successive)' }}
            </span>
            <span class="metric-sub">{{ comparison.overlapSpanText }}</span>
          </div>

          <div class="metric-card">
            <span class="metric-label">Documented Inter-Site Relation</span>
            <span class="metric-val relation-text">
              {{ (comparison.directRelationships && comparison.directRelationships.length > 0) ? comparison.directRelationships[0].relationshipType : 'Independent Horizons' }}
            </span>
            <span class="metric-sub" *ngIf="comparison.directRelationships && comparison.directRelationships.length > 0">
              {{ comparison.directRelationships[0].description }}
            </span>
          </div>
        </div>

        <!-- Comparative Columns -->
        <div class="comparison-grid" *ngIf="comparison">
          <!-- Column 1 -->
          <div class="site-column">
            <div class="column-header">
              <h4 class="col-site-name font-display">{{ comparison.site1.name }}</h4>
              <div class="col-ancient" *ngIf="comparison.site1.ancientName"><em>{{ comparison.site1.ancientName }}</em></div>
              <div class="col-meta">{{ comparison.site1.region }}, <strong>{{ comparison.site1.country }}</strong></div>
              <div class="col-dates font-mono">⏳ {{ comparison.site1.startYearFormatted }} – {{ comparison.site1.endYearFormatted }}</div>
            </div>

            <div class="section-box">
              <h5>Site Type & Function</h5>
              <p>{{ comparison.site1.siteType }}</p>
            </div>

            <div class="section-box">
              <h5>Hydraulic Engineering & Water</h5>
              <p>{{ comparison.site1.waterSource || 'N/A' }}</p>
            </div>

            <div class="section-box">
              <h5>Architectural Highlights</h5>
              <p>{{ comparison.site1.architecturalHighlights || 'N/A' }}</p>
            </div>

            <div class="section-box">
              <h5>Excavation & Legal Status</h5>
              <p>{{ comparison.site1.excavationStatus || 'N/A' }}</p>
            </div>
          </div>

          <!-- Column 2 -->
          <div class="site-column">
            <div class="column-header">
              <h4 class="col-site-name font-display">{{ comparison.site2.name }}</h4>
              <div class="col-ancient" *ngIf="comparison.site2.ancientName"><em>{{ comparison.site2.ancientName }}</em></div>
              <div class="col-meta">{{ comparison.site2.region }}, <strong>{{ comparison.site2.country }}</strong></div>
              <div class="col-dates font-mono">⏳ {{ comparison.site2.startYearFormatted }} – {{ comparison.site2.endYearFormatted }}</div>
            </div>

            <div class="section-box">
              <h5>Site Type & Function</h5>
              <p>{{ comparison.site2.siteType }}</p>
            </div>

            <div class="section-box">
              <h5>Hydraulic Engineering & Water</h5>
              <p>{{ comparison.site2.waterSource || 'N/A' }}</p>
            </div>

            <div class="section-box">
              <h5>Architectural Highlights</h5>
              <p>{{ comparison.site2.architecturalHighlights || 'N/A' }}</p>
            </div>

            <div class="section-box">
              <h5>Excavation & Legal Status</h5>
              <p>{{ comparison.site2.excavationStatus || 'N/A' }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-backdrop {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.45);
      backdrop-filter: blur(8px);
      z-index: 1100;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 24px;
      animation: fadeIn 0.2s ease;
    }

    .modal-card {
      background: #ffffff;
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 16px;
      width: 920px;
      max-width: 95vw;
      max-height: 90vh;
      display: flex;
      flex-direction: column;
      overflow: hidden;
      box-shadow: 0 24px 60px rgba(0, 0, 0, 0.18);
    }

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 20px 26px;
      background: #fafafa;
      border-bottom: 1px solid rgba(0, 0, 0, 0.08);
    }

    .badge-compare {
      font-size: 10.5px;
      font-weight: 700;
      color: #92400e;
      letter-spacing: 0.06em;
      text-transform: uppercase;
      background: #fef8e7;
      border: 1px solid #fef3c7;
      padding: 3px 8px;
      border-radius: 6px;
    }

    .modal-title {
      font-size: 20px;
      font-weight: 700;
      color: #111827;
      margin-top: 4px;
    }

    .close-btn {
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      color: #4b5563;
      width: 32px;
      height: 32px;
      border-radius: 50%;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: all 0.2s ease;
    }

    .close-btn:hover {
      background: #fee2e2;
      color: #b91c1c;
      border-color: #ef4444;
    }

    .selector-bar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px 26px;
      background: #f8fafc;
      border-bottom: 1px solid rgba(0, 0, 0, 0.06);
    }

    .selected-site-label {
      font-size: 13.5px;
      color: #374151;
    }

    .selected-site-label strong {
      color: var(--accent-terracotta);
    }

    .site-picker {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 12.5px;
      color: #4b5563;
    }

    .site-select {
      background: #ffffff;
      border: 1px solid #d1d5db;
      color: #111827;
      font-size: 13px;
      font-family: inherit;
      padding: 6px 12px;
      border-radius: 8px;
      outline: none;
    }

    .metrics-banner {
      display: grid;
      grid-template-columns: 1fr 1fr 1.5fr;
      gap: 12px;
      padding: 14px 26px;
      background: #ffffff;
      border-bottom: 1px solid rgba(0, 0, 0, 0.06);
    }

    .metric-card {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 12px 14px;
      display: flex;
      flex-direction: column;
    }

    .metric-card.highlight-overlap {
      background: #ecfdf5;
      border-color: #a7f3d0;
    }

    .metric-label {
      font-size: 10px;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      color: #6b7280;
      margin-bottom: 2px;
      font-weight: 700;
    }

    .metric-val {
      font-size: 18px;
      font-weight: 700;
      color: #111827;
    }

    .relation-text {
      font-size: 13.5px;
      color: var(--accent-terracotta);
      font-family: inherit;
    }

    .metric-sub {
      font-size: 11px;
      color: #6b7280;
      margin-top: 2px;
      line-height: 1.3;
    }

    .comparison-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 18px;
      padding: 22px 26px;
      overflow-y: auto;
      flex: 1;
      background: #ffffff;
    }

    .site-column {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .column-header {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 16px;
    }

    .col-site-name {
      font-size: 17px;
      font-weight: 700;
      color: #111827;
      margin-bottom: 2px;
    }

    .col-ancient {
      font-size: 12px;
      color: var(--accent-terracotta);
      margin-bottom: 4px;
    }

    .col-meta {
      font-size: 12px;
      color: #6b7280;
      margin-bottom: 6px;
    }

    .col-dates {
      font-size: 12px;
      color: #92400e;
      background: #fef8e7;
      display: inline-block;
      padding: 2px 6px;
      border-radius: 4px;
    }

    .section-box {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 14px;
    }

    .section-box h5 {
      font-size: 11.5px;
      text-transform: uppercase;
      letter-spacing: 0.04em;
      color: #6b7280;
      margin-bottom: 4px;
      font-weight: 700;
    }

    .section-box p {
      font-size: 13px;
      color: #374151;
      line-height: 1.5;
    }
  `]
})
export class ComparisonModalComponent implements OnChanges {
  @Input() site1: SiteDetail | null = null;
  @Input() allSites: SiteSummary[] = [];
  @Input() isOpen = false;
  @Output() closeModal = new EventEmitter<void>();

  private readonly api = inject(ArchaeologyApiService);

  protected readonly selectedSite2Id = signal<number | null>(null);
  protected comparison: SiteComparisonResult | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && this.isOpen && this.site1 && this.allSites.length > 0) {
      const other = this.allSites.find(s => s.id !== this.site1!.id);
      if (other) {
        this.selectedSite2Id.set(other.id);
        this.runComparison(this.site1.id, other.id);
      }
    }
  }

  onSite2Selected(site2Id: any): void {
    const id = Number(site2Id);
    this.selectedSite2Id.set(id);
    if (this.site1) {
      this.runComparison(this.site1.id, id);
    }
  }

  close(): void {
    this.closeModal.emit();
  }

  private runComparison(id1: number, id2: number): void {
    this.api.compareSites(id1, id2).subscribe({
      next: (res) => this.comparison = res,
      error: (err) => console.error('Failed to run site comparison', err)
    });
  }
}
