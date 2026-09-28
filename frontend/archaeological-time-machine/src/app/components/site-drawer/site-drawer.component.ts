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
import {
  SiteDetail,
  NearbySiteResult,
  SiteSummary,
  Artefact
} from '../../models/archaeology.models';
import { ArchaeologyApiService } from '../../services/archaeology-api.service';
import { ArtefactViewer3DComponent } from '../artefact-viewer3d/artefact-viewer3d.component';

@Component({
  selector: 'app-site-drawer',
  standalone: true,
  imports: [CommonModule, ArtefactViewer3DComponent],
  template: `
    <div class="drawer-backdrop" *ngIf="isOpen" (click)="close()"></div>

    <aside class="site-drawer" [class.open]="isOpen">
      <!-- Loading Skeleton -->
      <div *ngIf="isLoading" class="drawer-loading">
        <div class="loader-spinner"></div>
        <p>Loading Archaeological Strata & Artefacts...</p>
      </div>

      <!-- Main Drawer Content -->
      <div *ngIf="!isLoading && site" class="drawer-content">
        <!-- Drawer Header -->
        <header class="drawer-header">
          <div class="header-badges">
            <span class="badge-unesco" *ngIf="site.isUnescoWorldHeritage">★ UNESCO World Heritage</span>
            <span class="badge-country">{{ site.country }}</span>
            <span class="badge-period" *ngFor="let p of site.periods">{{ p.periodName }}</span>
          </div>

          <button class="close-btn" (click)="close()" title="Close Drawer">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>

          <h2 class="site-title font-display">{{ site.name }}</h2>
          <div class="site-ancient" *ngIf="site.ancientName">Ancient / Indigenous Designation: <em>{{ site.ancientName }}</em></div>
          
          <div class="site-meta-bar">
            <div class="meta-item">
              <span class="meta-icon">📍</span>
              <span>{{ site.region }}</span>
            </div>
            <div class="meta-item font-mono date-highlight">
              <span class="meta-icon">⏳</span>
              <span>{{ site.startYearFormatted }} – {{ site.endYearFormatted }}</span>
            </div>
            <div class="meta-item font-mono">
              <span class="meta-icon">🌐</span>
              <span>{{ site.latitude | number:'1.4-4' }}°, {{ site.longitude | number:'1.4-4' }}°</span>
            </div>
          </div>

          <div class="header-actions">
            <button class="action-btn compare-btn" (click)="onCompareClicked()">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M16 3h5v5M4 20L21 3M21 16v5h-5M15 15l6 6M4 4l5 5"/>
              </svg>
              Compare with Another Site
            </button>
          </div>
        </header>

        <!-- Navigation Tabs -->
        <nav class="drawer-tabs">
          <button
            class="tab-btn"
            [class.active]="activeTab() === 'overview'"
            (click)="setTab('overview')"
          >
            Overview
          </button>
          <button
            class="tab-btn"
            [class.active]="activeTab() === 'stratigraphy'"
            (click)="setTab('stratigraphy')"
          >
            Stratigraphy ({{ countLayers() }})
          </button>
          <button
            class="tab-btn"
            [class.active]="activeTab() === 'artefacts'"
            (click)="setTab('artefacts')"
          >
            3D Artefacts ({{ site.artefacts.length }})
          </button>
          <button
            class="tab-btn"
            [class.active]="activeTab() === 'spatial'"
            (click)="setTab('spatial')"
          >
            Nearby & Contemporaneous
          </button>
          <button
            class="tab-btn"
            [class.active]="activeTab() === 'references'"
            (click)="setTab('references')"
          >
            Bibliography ({{ site.references.length }})
          </button>
        </nav>

        <!-- Tab Body -->
        <div class="tab-body">
          <!-- 1. OVERVIEW TAB -->
          <div *ngIf="activeTab() === 'overview'" class="tab-pane">
            <div class="site-image-card" *ngIf="site.imageUrl">
              <img [src]="site.imageUrl" [alt]="site.name" loading="lazy" />
              <div class="image-caption">{{ site.name }} archaeological excavation horizon</div>
            </div>

            <div class="info-card">
              <h4 class="card-title font-display">Architectural & Settlement Highlights</h4>
              <p class="highlight-text">{{ site.architecturalHighlights || 'Documented masonry, fortifications and settlement structures.' }}</p>
            </div>

            <div class="info-card">
              <h4 class="card-title font-display">Archaeological Synopsis</h4>
              <p class="body-text">{{ site.description }}</p>
            </div>

            <div class="info-grid">
              <div class="info-card">
                <h5 class="sub-title">Hydraulic & Water Engineering</h5>
                <p class="body-text">{{ site.waterSource || 'Seasonal water collection channels.' }}</p>
              </div>
              <div class="info-card">
                <h5 class="sub-title">Excavation & Conservation Status</h5>
                <p class="body-text">{{ site.excavationStatus || 'Protected archaeological monument.' }}</p>
              </div>
              <div class="info-card" *ngIf="site.discoveryInformation">
                <h5 class="sub-title">Discovery History</h5>
                <p class="body-text">{{ site.discoveryInformation }}</p>
              </div>
              <div class="info-card" *ngIf="site.datingPrecision">
                <h5 class="sub-title">Scientific Chronology & Dating</h5>
                <p class="body-text font-mono">{{ site.datingPrecision }}</p>
              </div>
            </div>
          </div>

          <!-- 2. STRATIGRAPHY TAB -->
          <div *ngIf="activeTab() === 'stratigraphy'" class="tab-pane">
            <div class="stratigraphy-intro">
              Vertical geological cutaway of archaeological strata documented through scientific stratigraphic soundings.
            </div>

            <div *ngFor="let exc of site.excavations" class="excavation-block">
              <div class="excavation-header">
                <div>
                  <h4 class="excavation-name font-display">{{ exc.expeditionName }}</h4>
                  <div class="excavator-meta">
                    Director: <strong>{{ exc.leadArchaeologist }}</strong> • {{ exc.organization }} ({{ exc.startYear }}-{{ exc.endYear || 'Present' }})
                  </div>
                </div>
              </div>

              <!-- Stratigraphic Layers Sequence -->
              <div class="strata-sequence">
                <div *ngFor="let layer of exc.layers" class="stratum-card">
                  <div class="stratum-depth-indicator">
                    <span class="depth-val font-mono">{{ layer.depthMeters }}m</span>
                    <span class="depth-label">Depth</span>
                  </div>

                  <div class="stratum-info">
                    <div class="stratum-head">
                      <h5 class="stratum-name font-display">{{ layer.layerName }}</h5>
                      <span class="stratum-culture" *ngIf="layer.culturalAffiliation">{{ layer.culturalAffiliation }}</span>
                    </div>

                    <div class="stratum-dates font-mono" *ngIf="layer.estimatedStartYearFormatted">
                      Horizon: {{ layer.estimatedStartYearFormatted }} – {{ layer.estimatedEndYearFormatted }}
                    </div>

                    <p class="stratum-desc">{{ layer.description }}</p>
                    
                    <div class="stratum-soil" *ngIf="layer.soilComposition">
                      <em>Matrix:</em> {{ layer.soilComposition }}
                    </div>

                    <!-- Findings within this layer -->
                    <div class="layer-findings" *ngIf="layer.findings && layer.findings.length > 0">
                      <div class="findings-header">Diagnostic In Situ Finds:</div>
                      <div class="finding-pill" *ngFor="let f of layer.findings">
                        <span class="finding-type">[{{ f.findingType }}]</span>
                        <span class="finding-name">{{ f.name }}</span>
                        <span class="finding-year" *ngIf="f.yearFound">(Found {{ f.yearFound }})</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div *ngIf="!site.excavations || site.excavations.length === 0" class="empty-state">
              No detailed stratigraphic profiles currently cataloged for this site.
            </div>
          </div>

          <!-- 3. 3D ARTEFACTS TAB -->
          <div *ngIf="activeTab() === 'artefacts'" class="tab-pane">
            <div *ngIf="site.artefacts && site.artefacts.length > 0">
              <!-- Artefact Selection Chips -->
              <div class="artefact-selector">
                <button
                  *ngFor="let art of site.artefacts; let i = index"
                  class="art-chip"
                  [class.active]="selectedArtefact()?.id === art.id"
                  (click)="selectArtefact(art)"
                >
                  <span class="art-num font-mono">Find #{{ i + 1 }}</span>
                  <span class="art-name">{{ art.name }}</span>
                </button>
              </div>

              <!-- Interactive 3D WebGL Viewer -->
              <div class="artefact-3d-wrapper" *ngIf="selectedArtefact()">
                <app-artefact-viewer3d [artefact]="selectedArtefact()"></app-artefact-viewer3d>
              </div>

              <div class="artefact-details-card" *ngIf="selectedArtefact()">
                <h4 class="card-title font-display">{{ selectedArtefact()!.name }}</h4>
                <p class="body-text">{{ selectedArtefact()!.description }}</p>
                <div class="art-meta-grid">
                  <div><strong>Dimensions:</strong> {{ selectedArtefact()!.dimensions || 'N/A' }}</div>
                  <div><strong>Material:</strong> {{ selectedArtefact()!.material }}</div>
                  <div><strong>Museum Location:</strong> {{ selectedArtefact()!.currentLocation || 'On-site Museum' }}</div>
                </div>
              </div>
            </div>

            <div *ngIf="!site.artefacts || site.artefacts.length === 0" class="empty-state">
              No diagnostic 3D artefacts cataloged for this site.
            </div>
          </div>

          <!-- 4. SPATIAL & CONTEMPORANEOUS TAB -->
          <div *ngIf="activeTab() === 'spatial'" class="tab-pane">
            <!-- Geodesic Neighbours -->
            <div class="spatial-section">
              <h4 class="card-title font-display">Geodesic Proximity (Closest Sites)</h4>
              <p class="section-sub">Calculated via geodesic ellipsoidal spatial distance.</p>

              <div class="nearby-list">
                <div
                  *ngFor="let n of nearbySites"
                  class="nearby-card"
                  (click)="onNearbySiteSelected(n.nearbySiteId)"
                >
                  <div class="nearby-info">
                    <span class="nearby-name font-display">{{ n.nearbySiteName }}</span>
                    <span class="nearby-loc">{{ n.region }}, {{ n.country }}</span>
                    <span class="nearby-dates font-mono">{{ n.startYearFormatted }} – {{ n.endYearFormatted }}</span>
                  </div>
                  <div class="nearby-distance font-mono">
                    {{ n.distanceKm | number:'1.1-1' }} km
                  </div>
                </div>
              </div>
            </div>

            <!-- Contemporaneous Sites -->
            <div class="spatial-section">
              <h4 class="card-title font-display">Contemporaneous Settlements</h4>
              <p class="section-sub">Ancient settlements co-existing during overlapping occupational centuries.</p>

              <div class="contemp-grid">
                <div
                  *ngFor="let c of contemporaneousSites"
                  class="contemp-card"
                  (click)="onNearbySiteSelected(c.id)"
                >
                  <div class="contemp-name font-display">{{ c.name }}</div>
                  <div class="contemp-loc">{{ c.region }}, {{ c.country }}</div>
                  <div class="contemp-dates font-mono">{{ c.startYearFormatted }} – {{ c.endYearFormatted }}</div>
                </div>
              </div>
            </div>
          </div>

          <!-- 5. BIBLIOGRAPHY TAB -->
          <div *ngIf="activeTab() === 'references'" class="tab-pane">
            <h4 class="card-title font-display">Academic & Archaeological Citations</h4>
            <div class="citations-list">
              <div *ngFor="let ref of site.references" class="citation-card">
                <div class="citation-key font-mono">[{{ ref.citationKey }}]</div>
                <div class="citation-content">
                  <div class="citation-title font-display">{{ ref.title }}</div>
                  <div class="citation-authors">{{ ref.authors }} ({{ ref.publicationYear }})</div>
                  <div class="citation-publisher"><em>{{ ref.journalOrPublisher }}</em></div>
                  <div class="citation-pages" *ngIf="ref.specificPagesOrPlates">
                    Cited Sections: {{ ref.specificPagesOrPlates }}
                  </div>
                  <a *ngIf="ref.url" [href]="ref.url" target="_blank" rel="noopener" class="citation-link">
                    View Academic Resource ↗
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </aside>
  `,
  styles: [`
    .drawer-backdrop {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.4);
      backdrop-filter: blur(4px);
      z-index: 950;
      animation: fadeIn 0.2s ease;
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }

    .site-drawer {
      position: fixed;
      top: 0;
      right: 0;
      width: 580px;
      max-width: 90vw;
      height: 100vh;
      background: rgba(255, 255, 255, 0.98);
      border-left: 1px solid rgba(0, 0, 0, 0.12);
      backdrop-filter: blur(24px);
      box-shadow: -12px 0 45px rgba(0, 0, 0, 0.15);
      z-index: 1000;
      transform: translateX(100%);
      transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1);
      display: flex;
      flex-direction: column;
      overflow: hidden;
    }

    .site-drawer.open {
      transform: translateX(0);
    }

    .drawer-loading {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 100%;
      color: #6b7280;
      gap: 12px;
    }

    .loader-spinner {
      width: 40px;
      height: 40px;
      border: 3px solid rgba(0, 0, 0, 0.1);
      border-top-color: var(--accent-terracotta);
      border-radius: 50%;
      animation: spin 0.8s linear infinite;
    }

    @keyframes spin {
      to { transform: rotate(360deg); }
    }

    .drawer-content {
      display: flex;
      flex-direction: column;
      height: 100%;
      overflow: hidden;
    }

    .drawer-header {
      position: relative;
      padding: 24px 28px 16px;
      background: #ffffff;
      border-bottom: 1px solid rgba(0, 0, 0, 0.08);
      flex-shrink: 0;
    }

    .header-badges {
      display: flex;
      flex-wrap: wrap;
      gap: 6px;
      margin-bottom: 8px;
    }

    .close-btn {
      position: absolute;
      top: 20px;
      right: 20px;
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      color: #4b5563;
      width: 34px;
      height: 34px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .close-btn:hover {
      background: #fee2e2;
      border-color: #ef4444;
      color: #b91c1c;
    }

    .site-title {
      font-size: 24px;
      font-weight: 700;
      color: #111827;
      margin-bottom: 4px;
    }

    .site-ancient {
      font-size: 13px;
      color: var(--accent-terracotta);
      margin-bottom: 10px;
    }

    .site-meta-bar {
      display: flex;
      flex-wrap: wrap;
      gap: 16px;
      font-size: 12px;
      color: #4b5563;
      margin-bottom: 14px;
    }

    .meta-item {
      display: flex;
      align-items: center;
      gap: 5px;
    }

    .date-highlight {
      color: #92400e;
      background: #fef8e7;
      padding: 2px 7px;
      border-radius: 4px;
      border: 1px solid #fef3c7;
      font-weight: 600;
    }

    .header-actions {
      display: flex;
      gap: 8px;
    }

    .action-btn {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: #f8fafc;
      border: 1px solid #cbd5e1;
      color: #0f172a;
      font-size: 12px;
      font-weight: 600;
      padding: 7px 14px;
      border-radius: 8px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .action-btn:hover {
      background: #111827;
      border-color: #111827;
      color: #ffffff;
      transform: translateY(-1px);
    }

    /* Tabs Navigation */
    .drawer-tabs {
      display: flex;
      border-bottom: 1px solid rgba(0, 0, 0, 0.08);
      background: #f9fafb;
      overflow-x: auto;
      flex-shrink: 0;
    }

    .tab-btn {
      padding: 12px 16px;
      background: none;
      border: none;
      border-bottom: 2px solid transparent;
      color: #6b7280;
      font-size: 12.5px;
      font-weight: 600;
      cursor: pointer;
      white-space: nowrap;
      transition: all 0.2s ease;
    }

    .tab-btn:hover {
      color: #111827;
    }

    .tab-btn.active {
      color: var(--accent-terracotta);
      border-bottom-color: var(--accent-terracotta);
      background: #ffffff;
    }

    /* Tab Body Scrollable */
    .tab-body {
      flex: 1;
      overflow-y: auto;
      padding: 24px;
      background: #ffffff;
    }

    .site-image-card {
      border-radius: 12px;
      overflow: hidden;
      margin-bottom: 20px;
      border: 1px solid #e5e7eb;
      box-shadow: 0 4px 14px rgba(0,0,0,0.06);
    }

    .site-image-card img {
      width: 100%;
      height: 220px;
      object-fit: cover;
      display: block;
    }

    .image-caption {
      font-size: 11px;
      color: #6b7280;
      padding: 6px 12px;
      background: #f9fafb;
      font-style: italic;
      border-top: 1px solid #f3f4f6;
    }

    .info-card {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 16px;
      margin-bottom: 16px;
    }

    .card-title {
      font-size: 14.5px;
      color: #111827;
      margin-bottom: 8px;
    }

    .sub-title {
      font-size: 12px;
      color: #4b5563;
      margin-bottom: 6px;
      font-weight: 700;
      text-transform: uppercase;
      letter-spacing: 0.04em;
    }

    .body-text {
      font-size: 13px;
      line-height: 1.6;
      color: #374151;
    }

    .highlight-text {
      font-size: 13.5px;
      line-height: 1.6;
      color: #111827;
    }

    .info-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 12px;
    }

    /* Stratigraphy Styling */
    .stratigraphy-intro {
      font-size: 12.5px;
      color: #6b7280;
      margin-bottom: 16px;
      font-style: italic;
    }

    .excavation-block {
      margin-bottom: 24px;
    }

    .excavation-header {
      background: #fef8e7;
      border: 1px solid #fef3c7;
      padding: 14px 18px;
      border-radius: 10px 10px 0 0;
    }

    .excavation-name {
      font-size: 15px;
      color: #92400e;
      font-weight: 700;
    }

    .excavator-meta {
      font-size: 11.5px;
      color: #78350f;
      margin-top: 2px;
    }

    .strata-sequence {
      display: flex;
      flex-direction: column;
      border-left: 2px solid var(--accent-terracotta);
      margin-left: 16px;
      padding-left: 16px;
      gap: 14px;
      margin-top: 12px;
    }

    .stratum-card {
      position: relative;
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 14px;
      display: flex;
      gap: 14px;
    }

    .stratum-depth-indicator {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      min-width: 52px;
      background: #ffffff;
      border: 1px solid #e5e7eb;
      border-radius: 8px;
      padding: 6px;
    }

    .depth-val {
      font-size: 15px;
      font-weight: 700;
      color: var(--accent-terracotta);
    }

    .depth-label {
      font-size: 9px;
      text-transform: uppercase;
      color: #9ca3af;
    }

    .stratum-info {
      flex: 1;
    }

    .stratum-head {
      display: flex;
      justify-content: space-between;
      align-items: baseline;
      margin-bottom: 4px;
    }

    .stratum-name {
      font-size: 14px;
      font-weight: 700;
      color: #111827;
    }

    .stratum-culture {
      font-size: 11px;
      color: var(--accent-emerald);
      font-weight: 600;
    }

    .stratum-dates {
      font-size: 11.5px;
      color: #92400e;
      margin-bottom: 6px;
    }

    .stratum-desc {
      font-size: 12px;
      color: #4b5563;
      line-height: 1.5;
      margin-bottom: 6px;
    }

    .stratum-soil {
      font-size: 11px;
      color: #6b7280;
    }

    .layer-findings {
      margin-top: 10px;
      padding-top: 8px;
      border-top: 1px dashed #e5e7eb;
    }

    .findings-header {
      font-size: 11px;
      font-weight: 700;
      color: var(--accent-terracotta);
      margin-bottom: 4px;
    }

    .finding-pill {
      font-size: 11px;
      color: #374151;
      margin-bottom: 2px;
    }

    .finding-type {
      color: #92400e;
      font-family: var(--font-mono);
      margin-right: 4px;
    }

    /* Artefacts Tab */
    .artefact-selector {
      display: flex;
      flex-wrap: wrap;
      gap: 6px;
      margin-bottom: 14px;
    }

    .art-chip {
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      border-radius: 8px;
      padding: 6px 12px;
      cursor: pointer;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      transition: all 0.2s ease;
    }

    .art-chip:hover, .art-chip.active {
      background: #fef8e7;
      border-color: #d4a373;
    }

    .art-num {
      font-size: 9.5px;
      color: var(--accent-terracotta);
      text-transform: uppercase;
      font-weight: 700;
    }

    .art-name {
      font-size: 12px;
      color: #111827;
      font-weight: 600;
    }

    .artefact-3d-wrapper {
      margin-bottom: 16px;
    }

    .artefact-details-card {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 16px;
    }

    .art-meta-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 8px;
      font-size: 12px;
      color: #4b5563;
      margin-top: 10px;
    }

    /* Spatial Section */
    .spatial-section {
      margin-bottom: 24px;
    }

    .section-sub {
      font-size: 11.5px;
      color: #6b7280;
      margin-bottom: 10px;
    }

    .nearby-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .nearby-card {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 12px 16px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .nearby-card:hover {
      background: #ffffff;
      border-color: var(--accent-terracotta);
      box-shadow: 0 4px 14px rgba(0,0,0,0.06);
      transform: translateX(4px);
    }

    .nearby-info {
      display: flex;
      flex-direction: column;
    }

    .nearby-name {
      font-size: 14px;
      font-weight: 700;
      color: #111827;
    }

    .nearby-loc {
      font-size: 11.5px;
      color: #6b7280;
    }

    .nearby-dates {
      font-size: 11px;
      color: #92400e;
    }

    .nearby-distance {
      font-size: 13px;
      font-weight: 700;
      color: #0369a1;
      background: #f0f9ff;
      border: 1px solid #bae6fd;
      padding: 4px 10px;
      border-radius: 8px;
    }

    .contemp-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 8px;
    }

    .contemp-card {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 8px;
      padding: 10px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .contemp-card:hover {
      background: #ffffff;
      border-color: var(--accent-terracotta);
      transform: translateY(-2px);
    }

    .contemp-name {
      font-size: 13px;
      font-weight: 700;
      color: #111827;
    }

    .contemp-loc {
      font-size: 11px;
      color: #6b7280;
    }

    .contemp-dates {
      font-size: 10.5px;
      color: #92400e;
      margin-top: 2px;
    }

    /* Bibliography Section */
    .citations-list {
      display: flex;
      flex-direction: column;
      gap: 12px;
      margin-top: 12px;
    }

    .citation-card {
      background: #f9fafb;
      border: 1px solid #e5e7eb;
      border-radius: 10px;
      padding: 16px;
      display: flex;
      gap: 12px;
    }

    .citation-key {
      color: var(--accent-terracotta);
      font-size: 12px;
      font-weight: 700;
      flex-shrink: 0;
    }

    .citation-content {
      font-size: 12px;
      line-height: 1.5;
    }

    .citation-title {
      font-weight: 700;
      color: #111827;
      font-size: 13.5px;
      margin-bottom: 2px;
    }

    .citation-authors {
      color: #4b5563;
    }

    .citation-publisher {
      color: #6b7280;
    }

    .citation-pages {
      color: #92400e;
      margin-top: 4px;
    }

    .citation-link {
      display: inline-block;
      margin-top: 6px;
      color: #0284c7;
      text-decoration: none;
      font-weight: 600;
    }

    .citation-link:hover {
      text-decoration: underline;
    }

    .empty-state {
      padding: 30px;
      text-align: center;
      color: #9ca3af;
      font-size: 13px;
    }
  `]
})
export class SiteDrawerComponent implements OnChanges {
  @Input() siteId: number | null = null;
  @Input() isOpen = false;
  @Output() closeDrawer = new EventEmitter<void>();
  @Output() compareSite = new EventEmitter<SiteDetail>();
  @Output() navigateToSite = new EventEmitter<number>();

