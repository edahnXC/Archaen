import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  signal,
  computed
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface HorizonKeyframe {
  year: number;
  label: string;
  civilization: string;
}

export interface SynchronousRegion {
  regionName: string;
  flagOrIcon: string;
  settlements: string;
  developments: string;
}

export interface SynchronousHorizon {
  year: number;
  eraName: string;
  panoramicTitle: string;
  panoramicSummary: string;
  regions: SynchronousRegion[];
}

@Component({
  selector: 'app-timeline-scrubber',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="time-machine-hud-container">
      <!-- Synchronous Ancient Horizons Panoramic Dossier (Collapsible Panoramic Matrix) -->
      <div class="sync-horizons-card" *ngIf="isHorizonsOpen() && activeHorizon() as hz">
        <div class="sync-horizons-header">
          <div class="hz-badge-title">
            <span class="hz-globe-icon">🌐</span>
            <div>
              <span class="hz-eyebrow font-mono">SYNCHRONOUS ANCIENT HORIZONS • CALIBRATED {{ formatYear(hz.year) }}</span>
              <h4 class="hz-title font-display">{{ hz.panoramicTitle }}</h4>
            </div>
          </div>
          <button type="button" class="hz-close-btn" (click)="toggleHorizons()" title="Close Panoramic Dossier">✕</button>
        </div>

        <p class="hz-summary">{{ hz.panoramicSummary }}</p>

        <div class="hz-regions-grid">
          <div class="hz-region-col" *ngFor="let reg of hz.regions">
            <div class="reg-head">
              <span class="reg-flag">{{ reg.flagOrIcon }}</span>
              <span class="reg-name font-display">{{ reg.regionName }}</span>
            </div>
            <div class="reg-settlements font-mono">{{ reg.settlements }}</div>
            <p class="reg-dev">{{ reg.developments }}</p>
          </div>
        </div>
      </div>

      <div class="time-machine-floating-pill">
        <!-- Year Readout (Google Sans Display Typography) -->
        <div class="year-readout-col">
          <div class="year-badge-top">Temporal Horizon</div>
          <div class="current-year-display font-display" [class.bce]="currentYear() < 0" [class.ce]="currentYear() >= 0">
            {{ formattedCurrentYear() }}
          </div>
          <div class="epoch-label">{{ activeEpoch() }}</div>
          <button
            type="button"
            class="sync-horizons-toggle-btn font-mono"
            [class.active]="isHorizonsOpen()"
            (click)="toggleHorizons()"
            title="Inspect Synchronous Ancient Horizons across continents"
          >
            <span>🌐 Synchronous Horizons</span>
            <span class="hz-arrow">{{ isHorizonsOpen() ? '▲' : '▼' }}</span>
          </button>
        </div>

        <!-- Playback Controls (Zoom Earth Player Inspired) -->
        <div class="player-controls-group">
          <button class="step-btn" (click)="stepYear(-100)" title="Step -100 Years">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polyline points="11 17 6 12 11 7"/><polyline points="18 17 13 12 18 7"/>
            </svg>
          </button>

          <button class="play-pause-circle" [class.playing]="isPlaying()" (click)="togglePlay()" title="Auto Timeline Playback">
            <svg *ngIf="!isPlaying()" width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <polygon points="5 3 19 12 5 21 5 3"/>
            </svg>
            <svg *ngIf="isPlaying()" width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <rect x="6" y="4" width="4" height="16"/><rect x="14" y="4" width="4" height="16"/>
            </svg>
          </button>

          <button class="step-btn" (click)="stepYear(100)" title="Step +100 Years">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polyline points="13 17 18 12 13 7"/><polyline points="6 17 11 12 6 7"/>
            </svg>
          </button>

          <!-- Speed Pill -->
          <button class="speed-pill" (click)="cycleSpeed()" title="Playback Speed Multiplier">
            {{ playbackSpeed() }}x
          </button>
        </div>

        <!-- Timeline Scrubber Track & Milestone Pills -->
        <div class="scrubber-track-area">
          <div class="slider-wrapper">
            <input
              type="range"
              class="zoom-time-slider"
              [min]="minYear"
              [max]="maxYear"
              [step]="25"
              [ngModel]="currentYear()"
              (ngModelChange)="onSliderInput($event)"
            />
          </div>

          <!-- Landmark Milestone Horizon Chips -->
          <div class="milestones-row">
            <button
              *ngFor="let kf of keyframes"
              class="ms-chip"
              [class.active]="isNearKeyframe(kf.year)"
              (click)="jumpToYear(kf.year)"
            >
              <span class="ms-year font-mono">{{ formatYear(kf.year) }}</span>
              <span class="ms-name">{{ kf.label }}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .time-machine-hud-container {
      position: absolute;
      bottom: 20px;
      left: 50%;
      transform: translateX(-50%);
      z-index: 850;
      width: calc(100% - 360px);
      max-width: 880px;
      pointer-events: none;
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    @media (max-width: 1200px) {
      .time-machine-hud-container {
        width: calc(100% - 48px);
        max-width: 820px;
      }
    }

    /* Synchronous Ancient Horizons Panoramic Card */
    .sync-horizons-card {
      pointer-events: auto;
      background: rgba(255, 255, 255, 0.98);
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 18px;
      padding: 14px 18px;
      box-shadow: 0 16px 40px rgba(0, 0, 0, 0.16);
      backdrop-filter: blur(20px);
      animation: slideUpFade 0.25s cubic-bezier(0.16, 1, 0.3, 1);
    }

    @keyframes slideUpFade {
      from { opacity: 0; transform: translateY(12px); }
      to { opacity: 1; transform: translateY(0); }
    }

    .sync-horizons-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-bottom: 6px;
    }

    .hz-badge-title {
      display: flex;
      align-items: center;
      gap: 10px;
    }

    .hz-globe-icon {
      font-size: 20px;
    }

    .hz-eyebrow {
      font-size: 9px;
      font-weight: 800;
      color: #0f766e;
      letter-spacing: 0.08em;
      text-transform: uppercase;
    }

    .hz-title {
      font-size: 13.5px;
      font-weight: 700;
      color: #111827;
      margin: 0;
    }

    .hz-close-btn {
      background: #f1f5f9;
      border: none;
      border-radius: 50%;
      width: 24px;
      height: 24px;
      font-size: 11px;
      cursor: pointer;
      color: #64748b;
      transition: all 0.2s ease;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .hz-close-btn:hover {
      background: #111827;
      color: #ffffff;
    }

    .hz-summary {
      font-size: 11px;
      color: #4b5563;
      line-height: 1.45;
      margin: 0 0 10px 0;
    }

    .hz-regions-grid {
      display: grid;
      grid-template-columns: repeat(4, 1fr);
      gap: 8px;
    }

    @media (max-width: 900px) {
      .hz-regions-grid {
        grid-template-columns: repeat(2, 1fr);
      }
    }

    .hz-region-col {
      background: #f8fafc;
      border: 1px solid #e2e8f0;
      border-radius: 10px;
      padding: 8px 10px;
    }

    .reg-head {
      display: flex;
      align-items: center;
      gap: 5px;
      margin-bottom: 2px;
    }

    .reg-name {
      font-size: 11px;
      font-weight: 700;
      color: #0f172a;
    }

    .reg-settlements {
      font-size: 9.5px;
      color: var(--accent-terracotta, #c25e2e);
      font-weight: 600;
      margin-bottom: 4px;
    }

    .reg-dev {
      font-size: 10px;
      color: #475569;
      line-height: 1.35;
      margin: 0;
    }

    .sync-horizons-toggle-btn {
      display: inline-flex;
      align-items: center;
      gap: 4px;
      background: #f0fdf4;
      border: 1px solid #bbf7d0;
      border-radius: 10px;
      padding: 3px 8px;
      font-size: 9.5px;
      font-weight: 700;
      color: #166534;
      cursor: pointer;
      margin-top: 4px;
      transition: all 0.2s ease;
    }

    .sync-horizons-toggle-btn:hover, .sync-horizons-toggle-btn.active {
      background: #166534;
      color: #ffffff;
      border-color: #166534;
    }

    .time-machine-floating-pill {
      pointer-events: auto;
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 10px 20px;
      background: rgba(255, 255, 255, 0.95);
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 20px;
      backdrop-filter: blur(20px);
      box-shadow: 0 14px 40px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.04);
    }

    .year-readout-col {
      min-width: 160px;
      display: flex;
      flex-direction: column;
      border-right: 1px solid rgba(0, 0, 0, 0.08);
      padding-right: 14px;
    }

    .year-badge-top {
      font-size: 9.5px;
      text-transform: uppercase;
      letter-spacing: 0.08em;
      color: #6b7280;
      font-weight: 700;
    }

    .current-year-display {
      font-size: 22px;
      font-weight: 700;
      line-height: 1.1;
      margin: 2px 0;
    }

    .current-year-display.bce {
      color: var(--accent-terracotta);
    }

    .current-year-display.ce {
      color: var(--accent-emerald);
    }

    .epoch-label {
      font-size: 10px;
      color: #4b5563;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      font-weight: 500;
    }

    .player-controls-group {
      display: flex;
      align-items: center;
      gap: 6px;
    }

    .step-btn {
      width: 30px;
      height: 30px;
      border-radius: 50%;
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      color: #374151;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .step-btn:hover {
      background: #111827;
      color: #ffffff;
    }

    .play-pause-circle {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: #111827;
      border: none;
      color: #ffffff;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      box-shadow: 0 4px 14px rgba(0, 0, 0, 0.2);
      transition: all 0.2s ease;
    }

    .play-pause-circle:hover {
      transform: scale(1.08);
      background: var(--accent-terracotta);
    }

    .play-pause-circle.playing {
      background: var(--accent-emerald);
    }

    .speed-pill {
      font-family: var(--font-mono);
      font-size: 10.5px;
      font-weight: 700;
      color: #374151;
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      padding: 4px 8px;
      border-radius: 10px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .speed-pill:hover {
      background: #e5e7eb;
      color: #111827;
    }

    .scrubber-track-area {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 6px;
    }

    .slider-wrapper {
      position: relative;
      width: 100%;
    }

    .zoom-time-slider {
      -webkit-appearance: none;
      appearance: none;
      width: 100%;
      height: 7px;
      border-radius: 10px;
      background: linear-gradient(90deg, #c25e2e 0%, #e06a3b 35%, #b5838d 60%, #b91c1c 75%, #0f766e 88%, #1d4ed8 100%);
      outline: none;
      cursor: pointer;
      box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.1);
    }

    .zoom-time-slider::-webkit-slider-thumb {
      -webkit-appearance: none;
      appearance: none;
      width: 20px;
      height: 20px;
      border-radius: 50%;
      background: #ffffff;
      border: 3px solid #111827;
      cursor: pointer;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.25);
      transition: transform 0.15s ease;
    }

    .zoom-time-slider::-webkit-slider-thumb:hover {
      transform: scale(1.2);
      border-color: var(--accent-terracotta);
    }

    .milestones-row {
      display: flex;
      justify-content: space-between;
      gap: 6px;
      overflow-x: auto;
    }

    .ms-chip {
      background: #f8f9fa;
      border: 1px solid #e9ecef;
      border-radius: 6px;
      padding: 2px 7px;
      cursor: pointer;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      transition: all 0.2s ease;
      white-space: nowrap;
    }

    .ms-chip:hover, .ms-chip.active {
      background: #fef8e7;
      border-color: #d4a373;
    }

    .ms-year {
      font-size: 9.5px;
      font-weight: 700;
      color: var(--accent-terracotta);
    }

    .ms-name {
      font-size: 9px;
      color: #6b7280;
    }
  `]
})
export class TimelineScrubberComponent implements OnInit, OnDestroy {
  @Input() initialYear = -2500;
  @Output() yearChanged = new EventEmitter<number>();

  protected readonly minYear = -3500;
  protected readonly maxYear = 500;

  protected readonly currentYear = signal(-2500);
  protected readonly isPlaying = signal(false);
  protected readonly playbackSpeed = signal(1);
  public readonly isHorizonsOpen = signal(false);

  private playbackTimerId: any = null;

  toggleHorizons(): void {
    this.isHorizonsOpen.update(v => !v);
  }

  protected readonly keyframes: HorizonKeyframe[] = [
    { year: -3300, label: 'Early Indus / Mehrgarh', civilization: 'Early Harappan' },
    { year: -2500, label: 'Peak Mature Indus (Dholavira/Lothal)', civilization: 'Mature Harappan' },
    { year: -1900, label: 'Sinauli Chariots & Copper Age', civilization: 'Copper Hoard' },
    { year: -1000, label: 'Painted Grey Ware & Iron Dawn', civilization: 'Vedic PGW' },
    { year: -580, label: 'Sangam Keeladi & Second Urbanization', civilization: 'Sangam / NBPW' },
    { year: -250, label: 'Ashokan Mauryan Empire (Sannati)', civilization: 'Mauryan' },
    { year: 50, label: 'Indo-Roman Global Maritime Trade', civilization: 'Indo-Roman' }
  ];

  protected readonly synchronousHorizons: SynchronousHorizon[] = [
    {
      year: -3300,
      eraName: 'Early Bronze & Urban Genesis',
      panoramicTitle: 'Formative Agro-Urban Genesis Across Continents',
      panoramicSummary: 'The threshold of human civilization: permanent brick settlements, wheel-thrown painted ceramics, and the earliest pre-cuneiform and proto-Indus script experiments emerge in riverine valleys.',
      regions: [
        {
          regionName: 'Indus Valley',
          flagOrIcon: '🇮🇳',
          settlements: 'Mehrgarh VII & Ravi Phase',
          developments: 'Mudbrick communal granaries, wheel-thrown Hakra ware, bone tools, and proto-script potter marks incised before kilning.'
        },
        {
          regionName: 'Mesopotamia',
          flagOrIcon: '🏛️',
          settlements: 'Uruk (Eanna District)',
          developments: 'Invention of pictographic cuneiform on clay bullae; limestone temple complexes; monumental mudbrick ziggurats.'
        },
        {
          regionName: 'Nile Valley',
          flagOrIcon: '🇪🇬',
          settlements: 'Naqada III & Abydos',
          developments: 'Protodynastic state formation; royal serekhs of King Narmer; early hieroglyphic labels and unification wars.'
        },
        {
          regionName: 'Atlantic Europe',
          flagOrIcon: '🗿',
          settlements: 'Stonehenge Phase 1',
          developments: 'Construction of the circular ditch and chalk embankment using red deer antler picks; cremation deposits.'
        }
      ]
    },
    {
      year: -2500,
      eraName: 'Peak Mature Bronze Metropolises',
      panoramicTitle: 'Apex of Planned Bronze Age Cities & Monumental Architecture',
      panoramicSummary: 'A golden epoch of urban geometry: rectilinear planned metropolises, trans-oceanic Gulf trade routes, monumental stone pyramids, and royal sumptuary tombs.',
      regions: [
        {
          regionName: 'Indus Valley',
          flagOrIcon: '🇮🇳',
          settlements: 'Mohenjo-daro, Dholavira, Lothal',
          developments: 'Great Bath, tripartite rock-cut reservoirs, baked-brick tidal dockyard, standardized chert weights, and unicorn intaglio seals.'
        },
        {
          regionName: 'Old Kingdom Egypt',
          flagOrIcon: '🇪🇬',
          settlements: 'Giza Plateau & Memphis',
          developments: 'Pharaohs Khufu, Khafre, and Menkaure erect the Great Pyramids and Sphinx with high-precision ashlar limestone.'
        },
        {
          regionName: 'Sumer / Mesopotamia',
          flagOrIcon: '🏛️',
          settlements: 'Ur (Royal Cemetery)',
          developments: 'Royal tombs of Queen Puabi yielding gold lunate diadems, lapis lazuli bull lyres, and the mosaic Standard of Ur.'
        },
        {
          regionName: 'Megalithic Britain',
          flagOrIcon: '🗿',
          settlements: 'Stonehenge Phase 3',
          developments: 'Erection of the massive sarsen trilithons and transport of Preseli bluestones from Wales aligned to solar solstices.'
        }
      ]
    },
    {
      year: -1900,
      eraName: 'Martial Copper Age & Transitional Shifts',
      panoramicTitle: 'The Warrior Aristocracy & Trans-Continental Bronze Shifts',
      panoramicSummary: 'Environmental shifts cause river course alterations while horse/chariot martial regalia, arsenical copper hoards, and early palatial Aegean centers transform ancient society.',
      regions: [
        {
          regionName: 'Ganga-Yamuna Doab',
          flagOrIcon: '🇮🇳',
          settlements: 'Sinauli Necropolis',
          developments: 'Royal solid-wheeled war chariots with copper triangle mounts, antennae swords with wire hilts, and warrior coffins.'
        },
        {
          regionName: 'Babylonia',
          flagOrIcon: '🏛️',
          settlements: 'Babylon & Isin',
          developments: 'Rise of the First Dynasty of Babylon; legal codification of Hammurabi on diorite stele; mathematical algebra tablets.'
        },
        {
          regionName: 'Middle Kingdom Egypt',
          flagOrIcon: '🇪🇬',
          settlements: 'Thebes & Kahun',
          developments: 'Classical Middle Kingdom renaissance; construction of Senusret III fortresses along the second cataract of the Nile.'
        },
        {
          regionName: 'Aegean Sea',
          flagOrIcon: '🏺',
          settlements: 'Knossos (Crete)',
          developments: 'First Protopalatial monumental complex at Knossos with central courtyards, ceramic workshops, and early Linear A script.'
        }
      ]
    },
    {
      year: -1000,
      eraName: 'Early Iron Age & Nubian Awakening',
      panoramicTitle: 'Iron Metallurgy Dawn & The Rise of the Kushite Empire',
      panoramicSummary: 'The Bronze Age Collapse transitions into the widespread adoption of iron smelting, agricultural intensification, and the emergence of independent Nubian kingship.',
      regions: [
        {
          regionName: 'Northern India',
          flagOrIcon: '🇮🇳',
          settlements: 'Hastinapur, Ahichchhatra, Inamgaon',
          developments: 'Painted Grey Ware (PGW) culture; early iron agricultural ploughshares; Late Jorwe spouted ware pottery in the Deccan.'
        },
        {
          regionName: 'Nubia / Sudan',
          flagOrIcon: '🔺',
          settlements: 'Napata & Kurru',
          developments: 'Foundation of the Kingdom of Kush; Nubian kings prepare for the conquest of Egypt to establish the 25th Dynasty.'
        },
        {
          regionName: 'Levant & Mediterranean',
          flagOrIcon: '⛵',
          settlements: 'Tyre, Sidon, Byblos',
          developments: 'Phoenician maritime seafaring expansion; transmission of the 22-letter phonetic alphabet throughout the Mediterranean.'
        },
        {
          regionName: 'Mesopotamia',
          flagOrIcon: '🏛️',
          settlements: 'Nimrud & Nineveh',
          developments: 'Rise of the Neo-Assyrian military empire; monumental palace wall reliefs carving royal lion hunts and iron weaponry.'
        }
      ]
    },
    {
      year: -580,
      eraName: 'Second Urbanization & Sangam Dawn',
      panoramicTitle: 'Literate Urban Renaissance: Sangam South & Gangetic Mahajanapadas',
      panoramicSummary: 'Subcontinental second urbanization: literate civic settlements on the Vaigai, universal coinage, and intellectual revolutions spanning from Magadha to Athens and Babylon.',
      regions: [
        {
          regionName: 'South India',
          flagOrIcon: '🇮🇳',
          settlements: 'Keeladi (Vaigai Valley)',
          developments: 'Urban brick residences, ring wells, carnelian lapidary crafts, and early Tamil-Brahmi literacy dating to 580 BCE.'
        },
        {
          regionName: 'Gangetic Basin',
          flagOrIcon: '🪙',
          settlements: 'Pataliputra, Rajgir, Varanasi',
          developments: 'Sixteen Mahajanapadas; punch-marked silver coins (karshapanas); birth of Buddhism and Jainism philosophical traditions.'
        },
        {
          regionName: 'Persian Empire',
          flagOrIcon: '👑',
          settlements: 'Pasargadae & Persepolis',
          developments: 'Cyrus the Great unifies the Achaemenid Empire; Persian Royal Road; imperial satrapies incorporating the Indus valley.'
        },
        {
          regionName: 'Classical Greece',
          flagOrIcon: '🏛️',
          settlements: 'Athens & Corinth',
          developments: 'Archaic to Classical transition; Solonian constitutional reforms, black-figure pottery, and early polis civic assemblies.'
        }
      ]
    },
    {
      year: -250,
      eraName: 'Imperial Horizons & Ashokan Dharma',
      panoramicTitle: 'Imperial Subcontinental Unification & The Hellenistic World',
      panoramicSummary: 'Emperor Ashoka unites the Indian subcontinent under ethical rock and pillar edicts, while the Hellenistic successor states and Nabataean trade networks connect East and West.',
      regions: [
        {
          regionName: 'Mauryan India',
          flagOrIcon: '🇮🇳',
          settlements: 'Pataliputra, Sannati, Dhauli',
          developments: 'Major Rock & Pillar Edicts inscribed in Brahmi, Greek, and Aramaic; 80-pillared hypostyle hall; Ashokan portrait relief at Sannati.'
        },
        {
          regionName: 'Arabia / Levant',
          flagOrIcon: '🏛️',
          settlements: 'Petra (Jordan)',
          developments: 'Nabataean Kingdom thrives on the incense route; monumentally carved Hellenistic cliff façades and pressurized water conduits.'
        },
        {
          regionName: 'Ptolemaic Egypt',
          flagOrIcon: '🇪🇬',
          settlements: 'Alexandria',
          developments: 'The Great Library of Alexandria, Pharos Lighthouse, and Greek-Egyptian syncretism under Ptolemy II Philadelphus.'
        },
        {
          regionName: 'Mediterranean',
          flagOrIcon: '⚔️',
          settlements: 'Rome & Carthage',
          developments: 'First Punic War; Roman naval expansion throughout the western Mediterranean basin.'
        }
      ]
    },
    {
      year: 50,
      eraName: 'Classical Imperial Maritime Silk Road',
      panoramicTitle: 'Global Trans-Oceanic Commerce & Classical High Empires',
      panoramicSummary: 'Monsoon trade winds connect the Roman Empire directly to Southern Indian ports, exchanging Roman gold and Mediterranean wine for Indian spices, silk, and jewels.',
      regions: [
        {
          regionName: 'Coromandel & Malabar',
          flagOrIcon: '🇮🇳',
          settlements: 'Arikamedu, Muziris, Keeladi',
          developments: 'Direct Indo-Roman maritime trade; imported Roman amphorae, terra sigillata ware, gold coins, and raw beryls export.'
        },
        {
          regionName: 'Roman Empire',
          flagOrIcon: '🏛️',
          settlements: 'Rome & Pompeii',
          developments: 'Construction and dedication of the Colosseum (80 CE); Pompeii sealed by Mount Vesuvius preserving an Indian ivory Lakshmi statuette.'
        },
        {
          regionName: 'Kingdom of Kush',
          flagOrIcon: '🔺',
          settlements: 'Meroë (Sudan)',
          developments: 'Golden age of Candace queens (Amanishakheto); steep-angled royal pyramids; thriving sub-Saharan iron blast furnaces.'
        },
        {
          regionName: 'Nabataea / Roman East',
          flagOrIcon: '🏺',
          settlements: 'Petra & Palmyra',
          developments: 'Peak of Nabataean rock-cut architecture (Al-Khazneh) before formal annexation into the Roman province of Arabia Petraea.'
        }
      ]
    }
  ];

  protected readonly activeHorizon = computed<SynchronousHorizon>(() => {
    const y = this.currentYear();
    let best = this.synchronousHorizons[0];
    let minDiff = Infinity;
    for (const hz of this.synchronousHorizons) {
      const diff = Math.abs(y - hz.year);
      if (diff < minDiff) {
        minDiff = diff;
        best = hz;
      }
    }
    return best;
  });

  ngOnInit(): void {
    this.currentYear.set(this.initialYear);
  }

  ngOnDestroy(): void {
    this.stopPlayback();
  }

  onSliderInput(newYear: number): void {
    this.currentYear.set(Number(newYear));
    this.yearChanged.emit(this.currentYear());
  }

  stepYear(delta: number): void {
    const target = Math.min(this.maxYear, Math.max(this.minYear, this.currentYear() + delta));
    this.currentYear.set(target);
    this.yearChanged.emit(target);
  }

  jumpToYear(year: number): void {
    this.currentYear.set(year);
    this.yearChanged.emit(year);
  }

  togglePlay(): void {
    if (this.isPlaying()) {
      this.stopPlayback();
    } else {
      this.startPlayback();
    }
  }

  cycleSpeed(): void {
    const speeds = [1, 2, 5, 10];
    const currentIndex = speeds.indexOf(this.playbackSpeed());
    const nextSpeed = speeds[(currentIndex + 1) % speeds.length];
    this.playbackSpeed.set(nextSpeed);

    if (this.isPlaying()) {
      this.stopPlayback();
      this.startPlayback();
    }
  }

  isNearKeyframe(year: number): boolean {
    return Math.abs(this.currentYear() - year) <= 75;
  }

  formattedCurrentYear(): string {
    return this.formatYear(this.currentYear());
  }

  formatYear(year: number): string {
    if (year < 0) {
      return `${Math.abs(year)} BCE`;
    } else if (year === 0) {
      return `1 BCE / 1 CE`;
    } else {
      return `${year} CE`;
    }
  }

  activeEpoch(): string {
    const y = this.currentYear();
    if (y <= -2600) return 'Early Urban & Mature Indus Integration';
    if (y <= -1900) return 'Mature Harappan Metropolis Era';
    if (y <= -1400) return 'Copper Hoard Warrior Horizon (Sinauli)';
    if (y <= -600) return 'Early Iron Age & Painted Grey Ware';
    if (y <= -300) return 'Second Urbanization & Sangam Dawn (Keeladi)';
    if (y <= -185) return 'Mauryan Imperial Horizon (Ashokan Edicts)';
    return 'Classical Antiquity & Indo-Roman Maritime Horizon';
  }

  private startPlayback(): void {
    this.isPlaying.set(true);
    const intervalMs = Math.max(80, 280 / this.playbackSpeed());
    const stepSize = 25;

    this.playbackTimerId = setInterval(() => {
      let nextYear = this.currentYear() + stepSize;
      if (nextYear > this.maxYear) {
        nextYear = this.minYear;
      }
      this.currentYear.set(nextYear);
      this.yearChanged.emit(nextYear);
    }, intervalMs);
  }

  private stopPlayback(): void {
    this.isPlaying.set(false);
    if (this.playbackTimerId) {
      clearInterval(this.playbackTimerId);
      this.playbackTimerId = null;
    }
  }
}
