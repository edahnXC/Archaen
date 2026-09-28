import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ArchaeologyApiService } from './services/archaeology-api.service';
import {
  SiteSummary,
  SiteDetail,
  Civilization,
  HistoricalPeriod
} from './models/archaeology.models';
import { MapComponent } from './components/map/map.component';
import { TimelineScrubberComponent } from './components/timeline-scrubber/timeline-scrubber.component';
import { SiteDrawerComponent } from './components/site-drawer/site-drawer.component';
import { ComparisonModalComponent } from './components/comparison-modal/comparison-modal.component';
import { IndiaPriorityBarComponent, HorizonPreset } from './components/india-priority-bar/india-priority-bar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MapComponent,
    TimelineScrubberComponent,
    SiteDrawerComponent,
    ComparisonModalComponent,
    IndiaPriorityBarComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private readonly api = inject(ArchaeologyApiService);

  // Core State
  protected readonly allSites = signal<SiteSummary[]>([]);
  protected readonly filteredSites = signal<SiteSummary[]>([]);
  protected readonly currentYear = signal<number>(-2500);
  protected readonly selectedSiteId = signal<number | null>(null);
  protected readonly isDrawerOpen = signal<boolean>(false);
  protected readonly isComparisonModalOpen = signal<boolean>(false);
  protected readonly comparisonSourceSite = signal<SiteDetail | null>(null);
  protected readonly searchQuery = signal<string>('');
  protected readonly selectedRegionFilter = signal<string>('All');
  protected readonly isSidebarVisible = signal<boolean>(true);
  protected readonly isTimeFilterEnabled = signal<boolean>(true);

  // Metadata
  protected readonly civilizations = signal<Civilization[]>([]);
  protected readonly periods = signal<HistoricalPeriod[]>([]);
  protected readonly isLoading = signal<boolean>(false);

  // Computed Indicators
  protected readonly indianSitesCount = computed(() => {
    return this.filteredSites().filter(s => s.country.toLowerCase() === 'india').length;
  });

  ngOnInit(): void {
    this.loadCivilizationsAndPeriods();
    this.fetchSites();
  }

  /**
   * Fetches sites based on year, region, search query, etc.
   */
  fetchSites(): void {
    this.isLoading.set(true);

    const params: any = {
      pageSize: 100
    };

    if (this.isTimeFilterEnabled()) {
      params.year = this.currentYear();
    }

    if (this.selectedRegionFilter() === 'India') {
      params.region = 'India';
    }

    if (this.searchQuery().trim()) {
      params.search = this.searchQuery().trim();
    }

    this.api.getSites(params).subscribe({
      next: (res) => {
        this.allSites.set(res.items);
        this.applyLocalFilters();
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to fetch sites', err);
        this.isLoading.set(false);
      }
    });
  }

  onYearChanged(year: number): void {
    this.currentYear.set(year);
    if (this.isTimeFilterEnabled()) {
      this.fetchSites();
    }
  }

  toggleTimeFilter(): void {
    this.isTimeFilterEnabled.update(v => !v);
    this.fetchSites();
  }

  onSearchChange(): void {
    this.fetchSites();
  }

  onRegionFilterChange(region: string): void {
    this.selectedRegionFilter.set(region);
    this.fetchSites();
  }

  onSiteClicked(siteId: number): void {
    this.selectedSiteId.set(siteId);
    this.isDrawerOpen.set(true);
  }

  closeDrawer(): void {
    this.isDrawerOpen.set(false);
  }

  openCompareModal(site: SiteDetail): void {
    this.comparisonSourceSite.set(site);
    this.isComparisonModalOpen.set(true);
  }

  closeCompareModal(): void {
    this.isComparisonModalOpen.set(false);
  }

  toggleSidebar(): void {
    this.isSidebarVisible.update(v => !v);
  }

  onPresetSelected(preset: HorizonPreset): void {
    if (preset.id === 'all-india') {
      this.selectedRegionFilter.set('India');
      this.searchQuery.set('');
      this.isTimeFilterEnabled.set(false);
      this.fetchSites();
    } else if (preset.id === 'global-view') {
      this.selectedRegionFilter.set('All');
      this.searchQuery.set('');
      this.isTimeFilterEnabled.set(true);
      this.fetchSites();
    } else {
      if (preset.targetYear !== undefined) {
        this.currentYear.set(preset.targetYear);
      }
      if (preset.search) {
        this.searchQuery.set(preset.search);
      }
      this.selectedRegionFilter.set('All');
      this.isTimeFilterEnabled.set(false);
      this.fetchSites();
    }
  }

  private loadCivilizationsAndPeriods(): void {
    this.api.getCivilizations().subscribe({
      next: (civs) => this.civilizations.set(civs)
    });
    this.api.getPeriods().subscribe({
      next: (p) => this.periods.set(p)
    });
  }

  private applyLocalFilters(): void {
    let list = this.allSites();
    if (this.selectedRegionFilter() === 'India') {
      list = list.filter(s => s.country.toLowerCase() === 'india');
    }
    this.filteredSites.set(list);
  }
}
