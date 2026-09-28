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
    <div class="time-machine-hud-container">
      <div class="time-machine-floating-pill">
        <!-- Year Readout (Google Sans Display Typography) -->
        <div class="year-readout-col">
          <div class="year-badge-top">Temporal Horizon</div>
          <div class="current-year-display font-display" [class.bce]="currentYear() < 0" [class.ce]="currentYear() >= 0">
            {{ formattedCurrentYear() }}
          </div>
          <div class="epoch-label">{{ activeEpoch() }}</div>
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
      bottom: 24px;
      left: 50%;
      transform: translateX(-50%);
      z-index: 850;
      width: calc(100% - 48px);
      max-width: 1080px;
      pointer-events: none;
    }

    .time-machine-floating-pill {
      pointer-events: auto;
      display: flex;
      align-items: center;
      gap: 20px;
      padding: 12px 24px;
      background: rgba(255, 255, 255, 0.94);
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 24px;
      backdrop-filter: blur(20px);
      box-shadow: 0 14px 40px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.04);
    }

    .year-readout-col {
      min-width: 155px;
      display: flex;
      flex-direction: column;
      border-right: 1px solid rgba(0, 0, 0, 0.08);
      padding-right: 18px;
    }

    .year-badge-top {
      font-size: 10px;
      text-transform: uppercase;
      letter-spacing: 0.08em;
      color: #6b7280;
      font-weight: 700;
    }

    .current-year-display {
      font-size: 24px;
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
      font-size: 11px;
      color: #4b5563;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      font-weight: 500;
    }

    .player-controls-group {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .step-btn {
      width: 32px;
      height: 32px;
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
      width: 44px;
      height: 44px;
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
      font-size: 11px;
      font-weight: 700;
      color: #374151;
      background: #f3f4f6;
      border: 1px solid #e5e7eb;
      padding: 5px 9px;
      border-radius: 12px;
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
      width: 22px;
      height: 22px;
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
      padding: 3px 8px;
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
      font-size: 10px;
      font-weight: 700;
      color: var(--accent-terracotta);
    }

    .ms-name {
      font-size: 9.5px;
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

  private playbackTimerId: any = null;

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
