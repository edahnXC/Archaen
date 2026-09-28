import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface HorizonKeyframe {
  year: number;
  label: string;
  civilization: string;
}

@Component({
  selector: 'app-timeline-scrubber',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="time-machine-bar">
      <!-- Year Counter & Playback Indicator -->
      <div class="time-readout-section">
        <div class="time-label">Active Horizon</div>
        <div class="current-year font-mono" [class.bce]="currentYear() < 0" [class.ce]="currentYear() >= 0">
          {{ formattedCurrentYear() }}
        </div>
        <div class="epoch-descriptor">{{ activeEpoch() }}</div>
      </div>

      <!-- Main Controls (Play/Pause, Step, Speed) -->
      <div class="playback-controls">
        <button class="icon-btn" (click)="stepYear(-100)" title="Step -100 Years">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <polyline points="11 17 6 12 11 7"/><polyline points="18 17 13 12 18 7"/>
          </svg>
        </button>

        <button class="play-btn" [class.playing]="isPlaying()" (click)="togglePlay()" title="Toggle Temporal Playback">
          <svg *ngIf="!isPlaying()" width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
            <polygon points="5 3 19 12 5 21 5 3"/>
          </svg>
          <svg *ngIf="isPlaying()" width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
            <rect x="6" y="4" width="4" height="16"/><rect x="14" y="4" width="4" height="16"/>
          </svg>
        </button>

        <button class="icon-btn" (click)="stepYear(100)" title="Step +100 Years">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <polyline points="13 17 18 12 13 7"/><polyline points="6 17 11 12 6 7"/>
          </svg>
        </button>

        <!-- Playback Speed Multiplier -->
        <button class="speed-badge" (click)="cycleSpeed()" title="Playback Speed Multiplier">
          {{ playbackSpeed() }}x
        </button>
      </div>

      <!-- Scrubber Slider & Historical Milestones -->
      <div class="slider-track-container">
        <div class="slider-wrapper">
          <input
            type="range"
            class="time-range-slider"
            [min]="minYear"
            [max]="maxYear"
            [step]="25"
            [ngModel]="currentYear()"
            (ngModelChange)="onSliderInput($event)"
          />
        </div>

        <!-- Key Historical Horizons -->
        <div class="milestones-bar">
          <button
            *ngFor="let kf of keyframes"
            class="milestone-chip"
            [class.active]="isNearKeyframe(kf.year)"
            (click)="jumpToYear(kf.year)"
          >
            <span class="ms-year">{{ formatYear(kf.year) }}</span>
            <span class="ms-label">{{ kf.label }}</span>
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .time-machine-bar {
      display: flex;
      align-items: center;
      gap: 20px;
      padding: 12px 24px;
      background: rgba(14, 16, 22, 0.94);
      border-top: 1px solid rgba(212, 175, 55, 0.28);
      backdrop-filter: blur(16px);
      box-shadow: 0 -8px 30px rgba(0, 0, 0, 0.7);
      width: 100%;
      height: 90px;
      z-index: 900;
    }

    .time-readout-section {
      min-width: 145px;
      display: flex;
      flex-direction: column;
      justify-content: center;
      border-right: 1px solid rgba(255, 255, 255, 0.08);
      padding-right: 18px;
    }

    .time-label {
      font-size: 10px;
      text-transform: uppercase;
      letter-spacing: 0.1em;
      color: #94a3b8;
      font-weight: 600;
    }

    .current-year {
      font-size: 22px;
      font-weight: 800;
      letter-spacing: -0.02em;
      line-height: 1.1;
      margin: 2px 0;
    }

    .current-year.bce {
      color: #f4a261;
      text-shadow: 0 0 16px rgba(244, 162, 97, 0.4);
    }

    .current-year.ce {
      color: #48cae4;
      text-shadow: 0 0 16px rgba(72, 202, 228, 0.4);
    }

    .epoch-descriptor {
      font-size: 11px;
      color: #cbd5e1;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      font-weight: 500;
    }

    .playback-controls {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .icon-btn {
      width: 32px;
      height: 32px;
      border-radius: 8px;
      background: rgba(255, 255, 255, 0.06);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: #cbd5e1;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .icon-btn:hover {
      background: rgba(212, 175, 55, 0.2);
      border-color: #d4af37;
      color: #ffd166;
    }

    .play-btn {
      width: 42px;
      height: 42px;
      border-radius: 50%;
      background: linear-gradient(135deg, #e06a3b, #d4af37);
      border: none;
      color: #0c0e14;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      box-shadow: 0 0 18px rgba(224, 106, 59, 0.4);
      transition: all 0.2s ease;
    }

    .play-btn:hover {
      transform: scale(1.08);
      box-shadow: 0 0 25px rgba(212, 175, 55, 0.6);
    }

    .play-btn.playing {
      background: linear-gradient(135deg, #2a9d8f, #48cae4);
      box-shadow: 0 0 20px rgba(42, 157, 143, 0.5);
    }

    .speed-badge {
      font-family: var(--font-mono, monospace);
      font-size: 11px;
      font-weight: 700;
      color: #e9c46a;
      background: rgba(233, 196, 106, 0.12);
      border: 1px solid rgba(233, 196, 106, 0.3);
      padding: 5px 8px;
      border-radius: 6px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .speed-badge:hover {
      background: rgba(233, 196, 106, 0.25);
    }

    .slider-track-container {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 6px;
    }

    .slider-wrapper {
      position: relative;
      width: 100%;
    }

    .time-range-slider {
      -webkit-appearance: none;
      appearance: none;
      width: 100%;
      height: 8px;
      border-radius: 4px;
      background: linear-gradient(90deg, #6d597a 0%, #e06a3b 35%, #b5838d 60%, #d90429 75%, #2a9d8f 88%, #457b9d 100%);
      outline: none;
      cursor: pointer;
      box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.6);
    }

    .time-range-slider::-webkit-slider-thumb {
      -webkit-appearance: none;
      appearance: none;
      width: 22px;
      height: 22px;
      border-radius: 50%;
      background: #f8fafc;
      border: 3px solid #d4af37;
      cursor: pointer;
      box-shadow: 0 0 12px rgba(212, 175, 55, 0.8);
      transition: transform 0.15s ease;
    }

    .time-range-slider::-webkit-slider-thumb:hover {
      transform: scale(1.2);
    }

    .milestones-bar {
      display: flex;
      justify-content: space-between;
      gap: 6px;
      overflow-x: auto;
    }

    .milestone-chip {
      background: rgba(255, 255, 255, 0.05);
      border: 1px solid rgba(255, 255, 255, 0.08);
      border-radius: 4px;
      padding: 3px 8px;
      cursor: pointer;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      transition: all 0.2s ease;
      white-space: nowrap;
    }

    .milestone-chip:hover, .milestone-chip.active {
      background: rgba(212, 175, 55, 0.18);
      border-color: #d4af37;
    }

    .ms-year {
      font-family: var(--font-mono, monospace);
      font-size: 10px;
      font-weight: 700;
      color: #ffd166;
    }

    .ms-label {
      font-size: 9.5px;
      color: #94a3b8;
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

  private playbackTimerId: any = null;

  // Curated landmark historical milestones focusing on Indian Archaeological Horizons
  protected readonly keyframes: HorizonKeyframe[] = [
    { year: -3300, label: 'Early Indus / Mehrgarh', civilization: 'Early Harappan' },
    { year: -2500, label: 'Peak Mature Indus (Dholavira/Lothal)', civilization: 'Mature Harappan' },
    { year: -1900, label: 'Sinauli Chariots & Copper Age', civilization: 'Copper Hoard' },
    { year: -1000, label: 'Painted Grey Ware & Iron Dawn', civilization: 'Vedic PGW' },
    { year: -580, label: 'Sangam Keeladi & Second Urbanization', civilization: 'Sangam / NBPW' },
    { year: -250, label: 'Ashokan Mauryan Empire (Sannati)', civilization: 'Mauryan' },
    { year: 50, label: 'Indo-Roman Global Maritime Trade', civilization: 'Indo-Roman' }
  ];

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
