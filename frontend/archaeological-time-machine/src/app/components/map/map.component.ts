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
        <!-- Layer Switcher Pills (Free Esri Satellite / Carto Voyager / Topo) -->
        <div class="layer-switcher-pill">
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'voyager'"
            (click)="setTileStyle('voyager')"
            title="Clean Archaeological Cartography"
          >
            <span class="btn-icon">🗺️</span> Map
          </button>
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'satellite'"
            (click)="setTileStyle('satellite')"
            title="Satellite Aerial Imagery (Esri Free)"
          >
            <span class="btn-icon">🛰️</span> Satellite
          </button>
          <button
            class="layer-toggle-btn"
            [class.active]="activeTileStyle() === 'topo'"
            (click)="setTileStyle('topo')"
            title="Topographic Terrain"
          >
            <span class="btn-icon">🏔️</span> Terrain
          </button>
        </div>

        <!-- Quick Geographic Fly-To Controls -->
        <div class="quick-nav-pills">
          <button class="nav-pill-btn" (click)="focusIndia()" title="Center on Indian Archaeological Landscape">
            <span class="flag-icon">🇮🇳</span> India Focus
          </button>
          <button class="nav-pill-btn" (click)="focusGlobal()" title="Global View">
            <span>🌍</span> World
          </button>
        </div>
      </div>

      <!-- Live Coordinates & Scale Bar (Zoom Earth Style) -->
      <div class="map-floating-hud-bottom-right">
        <div class="coords-chip font-mono">
          <span>Lat: {{ cursorCoords().lat | number:'1.3-3' }}°</span>
          <span>Lng: {{ cursorCoords().lng | number:'1.3-3' }}°</span>
          <span class="zoom-level">Z: {{ currentZoom() }}</span>
        </div>
      </div>

      <!-- Horizons Legend Pill (Clean White Design) -->
      <div class="map-legend-card">
        <div class="legend-header">
          <span class="legend-icon">🏺</span>
          <span class="legend-title">Archaeological Horizons</span>
        </div>
        <div class="legend-items">
          <div class="legend-item"><span class="legend-dot harappan"></span> Indus / Harappan</div>
          <div class="legend-item"><span class="legend-dot mauryan"></span> Mauryan & Gangetic</div>
          <div class="legend-item"><span class="legend-dot sangam"></span> Sangam Maritime</div>
          <div class="legend-item"><span class="legend-dot copper"></span> Copper Age / Sinauli</div>
          <div class="legend-item"><span class="legend-dot mesolithic"></span> Prehistoric Rock Art</div>
          <div class="legend-item"><span class="legend-dot international"></span> Mediterranean / Near East</div>
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

    /* Clean White Legend Card */
    .map-legend-card {
      position: absolute;
      bottom: 24px;
      left: 18px;
      z-index: 800;
      background: rgba(255, 255, 255, 0.95);
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 12px;
      padding: 12px 16px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.08);
      backdrop-filter: blur(16px);
      max-width: 290px;
    }

    .legend-header {
      display: flex;
      align-items: center;
      gap: 6px;
      margin-bottom: 8px;
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
    .legend-dot.mesolithic { background: #c25e2e; }
    .legend-dot.international { background: #d4a373; }
  `]
})
export class MapComponent implements OnInit, OnChanges, OnDestroy {
  @Input() sites: SiteSummary[] = [];
  @Input() selectedSiteId: number | null = null;
  @Output() siteSelected = new EventEmitter<number>();

  @ViewChild('mapContainer', { static: true }) mapContainerRef!: ElementRef<HTMLDivElement>;

  protected readonly activeTileStyle = signal<MapTileStyle>('voyager');
  protected readonly cursorCoords = signal<{ lat: number; lng: number }>({ lat: 22.5, lng: 78.5 });
  protected readonly currentZoom = signal<number>(5);

  private map!: L.Map;
  private currentTileLayer: L.TileLayer | null = null;
  private markerLayerGroup = L.layerGroup();
  private markersMap = new Map<number, L.Marker>();

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
  }

  ngOnDestroy(): void {
    if (this.map) {
      this.map.remove();
    }
  }

  /**
   * Switches map tile layer between Carto Voyager (clean map), Esri Satellite, and Terrain
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
        // High-resolution Esri World Imagery (Completely free, no key needed)
        url = 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}';
        attribution = '&copy; Esri, Maxar, Earthstar Geographics';
        maxZoom = 18;
        break;
      case 'topo':
        // OpenTopoMap / Relief terrain (Completely free)
        url = 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png';
        attribution = '&copy; OpenTopoMap & OpenStreetMap contributors';
        maxZoom = 17;
        break;
      case 'voyager':
      default:
        // CartoDB Voyager clean archaeological cartography (Completely free)
        url = 'https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png';
        attribution = '&copy; <a href="https://carto.com/">CARTO</a> & OpenStreetMap';
        maxZoom = 19;
        break;
    }

    this.currentTileLayer = L.tileLayer(url, {
      attribution,
      subdomains: style === 'satellite' ? [] : ['a', 'b', 'c', 'd'],
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

  private initMap(): void {
    const container = this.mapContainerRef.nativeElement;

    // Centered on the Indian subcontinent by default
    this.map = L.map(container, {
      center: [23.5, 76.5],
      zoom: 5,
      minZoom: 2,
      maxZoom: 18,
      zoomControl: false
    });

    L.control.zoom({ position: 'topleft' }).addTo(this.map);

    // Track mouse coordinates for Zoom Earth style readout
    this.map.on('mousemove', (e: L.LeafletMouseEvent) => {
      this.cursorCoords.set({ lat: e.latlng.lat, lng: e.latlng.lng });
    });

    this.map.on('zoomend', () => {
      this.currentZoom.set(this.map.getZoom());
    });

    // Default to clean Voyager map
    this.setTileStyle('voyager');
    this.markerLayerGroup.addTo(this.map);
  }

  private renderMarkers(): void {
    if (!this.map) return;
    this.markerLayerGroup.clearLayers();
    this.markersMap.clear();

    for (const site of this.sites) {
      const color = site.civilizations[0]?.colorHex || '#c25e2e';
      const isIndia = site.country.toLowerCase() === 'india';

      // Custom Floating Pin SVG icon (Clean White Bordered Pin)
      const customIcon = L.divIcon({
        className: 'custom-arch-marker',
        html: `
          <div class="arch-marker-pin ${isIndia ? 'marker-india' : ''}" style="background-color: ${color}; color: ${color}">
            <div class="arch-marker-inner" style="color: #ffffff; text-shadow: 0 1px 3px rgba(0,0,0,0.5);">
              ${isIndia ? '🏛️' : '🏺'}
            </div>
            <div class="arch-marker-pulse"></div>
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

      const popupHtml = `
        <div style="padding: 16px 18px; min-width: 260px; font-family: 'Google Sans Text', sans-serif;">
          <div style="margin-bottom: 6px;">${unescoBadge}</div>
          <h3 style="font-family: 'Google Sans Display', sans-serif; font-size: 17px; font-weight: 700; color: #111827; margin: 4px 0 2px;">
            ${site.name}
          </h3>
          ${site.ancientName ? `<div style="font-size: 12px; color: #c25e2e; font-style: italic; margin-bottom: 6px;">Ancient: ${site.ancientName}</div>` : ''}
          <div style="font-size: 12px; color: #4b5563; margin-bottom: 8px;">
            📍 ${site.region}, <strong>${site.country}</strong>
          </div>
          <div style="font-family: 'JetBrains Mono', monospace; font-size: 11px; color: #92400e; background: #fef8e7; padding: 4px 8px; border-radius: 6px; margin-bottom: 10px; border: 1px solid #fef3c7; display: inline-block;">
            ⏳ ${site.startYearFormatted} – ${site.endYearFormatted}
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
}
