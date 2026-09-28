import { Component, EventEmitter, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface HorizonPreset {
  id: string;
  name: string;
  badge: string;
  description: string;
  region?: string;
  search?: string;
  targetYear?: number;
  highlightColor: string;
}

@Component({
  selector: 'app-india-priority-bar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="priority-bar-container">
      <div class="priority-label">
        <span class="flag-icon">🇮🇳</span>
        <span class="priority-title">Indian Archaeology Priority:</span>
      </div>

      <div class="presets-scroll">
        <button
          *ngFor="let p of presets"
          class="preset-chip"
          [class.active]="activePreset() === p.id"
          [style.--chip-color]="p.highlightColor"
          (click)="selectPreset(p)"
        >
          <span class="chip-badge">{{ p.badge }}</span>
          <span class="chip-name">{{ p.name }}</span>
        </button>
      </div>
    </div>
  `,
  styles: [`
    .priority-bar-container {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 8px 18px;
      background: rgba(18, 21, 30, 0.92);
      border-bottom: 1px solid rgba(212, 175, 55, 0.25);
      backdrop-filter: blur(12px);
      z-index: 850;
      overflow-x: auto;
    }

    .priority-label {
      display: flex;
      align-items: center;
      gap: 6px;
      flex-shrink: 0;
    }

    .flag-icon {
      font-size: 16px;
    }

    .priority-title {
      font-family: var(--font-display, serif);
      font-size: 12px;
      font-weight: 700;
      color: #ffd166;
      letter-spacing: 0.05em;
      text-transform: uppercase;
      white-space: nowrap;
    }

    .presets-scroll {
      display: flex;
      align-items: center;
      gap: 8px;
      overflow-x: auto;
      padding-bottom: 2px;
    }

    .preset-chip {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: rgba(255, 255, 255, 0.05);
      border: 1px solid rgba(255, 255, 255, 0.1);
      border-radius: 20px;
      padding: 5px 12px;
      cursor: pointer;
      white-space: nowrap;
      transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .preset-chip:hover {
      background: rgba(212, 175, 55, 0.15);
      border-color: var(--chip-color, #d4af37);
      transform: translateY(-1px);
    }

    .preset-chip.active {
      background: rgba(212, 175, 55, 0.22);
      border-color: var(--chip-color, #ffd166);
      box-shadow: 0 0 12px rgba(212, 175, 55, 0.3);
    }

    .chip-badge {
      font-size: 12px;
    }

    .chip-name {
      font-size: 11.5px;
      font-weight: 600;
      color: #f1f5f9;
    }
  `]
})
export class IndiaPriorityBarComponent {
  @Output() presetSelected = new EventEmitter<HorizonPreset>();

  protected readonly activePreset = signal<string>('all-india');

  protected readonly presets: HorizonPreset[] = [
    {
      id: 'all-india',
      name: 'All Indian Sites',
      badge: '🇮🇳',
      description: 'Comprehensive view of all 12 prioritized Indian archaeological sites across all horizons.',
      region: 'India',
      highlightColor: '#ffd166'
    },
    {
      id: 'indus-valley',
      name: 'Indus / Harappan (Gujarat, Haryana, Rajasthan)',
      badge: '🏺',
      description: 'Dholavira, Lothal, Rakhigarhi, Kalibangan, Surkotada',
      search: 'Harappan',
      targetYear: -2500,
      highlightColor: '#e06a3b'
    },
    {
      id: 'copper-warriors',
      name: 'Copper Age & Sinauli Chariots',
      badge: '⚔️',
      description: 'Sinauli royal warrior burials, 3 solid-wheeled chariots, Inamgaon',
      search: 'Sinauli',
      targetYear: -1900,
      highlightColor: '#b5838d'
    },
    {
      id: 'mauryan-empire',
      name: 'Mauryan Imperial (Pataliputra & Sannati)',
      badge: '👑',
      description: 'Ashokan rock edicts, 80-pillared hypostyle hall, inscribed royal portrait',
      search: 'Mauryan',
      targetYear: -250,
      highlightColor: '#d90429'
    },
    {
      id: 'sangam-maritime',
      name: 'Sangam & Indo-Roman Maritime (Keeladi & Arikamedu)',
      badge: '⛵',
      description: '6th century BCE urban Keeladi with Tamil-Brahmi script, Roman amphorae port',
      search: 'Sangam',
      targetYear: -100,
      highlightColor: '#2a9d8f'
    },
    {
      id: 'rock-art',
      name: 'Paleolithic / Mesolithic Rock Art (Bhimbetka)',
      badge: '🎨',
      description: 'UNESCO World Heritage sandstone rock shelters with hematite paintings',
      search: 'Bhimbetka',
      targetYear: -8000,
      highlightColor: '#6d597a'
    },
    {
      id: 'global-view',
      name: 'Global Ancient Horizons',
      badge: '🌍',
      description: 'Mesopotamia, Nile Valley, Minoan Crete, Classical Rome',
      highlightColor: '#457b9d'
    }
  ];

  selectPreset(preset: HorizonPreset): void {
    this.activePreset.set(preset.id);
    this.presetSelected.emit(preset);
  }
}
