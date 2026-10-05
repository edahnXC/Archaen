import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import * as L from 'leaflet';
import { SiteSummary } from '../../models/archaeology.models';

export type MapTileStyle = 'voyager' | 'satellite' | 'topo';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="map-wrapper">
      <div #mapContainer class="map-container"></div>
      
      <!-- Zoom Earth Style Floating HUD: Map Style Switcher & Controls -->
      <div class="map-floating-hud-top-right">
        <!-- Layer Switcher Pills (100% Free OpenStreetMap / Esri Satellite / Topo - Zero Watermarks) -->
        <div class="layer-switcher-pill">
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'voyager'"
            (click)="setTileStyle('voyager')"
            title="Clean OpenStreetMap Cartography (Free & Open)"
          >
            <span class="btn-icon">🗺️</span> Map
          </button>
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'satellite'"
            (click)="setTileStyle('satellite')"
            title="High-Resolution Satellite Aerial Imagery (Esri Free)"
          >
            <span class="btn-icon">🛰️</span> Satellite
          </button>
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'topo'"
            (click)="setTileStyle('topo')"
            title="Topographic Terrain & Contours (Esri Free)"
          >
            <span class="btn-icon">🏔️</span> Terrain
          </button>
        </div>

        <!-- Quick Geographic Fly-To Controls -->
        <div class="quick-nav-pills">
          <button class="nav-pill-btn" (click)="focusIndia()" title="Center on Indian Archaeological Landscape">
            <span class="flag-icon">🇮🇳</span> India Focus
          </button>
          <button class="nav-pill-btn meroe-pill" (click)="focusMeroe()" title="Fly to Pyramids of Meroë (Sudan)">
            <span>🔺</span> Pyramids of Meroë
          </button>
          <button class="nav-pill-btn" (click)="focusGlobal()" title="Global View">
            <span>🌍</span> World
          </button>
        </div>
      </div>

      <!-- 4D Spatio-Temporal Dynamic Horizon HUD (Top Center) -->
      <div class="map-floating-4d-hud" *ngIf="isTimeFilterActive || isPlayingFlight">
        <div class="hud-4d-header">
          <span class="hud-4d-pulse-dot" [class.animating]="isPlayingFlight"></span>
          <span class="hud-4d-tag font-mono">4D ARCHAEOLOGICAL HORIZON</span>
          <button
            type="button"
            class="hud-4d-flight-btn"
            (click)="flightToggle.emit()"
            [title]="isPlayingFlight ? 'Pause continuous chronological sweep' : 'Sweep continuously through time'"
          >
            {{ isPlayingFlight ? '⏸ Pause' : '▶ 4D Flight' }}
          </button>
        </div>
        <div class="hud-4d-center">
          <span class="hud-4d-year font-display">{{ formattedYear() }}</span>
          <span class="hud-4d-epoch">{{ activeEpochName() }}</span>
        </div>
        <div class="hud-4d-meta font-mono">
          <span class="meta-pill">● {{ activeSitesCount() }} Active Archaeological Settlements</span>
          <span class="meta-pill">🌐 Calibrated Spatial Horizon</span>
        </div>
      </div>

      <!-- Natural Page Scroll Safety Hint (Bottom Left) -->
      <div class="map-scroll-hint-bar">
        <span class="scroll-mouse-icon">🖱️</span>
        <span class="scroll-hint-text">Page scrolls naturally • Use <strong>+ / -</strong> to zoom map</span>
      </div>

      <!-- Live Coordinates & Scale Bar (Zoom Earth Style) -->
      <div class="map-floating-hud-bottom-right">
        <div class="coords-chip font-mono">
          <span>Lat: {{ cursorCoords().lat | number:'1.3-3' }}°</span>
          <span>Lng: {{ cursorCoords().lng | number:'1.3-3' }}°</span>
          <span class="zoom-level">Z: {{ currentZoom() }}</span>
        </div>
      </div>

      <!-- Horizons Legend Pill (Clean White Design with Collapse Toggle) -->
      <div class="map-legend-card" [class.collapsed]="isLegendCollapsed()">
        <div class="legend-header" (click)="toggleLegend()" title="Click to minimize or expand horizons legend">
          <span class="legend-icon">🏺</span>
          <span class="legend-title">Archaeological Horizons</span>
          <button type="button" class="legend-toggle-btn" aria-label="Toggle Horizons Legend">
            {{ isLegendCollapsed() ? '▲ Show' : '▼ Hide' }}
          </button>
        </div>
        <div class="legend-items" *ngIf="!isLegendCollapsed()">
          <div class="legend-item"><span class="legend-dot harappan"></span> Indus / Harappan</div>
          <div class="legend-item"><span class="legend-dot mauryan"></span> Mauryan & Gangetic</div>
          <div class="legend-item"><span class="legend-dot sangam"></span> Sangam Maritime</div>
          <div class="legend-item"><span class="legend-dot copper"></span> Copper Age / Sinauli</div>
          <div class="legend-item"><span class="legend-dot kushite"></span> Kushite / Meroë (Sudan)</div>
          <div class="legend-item"><span class="legend-dot international"></span> World Heritage Wonders</div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .map-wrapper {
      position: relative;
      width: 100%;
      height: 100%;
      overflow: hidden;
      background: #f1f3f5;
    }

    .map-container {
      width: 100%;
      height: 100%;
    }

    /* Floating HUD Top Right (Zoom Earth inspired) */
    .map-floating-hud-top-right {
      position: absolute;
      top: 18px;
      right: 18px;
      z-index: 800;
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      gap: 10px;
    }

    .layer-switcher-pill {
      display: inline-flex;
      background: rgba(255, 255, 255, 0.94);
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 30px;
      padding: 3px;
      box-shadow: 0 4px 18px rgba(0, 0, 0, 0.08);
      backdrop-filter: blur(16px);
    }

    .layer-toggle-btn {
      display: inline-flex;
      align-items: center;
      gap: 5px;
      background: none;
      border: none;
      padding: 6px 14px;
      border-radius: 20px;
      font-family: var(--font-body);
      font-size: 12px;
      font-weight: 600;
      color: #4b5563;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .layer-toggle-btn:hover {
      color: #111827;
      background: rgba(0, 0, 0, 0.04);
    }

    .layer-toggle-btn.active {
      background: #111827;
      color: #ffffff;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
    }

    .quick-nav-pills {
      display: flex;
      gap: 6px;
    }

    .nav-pill-btn {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: rgba(255, 255, 255, 0.94);
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 20px;
      padding: 6px 14px;
      font-family: var(--font-body);
      font-size: 12px;
      font-weight: 600;
      color: #1f2937;
      box-shadow: 0 4px 14px rgba(0, 0, 0, 0.06);
      backdrop-filter: blur(16px);
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .nav-pill-btn:hover {
      background: #ffffff;
      border-color: var(--accent-terracotta);
      color: var(--accent-terracotta);
      transform: translateY(-1px);
      box-shadow: 0 6px 18px rgba(0, 0, 0, 0.1);
    }

    /* Floating HUD Bottom Right (Coords & Zoom) */
    .map-floating-hud-bottom-right {
      position: absolute;
      bottom: 24px;
      right: 18px;
      z-index: 800;
    }

    .coords-chip {
      display: inline-flex;
      align-items: center;
      gap: 10px;
      background: rgba(255, 255, 255, 0.92);
      border: 1px solid rgba(0, 0, 0, 0.08);
      border-radius: 20px;
      padding: 5px 12px;
      font-size: 11px;
      color: #4b5563;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.06);
      backdrop-filter: blur(10px);
    }

    .zoom-level {
      font-weight: 700;
      color: var(--accent-terracotta);
    }

    /* Clean White Legend Card - Elevated above bottom timeline scrubber bar */
    .map-legend-card {
      position: absolute;
      bottom: 118px;
      left: 18px;
      z-index: 800;
      background: rgba(255, 255, 255, 0.96);
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 12px;
      padding: 10px 14px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.08);
      backdrop-filter: blur(16px);
      max-width: 290px;
      transition: all 0.25s ease;
    }

    .map-legend-card.collapsed {
      padding: 6px 12px;
    }

    .legend-header {
      display: flex;
      align-items: center;
      gap: 6px;
      margin-bottom: 0;
      cursor: pointer;
      user-select: none;
    }

    .map-legend-card:not(.collapsed) .legend-header {
      margin-bottom: 8px;
    }

    .legend-toggle-btn {
      margin-left: auto;
      background: rgba(0, 0, 0, 0.05);
      border: 1px solid rgba(0, 0, 0, 0.08);
      border-radius: 10px;
      font-size: 9.5px;
      font-weight: 700;
      color: #64748b;
      padding: 2px 7px;
      cursor: pointer;
      transition: all 0.15s ease;
    }

    .legend-toggle-btn:hover {
      background: #111827;
      color: #ffffff;
    }

    .legend-icon {
      font-size: 14px;
    }

    .legend-title {
      font-family: var(--font-display);
      font-size: 11px;
      letter-spacing: 0.06em;
      text-transform: uppercase;
      color: #111827;
      font-weight: 700;
    }

    .legend-items {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 6px 12px;
    }

    .legend-item {
      display: flex;
      align-items: center;
      gap: 6px;
      font-size: 11px;
      color: #4b5563;
      font-weight: 500;
    }

    .legend-dot {
      width: 9px;
      height: 9px;
      border-radius: 50%;
      flex-shrink: 0;
    }

    .legend-dot.harappan { background: #e06a3b; }
    .legend-dot.mauryan { background: #b91c1c; }
    .legend-dot.sangam { background: #0f766e; }
    .legend-dot.copper { background: #9d4edd; }
    .legend-dot.kushite { background: #c59b27; }
    .legend-dot.international { background: #2563eb; }

    .meroe-pill {
      background: #fef8e7;
      border-color: #fef3c7;
      color: #92400e;
    }

    .meroe-pill:hover {
      background: #fde68a;
      color: #78350f;
    }

    .nav-pill-btn.active {
      background: #0284c7;
      color: #ffffff;
      border-color: #0284c7;
    }


    /* ==========================================================================
       4D SPATIO-TEMPORAL CHRONOMETER HUD (TOP CENTER OF MAP)
       ========================================================================== */

    .map-floating-4d-hud {
      position: absolute;
      top: 18px;
      left: 50%;
      transform: translateX(-50%);
      z-index: 850;
      background: rgba(255, 255, 255, 0.96);
      border: 1px solid rgba(0, 0, 0, 0.14);
      border-radius: 20px;
      padding: 10px 22px;
      box-shadow: 0 10px 30px rgba(0, 0, 0, 0.12);
      backdrop-filter: blur(16px);
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 3px;
      pointer-events: auto;
      text-align: center;
      min-width: 320px;
    }

    .hud-4d-header {
      display: inline-flex;
      align-items: center;
      gap: 8px;
    }

    .hud-4d-flight-btn {
      background: #0f172a;
      color: #ffffff;
      border: none;
      padding: 3px 10px;
      border-radius: 12px;
      font-size: 10px;
      font-weight: 700;
      letter-spacing: 0.04em;
      cursor: pointer;
      display: inline-flex;
      align-items: center;
      gap: 4px;
      transition: all 0.2s ease;
      box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
    }

    .hud-4d-flight-btn:hover {
      background: var(--accent-terracotta, #c25e2e);
      transform: scale(1.04);
    }

    .hud-4d-pulse-dot {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      background: #16a34a;
      box-shadow: 0 0 0 0 rgba(22, 163, 74, 0.7);
      animation: pulseGreen 1.8s infinite;
    }

    .hud-4d-pulse-dot.animating {
      background: #c25e2e;
      box-shadow: 0 0 0 0 rgba(194, 94, 46, 0.8);
      animation: pulseTerracotta 0.9s infinite;
    }

    @keyframes pulseGreen {
      0% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(22, 163, 74, 0.7); }
      70% { transform: scale(1); box-shadow: 0 0 0 8px rgba(22, 163, 74, 0); }
      100% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(22, 163, 74, 0); }
    }

    @keyframes pulseTerracotta {
      0% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(194, 94, 46, 0.8); }
      70% { transform: scale(1.2); box-shadow: 0 0 0 10px rgba(194, 94, 46, 0); }
      100% { transform: scale(0.95); box-shadow: 0 0 0 0 rgba(194, 94, 46, 0); }
    }

    .hud-4d-tag {
      font-size: 9.5px;
      font-weight: 800;
      letter-spacing: 0.1em;
      color: #0f766e;
    }

    .hud-4d-center {
      display: flex;
      align-items: baseline;
      gap: 8px;
    }

    .hud-4d-year {
      font-size: 22px;
      font-weight: 800;
      color: #111827;
      letter-spacing: -0.02em;
    }

    .hud-4d-epoch {
      font-size: 11.5px;
      font-weight: 600;
      color: var(--accent-terracotta, #c25e2e);
    }

    .hud-4d-meta {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 10.5px;
      color: #64748b;
    }

    .meta-pill {
      background: #f1f5f9;
      padding: 1px 7px;
      border-radius: 10px;
    }

    /* Natural Page Scroll Safety Hint (Bottom Right) */
    .map-scroll-hint-bar {
      position: absolute;
      bottom: 60px;
      right: 70px;
      z-index: 800;
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: rgba(255, 255, 255, 0.92);
      border: 1px solid rgba(0, 0, 0, 0.08);
      border-radius: 20px;
      padding: 5px 12px;
      font-size: 11px;
      color: #64748b;
      backdrop-filter: blur(12px);
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
      pointer-events: none;
    }

    /* Leaflet Controls Custom Placement (Bottom Right above coordinates) */
    :host ::ng-deep .leaflet-bottom.leaflet-right {
      margin-bottom: 58px !important;
      margin-right: 18px !important;
      z-index: 850 !important;
    }

    :host ::ng-deep .leaflet-control-zoom {
      border: 1px solid rgba(0, 0, 0, 0.12) !important;
      border-radius: 12px !important;
      overflow: hidden !important;
      box-shadow: 0 6px 18px rgba(0, 0, 0, 0.12) !important;
    }

    :host ::ng-deep .leaflet-control-zoom a {
      background: rgba(255, 255, 255, 0.96) !important;
      color: #111827 !important;
      font-weight: 700 !important;
      backdrop-filter: blur(12px) !important;
      transition: all 0.2s ease !important;
      width: 32px !important;
      height: 32px !important;
      line-height: 32px !important;
    }

    :host ::ng-deep .leaflet-control-zoom a:hover {
      background: #111827 !important;
      color: #ffffff !important;
    }
  `]
})
export class MapComponent implements OnInit, OnChanges, OnDestroy {
  @Input() sites: SiteSummary[] = [];
  @Input() selectedSiteId: number | null = null;
  @Input() currentYear: number = -2500;
  @Input() isTimeFilterActive: boolean = false;
  @Input() isPlayingFlight: boolean = false;
  @Output() siteSelected = new EventEmitter<number>();
  @Output() flightToggle = new EventEmitter<void>();

  public readonly isLegendCollapsed = signal(false);

  toggleLegend(): void {
    this.isLegendCollapsed.update(c => !c);
  }

  @ViewChild('mapContainer', { static: true }) mapContainerRef!: ElementRef<HTMLDivElement>;

  protected readonly activeTileStyle = signal<MapTileStyle>('voyager');
  protected readonly cursorCoords = signal<{ lat: number; lng: number }>({ lat: 22.5, lng: 78.5 });
  protected readonly currentZoom = signal<number>(5);

  private map!: L.Map;
  private currentTileLayer: L.TileLayer | null = null;
  private markerLayerGroup = L.layerGroup();
  private markersMap = new Map<number, L.Marker>();

  formattedYear(): string {
    const y = this.currentYear;
    if (y < 0) return `${Math.abs(y)} BCE`;
    return `${y} CE`;
  }

  activeEpochName(): string {
    const y = this.currentYear;
    if (y <= -2600) return 'Early Urban & Mature Indus Integration';
    if (y <= -1900) return 'Mature Harappan Metropolis Era (Giza & Ur)';
    if (y <= -1400) return 'Copper Hoard Warrior Horizon (Sinauli Chariots)';
    if (y <= -600) return 'Early Iron Age & Painted Grey Ware';
    if (y <= -300) return 'Second Urbanization & Sangam Dawn (Keeladi)';
    if (y <= -185) return 'Mauryan Imperial Horizon (Ashokan Edicts)';
    if (y <= 400) return 'Classical Antiquity & Indo-Roman Maritime Horizon';
    return 'Post-Classical & Medieval World Horizons';
  }

  activeSitesCount(): number {
    if (!this.isTimeFilterActive) return this.sites.length;
    return this.sites.filter(s => (this.currentYear >= s.startYear - 80) && (this.currentYear <= s.endYear + 80)).length;
  }

  ngOnInit(): void {
    this.initMap();
    this.renderMarkers();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['sites'] && !changes['sites'].firstChange) {
      this.renderMarkers();
    }
    if (changes['selectedSiteId'] && this.selectedSiteId !== null) {
      this.highlightSelectedSite(this.selectedSiteId);
    }
    if (changes['currentYear'] || changes['isTimeFilterActive'] || changes['isPlayingFlight']) {
      this.renderMarkers();
    }
  }

  focusMeroe(): void {
    if (!this.map) return;
    this.map.flyTo([16.9383, 33.7492], 6.5, {
      duration: 1.8,
      easeLinearity: 0.25
    });
  }

  /**
   * Switches map tile layer between OpenStreetMap Standard (clean map), Esri Satellite, and Terrain
   */
  setTileStyle(style: MapTileStyle): void {
    if (!this.map) return;
    this.activeTileStyle.set(style);

    if (this.currentTileLayer) {
      this.map.removeLayer(this.currentTileLayer);
    }

    let url = '';
    let attribution = '';
    let maxZoom = 19;

    switch (style) {
      case 'satellite':
        // High-resolution Esri World Imagery (Completely free, no watermark, no key required)
        url = 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}';
        attribution = '&copy; Esri, Maxar, Earthstar Geographics';
        maxZoom = 18;
        break;
      case 'topo':
        // Esri World Topographical Map with archaeological contours (Free, no watermark)
        url = 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}';
        attribution = '&copy; Esri, DeLorme, NAVTEQ, USGS, Intermap';
        maxZoom = 18;
        break;
      case 'voyager':
      default:
        // OpenStreetMap Standard - Clean, universal, 100% free with no watermark or API key
        url = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png';
        attribution = '&copy; <a href="https://www.openstreetmap.org/copyright" target="_blank">OpenStreetMap</a> contributors';
        maxZoom = 19;
        break;
    }

    this.currentTileLayer = L.tileLayer(url, {
      attribution,
      subdomains: style === 'satellite' || style === 'topo' ? [] : ['a', 'b', 'c'],
      maxZoom
    });

    this.currentTileLayer.addTo(this.map);
  }

  focusIndia(): void {
    if (!this.map) return;
    this.map.flyTo([22.5, 78.5], 5, {
      duration: 1.5,
      easeLinearity: 0.25
    });
  }

  focusGlobal(): void {
    if (!this.map) return;
    this.map.flyTo([28.0, 50.0], 3.5, {
      duration: 1.8,
      easeLinearity: 0.25
    });
  }

  focusSite(latitude: number, longitude: number, zoom: number = 7): void {
    if (!this.map) return;
    this.map.flyTo([latitude, longitude], zoom, { duration: 1.2 });
  }

  invalidateSize(): void {
    if (this.map) {
      setTimeout(() => this.map.invalidateSize(), 60);
    }
  }

  private initMap(): void {
    const container = this.mapContainerRef.nativeElement;

    // Centered on the Indian subcontinent by default
    // scrollWheelZoom: false ensures mouse wheel never hijacks page scrolling
    this.map = L.map(container, {
      center: [23.5, 76.5],
      zoom: 5,
      minZoom: 2,
      maxZoom: 18,
      zoomControl: false,
      scrollWheelZoom: false
    });

    L.control.zoom({ position: 'bottomright' }).addTo(this.map);

    // Track mouse coordinates for Zoom Earth style readout
    this.map.on('mousemove', (e: L.LeafletMouseEvent) => {
      this.cursorCoords.set({ lat: e.latlng.lat, lng: e.latlng.lng });
    });

    this.map.on('zoomend', () => {
      this.currentZoom.set(this.map.getZoom());
    });

    // Default to clean standard map
    this.setTileStyle('voyager');
    this.markerLayerGroup.addTo(this.map);
  }

  private renderMarkers(): void {
    if (!this.map) return;
    this.markerLayerGroup.clearLayers();
    this.markersMap.clear();

    const activeYear = this.currentYear;
    const isFilterOn = this.isTimeFilterActive;

    for (const site of this.sites) {
      const color = site.civilizations[0]?.colorHex || '#c25e2e';
      const isIndia = site.country.toLowerCase() === 'india';

      // 4D Spatio-Temporal calculation:
      // Active if within occupation span (+/- 80 yrs buffer for transition phases)
      const isOccupied = !isFilterOn || (activeYear >= (site.startYear - 80) && activeYear <= (site.endYear + 80));
      const isFuture = isFilterOn && (activeYear < site.startYear - 80);
      const isPast = isFilterOn && (activeYear > site.endYear + 80);

      const stateClass = isOccupied ? 'marker-4d-active' : 'marker-4d-dormant';

      // Custom Floating Pin SVG icon with 4D dynamic classes
      const customIcon = L.divIcon({
        className: 'custom-arch-marker',
        html: `
          <div class="arch-marker-pin ${stateClass} ${isIndia ? 'marker-india' : ''}" style="background-color: ${isOccupied ? color : '#94a3b8'}; color: ${isOccupied ? color : '#94a3b8'}">
            <div class="arch-marker-inner" style="color: #ffffff; text-shadow: 0 1px 3px rgba(0,0,0,0.5);">
              ${isPast ? '🗿' : (isIndia ? '🏛️' : '🏺')}
            </div>
            ${isOccupied ? '<div class="arch-marker-pulse"></div>' : ''}
          </div>
        `,
        iconSize: [34, 34],
        iconAnchor: [17, 34],
        popupAnchor: [0, -34]
      });

      const marker = L.marker([site.latitude, site.longitude], { icon: customIcon });

      // Clean White Museum Popup (Inspired by Google Arts & Culture)
      const unescoBadge = site.isUnescoWorldHeritage
        ? `<div class="badge-unesco">★ UNESCO World Heritage</div>`
        : '';

      const civTags = site.civilizations
        .map(c => `<span style="font-size: 11px; font-weight: 600; padding: 2px 7px; border-radius: 4px; background: rgba(0,0,0,0.05); color: ${c.colorHex}; border: 1px solid ${c.colorHex}30;">${c.civilizationName}</span>`)
        .join(' ');

      let temporalStatusHtml = '';
      if (isFilterOn) {
        if (isOccupied) {
          temporalStatusHtml = `<div style="font-size: 11px; font-weight: 700; color: #15803d; background: #dcfce7; padding: 4px 8px; border-radius: 6px; margin-bottom: 8px; border: 1px solid #bbf7d0;">
            ● FLOURISHING SETTLEMENT in ${this.formattedYear()}
          </div>`;
        } else if (isFuture) {
          temporalStatusHtml = `<div style="font-size: 11px; font-weight: 600; color: #64748b; background: #f1f5f9; padding: 4px 8px; border-radius: 6px; margin-bottom: 8px; border: 1px solid #e2e8f0;">
            ⏳ Unfounded in ${this.formattedYear()} • Emerges ${site.startYearFormatted}
          </div>`;
        } else {
          temporalStatusHtml = `<div style="font-size: 11px; font-weight: 600; color: #b45309; background: #fef3c7; padding: 4px 8px; border-radius: 6px; margin-bottom: 8px; border: 1px solid #fde68a;">
            🗿 Historic Ruin in ${this.formattedYear()} • Declined c. ${site.endYearFormatted}
          </div>`;
        }
      }

      const popupHtml = `
        <div style="padding: 16px 18px; min-width: 260px; font-family: 'Google Sans Text', sans-serif;">
          <div style="margin-bottom: 6px;">${unescoBadge}</div>
          ${temporalStatusHtml}
          <h3 style="font-family: 'Google Sans Display', sans-serif; font-size: 17px; font-weight: 700; color: #111827; margin: 4px 0 2px;">
            ${site.name}
          </h3>
          ${site.ancientName ? `<div style="font-size: 12px; color: #c25e2e; font-style: italic; margin-bottom: 6px;">Ancient: ${site.ancientName}</div>` : ''}
          <div style="font-size: 12px; color: #4b5563; margin-bottom: 8px;">
            📍 ${site.region}, <strong>${site.country}</strong>
          </div>
          <div style="font-family: 'JetBrains Mono', monospace; font-size: 11px; color: #92400e; background: #fef8e7; padding: 4px 8px; border-radius: 6px; margin-bottom: 10px; border: 1px solid #fef3c7; display: inline-block;">
            ⏳ Span: ${site.startYearFormatted} – ${site.endYearFormatted}
          </div>
          <div style="margin-bottom: 14px; display: flex; flex-wrap: wrap; gap: 4px;">
            ${civTags}
          </div>
          <button id="explore-btn-${site.id}" style="width: 100%; background: #111827; border: none; color: #ffffff; font-weight: 600; font-size: 12.5px; padding: 8px 14px; border-radius: 8px; cursor: pointer; display: flex; align-items: center; justify-content: center; gap: 6px; transition: background 0.2s ease;">
            Explore Excavation & Artefacts →
          </button>
        </div>
      `;

      marker.bindPopup(popupHtml);

      marker.on('popupopen', () => {
        setTimeout(() => {
          const btn = document.getElementById(`explore-btn-${site.id}`);
          if (btn) {
            btn.onclick = () => {
              this.siteSelected.emit(site.id);
            };
          }
        }, 50);
      });

      marker.on('click', () => {
        this.siteSelected.emit(site.id);
      });

      this.markerLayerGroup.addLayer(marker);
      this.markersMap.set(site.id, marker);
    }
  }

  private highlightSelectedSite(siteId: number): void {
    const marker = this.markersMap.get(siteId);
    if (marker) {
      const latLng = marker.getLatLng();
      this.map.flyTo(latLng, Math.max(this.map.getZoom(), 7), { duration: 1.0 });
      marker.openPopup();
    }
  }

  ngOnDestroy(): void {
    if (this.map) {
      this.map.remove();
    }
  }
}
