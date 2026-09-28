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
  ViewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import * as L from 'leaflet';
import { SiteSummary } from '../../models/archaeology.models';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="map-wrapper">
      <div #mapContainer class="map-container"></div>
      
      <!-- Map Quick Navigation Floating Controls -->
      <div class="map-overlay-tools">
        <button class="map-tool-btn" (click)="focusIndia()" title="Center on Indian Archaeological Landscape">
          <span class="flag-icon">🇮🇳</span> Focus India
        </button>
        <button class="map-tool-btn" (click)="focusGlobal()" title="Global Ancient Civilizations View">
          <span class="globe-icon">🌍</span> World View
        </button>
      </div>

      <!-- Live Marker Legend -->
      <div class="map-legend">
        <div class="legend-title">Archaeological Horizons</div>
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
    }

    .map-container {
      width: 100%;
      height: 100%;
      background: #090a0d;
    }

    .map-overlay-tools {
      position: absolute;
      top: 16px;
      left: 60px;
      z-index: 800;
      display: flex;
      gap: 8px;
    }

    .map-tool-btn {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: rgba(18, 20, 26, 0.85);
      border: 1px solid rgba(212, 175, 55, 0.35);
      color: #f3f4f6;
      font-size: 12px;
      font-weight: 600;
      padding: 7px 13px;
      border-radius: 8px;
      backdrop-filter: blur(10px);
      box-shadow: 0 4px 16px rgba(0, 0, 0, 0.5);
      cursor: pointer;
      transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .map-tool-btn:hover {
      background: rgba(212, 175, 55, 0.2);
      border-color: #d4af37;
      color: #ffd166;
      transform: translateY(-1px);
    }

    .map-legend {
      position: absolute;
      bottom: 24px;
      left: 18px;
      z-index: 800;
      background: rgba(18, 20, 26, 0.88);
      border: 1px solid rgba(255, 255, 255, 0.1);
      border-radius: 8px;
      padding: 10px 14px;
      backdrop-filter: blur(12px);
      box-shadow: 0 6px 20px rgba(0, 0, 0, 0.6);
      max-width: 280px;
    }

    .legend-title {
      font-family: var(--font-display, serif);
      font-size: 11px;
      letter-spacing: 0.05em;
      text-transform: uppercase;
      color: #e9c46a;
      margin-bottom: 6px;
      font-weight: 700;
    }

    .legend-items {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 5px 12px;
    }

    .legend-item {
      display: flex;
      align-items: center;
      gap: 6px;
      font-size: 10.5px;
      color: #cbd5e1;
    }

    .legend-dot {
      width: 9px;
      height: 9px;
      border-radius: 50%;
      flex-shrink: 0;
    }

    .legend-dot.harappan { background: #e06a3b; box-shadow: 0 0 6px #e06a3b; }
    .legend-dot.mauryan { background: #d90429; box-shadow: 0 0 6px #d90429; }
    .legend-dot.sangam { background: #2a9d8f; box-shadow: 0 0 6px #2a9d8f; }
    .legend-dot.copper { background: #b5838d; box-shadow: 0 0 6px #b5838d; }
    .legend-dot.mesolithic { background: #6d597a; box-shadow: 0 0 6px #6d597a; }
    .legend-dot.international { background: #d4a373; box-shadow: 0 0 6px #d4a373; }
  `]
})
export class MapComponent implements OnInit, OnChanges, OnDestroy {
  @Input() sites: SiteSummary[] = [];
  @Input() selectedSiteId: number | null = null;
  @Output() siteSelected = new EventEmitter<number>();

  @ViewChild('mapContainer', { static: true }) mapContainerRef!: ElementRef<HTMLDivElement>;

  private map!: L.Map;
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
   * Smoothly pans and zooms to the Indian subcontinent.
   */
  focusIndia(): void {
    if (!this.map) return;
    this.map.flyTo([22.5, 78.5], 5, {
      duration: 1.5,
      easeLinearity: 0.25
    });
  }

  /**
   * Smoothly pans out to show all ancient civilizations.
   */
  focusGlobal(): void {
    if (!this.map) return;
    this.map.flyTo([28.0, 50.0], 3.5, {
      duration: 1.8,
      easeLinearity: 0.25
    });
  }

  /**
   * Focuses on a specific coordinate.
   */
  focusSite(latitude: number, longitude: number, zoom: number = 7): void {
    if (!this.map) return;
    this.map.flyTo([latitude, longitude], zoom, {
      duration: 1.2
    });
  }

  private initMap(): void {
    const container = this.mapContainerRef.nativeElement;

    // Centered on the Indian subcontinent by default with priority view
    this.map = L.map(container, {
      center: [23.5, 76.5],
      zoom: 5,
      minZoom: 2,
      maxZoom: 18,
      zoomControl: false
    });

    L.control.zoom({ position: 'topleft' }).addTo(this.map);

    // CartoDB Dark Matter / Voyager high-aesthetic basemap
    const darkTileLayer = L.tileLayer('https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', {
      attribution: '&copy; <a href="https://carto.com/">CARTO</a> | Archaeological Survey Data',
      subdomains: 'abcd',
      maxZoom: 19,
      className: 'archaeology-tiles'
    });

    darkTileLayer.addTo(this.map);
    this.markerLayerGroup.addTo(this.map);
  }

  private renderMarkers(): void {
    if (!this.map) return;
    this.markerLayerGroup.clearLayers();
    this.markersMap.clear();

    for (const site of this.sites) {
      const color = site.civilizations[0]?.colorHex || '#e06a3b';
      const isIndia = site.country.toLowerCase() === 'india';

      // Custom Archaeological Pin SVG icon
      const customIcon = L.divIcon({
        className: 'custom-arch-marker',
        html: `
          <div class="arch-marker-pin ${isIndia ? 'marker-india' : ''}" style="background-color: ${color}; color: ${color}">
            <div class="arch-marker-inner">
              ${isIndia ? '🏛️' : '🏺'}
            </div>
            <div class="arch-marker-pulse"></div>
          </div>
        `,
        iconSize: [32, 32],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      });

      const marker = L.marker([site.latitude, site.longitude], { icon: customIcon });

      // Custom rich popup
      const unescoBadge = site.isUnescoWorldHeritage
        ? `<div class="badge-unesco">★ UNESCO World Heritage</div>`
        : '';

      const civTags = site.civilizations
        .map(c => `<span class="badge-civ" style="color: ${c.colorHex}; border-color: ${c.colorHex}">${c.civilizationName}</span>`)
        .join('');

      const popupHtml = `
        <div style="padding: 14px 16px; min-width: 250px;">
          ${unescoBadge}
          <h3 style="font-family: 'Cinzel', serif; font-size: 16px; font-weight: 700; color: #f8fafc; margin: 6px 0 2px;">
            ${site.name}
          </h3>
          ${site.ancientName ? `<div style="font-size: 12px; color: #e9c46a; font-style: italic; margin-bottom: 6px;">Ancient: ${site.ancientName}</div>` : ''}
          <div style="font-size: 12px; color: #94a3b8; margin-bottom: 8px;">
            📍 ${site.region}, <strong>${site.country}</strong>
          </div>
          <div style="font-family: 'JetBrains Mono', monospace; font-size: 11.5px; color: #f4a261; background: rgba(244, 162, 97, 0.1); padding: 4px 8px; border-radius: 4px; margin-bottom: 8px; border: 1px solid rgba(244, 162, 97, 0.25);">
            ⏳ ${site.startYearFormatted} – ${site.endYearFormatted}
          </div>
          <div style="margin-bottom: 12px; display: flex; flex-wrap: wrap; gap: 4px;">
            ${civTags}
          </div>
          <button id="explore-btn-${site.id}" style="width: 100%; background: linear-gradient(135deg, #e06a3b, #d4af37); border: none; color: #0f172a; font-weight: 700; font-size: 12px; padding: 7px 12px; border-radius: 6px; cursor: pointer; display: flex; align-items: center; justify-content: center; gap: 6px;">
            Examine Excavation & Artefacts →
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
