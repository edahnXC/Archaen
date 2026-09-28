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
        <span class="priority-title font-display">Indian Archaeological Horizons:</span>
      </div>

      <div class="presets-scroll">
        <button
          *ngFor="let p of presets"
          class="preset-chip"
          [class.active]="activePreset() === p.id"
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
      gap: 14px;
      padding: 8px 24px;
      background: rgba(255, 255, 255, 0.94);
      border-bottom: 1px solid rgba(0, 0, 0, 0.08);
      backdrop-filter: blur(16px);
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
      font-size: 11.5px;
      font-weight: 700;
      color: var(--accent-terracotta);
      letter-spacing: 0.06em;
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
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      border-radius: 20px;
      padding: 5px 14px;
      cursor: pointer;
      white-space: nowrap;
      transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .preset-chip:hover {
      background: #e5e7eb;
      border-color: #cbd5e1;
      transform: translateY(-1px);
    }

    .preset-chip.active {
      background: #111827;
      border-color: #111827;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }

    .preset-chip.active .chip-name {
      color: #ffffff;
    }

    .chip-badge {
      font-size: 13px;
    }

    .chip-name {
      font-size: 11.5px;
      font-weight: 600;
      color: #374151;
      transition: color 0.2s ease;
    }
  `]
})
export class IndiaPriorityBarComponent {
  @Output() presetSelected = new EventEmitter<HorizonPreset>();

  protected readonly activePreset = signal<string>('all-india');

  protected readonly presets: HorizonPreset[] = [
    {
      id: 'all-india',
      name: 'All Indian Sites (12)',
      badge: '🇮🇳',
      description: 'Comprehensive view of all 12 prioritized Indian archaeological sites across all horizons.',
      region: 'India',
      highlightColor: '#c25e2e'
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
      highlightColor: '#9d4edd'
    },
    {
      id: 'mauryan-empire',
      name: 'Mauryan Imperial (Pataliputra & Sannati)',
      badge: '👑',
      description: 'Ashokan rock edicts, 80-pillared hypostyle hall, inscribed royal portrait',
      search: 'Mauryan',
      targetYear: -250,
      highlightColor: '#b91c1c'
    },
    {
      id: 'sangam-maritime',
      name: 'Sangam & Indo-Roman Maritime (Keeladi & Arikamedu)',
      badge: '⛵',
      description: '6th century BCE urban Keeladi with Tamil-Brahmi script, Roman amphorae port',
      search: 'Sangam',
      targetYear: -100,
      highlightColor: '#0f766e'
    },
    {
      id: 'rock-art',
      name: 'Paleolithic / Mesolithic Rock Art (Bhimbetka)',
      badge: '🎨',
      description: 'UNESCO World Heritage sandstone rock shelters with hematite paintings',
      search: 'Bhimbetka',
      targetYear: -8000,
      highlightColor: '#c25e2e'
    },
    {
      id: 'global-view',
      name: 'Global Ancient Horizons',
      badge: '🌍',
      description: 'Mesopotamia, Nile Valley, Minoan Crete, Classical Rome',
      highlightColor: '#1d4ed8'
    }
  ];

  selectPreset(preset: HorizonPreset): void {
    this.activePreset.set(preset.id);
    this.presetSelected.emit(preset);
  }
}
