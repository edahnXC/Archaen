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
            <h3 class="modal-title">Side-by-Side Archaeological Comparison</h3>
          </div>
          <button class="close-btn" (click)="close()" title="Close Comparator">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </header>

        <!-- Selector for Site 2 if needed -->
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

        <!-- Comparative Metrics Banner -->
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
              {{ comparison.directRelationships.length > 0 ? comparison.directRelationships[0].relationshipType : 'Independent Horizons' }}
            </span>
            <span class="metric-sub" *ngIf="comparison.directRelationships.length > 0">
              {{ comparison.directRelationships[0].description }}
            </span>
          </div>
        </div>

        <!-- Comparative Columns -->
        <div class="comparison-grid" *ngIf="comparison">
          <!-- Column 1 -->
          <div class="site-column">
            <div class="column-header">
              <h4 class="col-site-name">{{ comparison.site1.name }}</h4>
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
              <h4 class="col-site-name">{{ comparison.site2.name }}</h4>
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
      background: rgba(0, 0, 0, 0.75);
      backdrop-filter: blur(8px);
      z-index: 1100;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 24px;
      animation: fadeIn 0.2s ease;
    }

    .modal-card {
      background: #12151f;
      border: 1px solid rgba(212, 175, 55, 0.4);
      border-radius: 14px;
      width: 900px;
      max-width: 95vw;
      max-height: 90vh;
      display: flex;
      flex-direction: column;
      overflow: hidden;
      box-shadow: 0 20px 60px rgba(0, 0, 0, 0.85);
    }

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 18px 24px;
      background: linear-gradient(180deg, #1b202e 0%, #12151f 100%);
      border-bottom: 1px solid rgba(255, 255, 255, 0.08);
    }

    .badge-compare {
      font-size: 10px;
      font-weight: 700;
      color: #e9c46a;
      letter-spacing: 0.08em;
      text-transform: uppercase;
      background: rgba(233, 196, 106, 0.15);
      border: 1px solid rgba(233, 196, 106, 0.3);
      padding: 2px 7px;
      border-radius: 4px;
    }

    .modal-title {
      font-family: var(--font-display, serif);
      font-size: 19px;
      color: #f8fafc;
      margin-top: 4px;
    }

    .close-btn {
      background: rgba(255, 255, 255, 0.06);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: #94a3b8;
      width: 32px;
      height: 32px;
      border-radius: 50%;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .close-btn:hover {
      background: rgba(217, 4, 41, 0.25);
      color: #ff4d6d;
    }

    .selector-bar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px 24px;
      background: rgba(20, 24, 34, 0.7);
      border-bottom: 1px solid rgba(255, 255, 255, 0.06);
    }

    .selected-site-label {
      font-size: 13.5px;
      color: #cbd5e1;
    }

    .selected-site-label strong {
      color: #ffd166;
    }

    .site-picker {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 12.5px;
      color: #94a3b8;
    }

    .site-select {
      background: #0d0f16;
      border: 1px solid rgba(212, 175, 55, 0.35);
      color: #f8fafc;
      font-size: 13px;
      padding: 6px 12px;
      border-radius: 6px;
      outline: none;
    }

    .metrics-banner {
      display: grid;
      grid-template-columns: 1fr 1fr 1.5fr;
      gap: 12px;
      padding: 14px 24px;
      background: rgba(10, 12, 18, 0.7);
      border-bottom: 1px solid rgba(255, 255, 255, 0.06);
    }

    .metric-card {
      background: rgba(255, 255, 255, 0.03);
      border: 1px solid rgba(255, 255, 255, 0.06);
      border-radius: 8px;
      padding: 10px 14px;
      display: flex;
      flex-direction: column;
    }

    .metric-card.highlight-overlap {
      background: rgba(42, 157, 143, 0.12);
      border-color: rgba(42, 157, 143, 0.3);
    }

    .metric-label {
      font-size: 10px;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      color: #94a3b8;
      margin-bottom: 2px;
    }

    .metric-val {
      font-size: 17px;
      font-weight: 800;
      color: #ffd166;
    }

    .relation-text {
      font-size: 13.5px;
      color: #f4a261;
      font-family: inherit;
    }

    .metric-sub {
      font-size: 11px;
      color: #94a3b8;
      margin-top: 2px;
      line-height: 1.3;
    }

    .comparison-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 18px;
      padding: 20px 24px;
      overflow-y: auto;
      flex: 1;
    }

    .site-column {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .column-header {
      background: rgba(255, 255, 255, 0.04);
      border: 1px solid rgba(255, 255, 255, 0.08);
      border-radius: 8px;
      padding: 14px;
    }

    .col-site-name {
      font-family: var(--font-display, serif);
      font-size: 16px;
      color: #f8fafc;
      margin-bottom: 2px;
    }

    .col-ancient {
      font-size: 12px;
      color: #e9c46a;
      margin-bottom: 4px;
    }

    .col-meta {
      font-size: 12px;
      color: #94a3b8;
      margin-bottom: 6px;
    }

    .col-dates {
      font-size: 12px;
      color: #ffd166;
    }

    .section-box {
      background: rgba(255, 255, 255, 0.02);
      border: 1px solid rgba(255, 255, 255, 0.05);
      border-radius: 8px;
      padding: 12px;
    }

    .section-box h5 {
      font-size: 11.5px;
      text-transform: uppercase;
      letter-spacing: 0.04em;
      color: #94a3b8;
      margin-bottom: 4px;
    }

    .section-box p {
      font-size: 12.5px;
      color: #cbd5e1;
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
      // Pick a default site2 (e.g. Lothal if site1 is Dholavira, or first other site)
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