  private readonly api = inject(ArchaeologyApiService);

  protected site: SiteDetail | null = null;
  protected nearbySites: NearbySiteResult[] = [];
  protected contemporaneousSites: SiteSummary[] = [];
  protected isLoading = false;

  protected readonly activeTab = signal<'overview' | 'stratigraphy' | 'artefacts' | 'spatial' | 'references'>('overview');
  protected readonly selectedArtefact = signal<Artefact | null>(null);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['siteId'] && this.siteId !== null && this.isOpen) {
      this.loadSiteData(this.siteId);
    }
  }

  setTab(tab: 'overview' | 'stratigraphy' | 'artefacts' | 'spatial' | 'references'): void {
    this.activeTab.set(tab);
  }

  selectArtefact(art: Artefact): void {
    this.selectedArtefact.set(art);
  }

  close(): void {
    this.closeDrawer.emit();
  }

  onCompareClicked(): void {
    if (this.site) {
      this.compareSite.emit(this.site);
    }
  }

  onNearbySiteSelected(targetId: number): void {
    this.navigateToSite.emit(targetId);
    this.loadSiteData(targetId);
  }

  countLayers(): number {
    if (!this.site || !this.site.excavations) return 0;
    return this.site.excavations.reduce((sum, exc) => sum + (exc.layers?.length || 0), 0);
  }

  private loadSiteData(id: number): void {
    this.isLoading = true;
    this.site = null;
    this.activeTab.set('overview');

    this.api.getSiteById(id).subscribe({
      next: (detail) => {
        this.site = detail;
        this.isLoading = false;
        if (detail.artefacts && detail.artefacts.length > 0) {
          this.selectedArtefact.set(detail.artefacts[0]);
        }

        this.api.getNearbySites(id, 800).subscribe({
          next: (nearby) => this.nearbySites = nearby,
          error: () => this.nearbySites = []
        });

        this.api.getContemporaneousSites(id).subscribe({
          next: (contemp) => this.contemporaneousSites = contemp,
          error: () => this.contemporaneousSites = []
        });
      },
      error: (err) => {
        console.error('Failed to load site detail', err);
        this.isLoading = false;
      }
    });
  }
}
