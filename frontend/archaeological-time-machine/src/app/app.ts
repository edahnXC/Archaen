import { Component, OnInit, OnDestroy, inject, signal, computed, ViewChild, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ArchaeologyApiService } from './services/archaeology-api.service';
import {
  SiteSummary,
  SiteDetail,
  Civilization,
  HistoricalPeriod,
  Artefact
} from './models/archaeology.models';
import { MapComponent } from './components/map/map.component';
import { TimelineScrubberComponent } from './components/timeline-scrubber/timeline-scrubber.component';
import { SiteDrawerComponent } from './components/site-drawer/site-drawer.component';
import { ComparisonModalComponent } from './components/comparison-modal/comparison-modal.component';
import { IndiaPriorityBarComponent, HorizonPreset } from './components/india-priority-bar/india-priority-bar.component';
import { ArtefactViewer3DComponent } from './components/artefact-viewer3d/artefact-viewer3d.component';

export interface ExpeditionStory {
  id: string;
  siteId: number;
  title: string;
  ancientName?: string;
  civilization: string;
  dates: string;
  epoch: string;
  region: string;
  country: string;
  latitude: number;
  longitude: number;
  narrative: string;
  highlights: string[];
  artefactName: string;
  artefactModelType: string;
  artefactMaterial: string;
  artefactSummary: string;
  artefactImageUrl?: string;
  excavatorQuote: string;
}

export interface MethodologyStep {
  stepNumber: string;
  title: string;
  subtitle: string;
  icon: string;
  summary: string;
  details: string;
  realWorldExample: string;
  studentKeyPoint: string;
}

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
    IndiaPriorityBarComponent,
    ArtefactViewer3DComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit, OnDestroy {
  public readonly api = inject(ArchaeologyApiService);

  @ViewChild(MapComponent) mapComponent?: MapComponent;

  // Active Navigation Section for Sticky Header & Scroll Spy
  public readonly activeSection = signal<string>('hero');
  public readonly activePage = signal<'home' | 'sites'>('home');

  // Core Archaeological State
  public readonly allSites = signal<SiteSummary[]>([]);
  public readonly filteredSites = signal<SiteSummary[]>([]);
  public readonly currentYear = signal<number>(-2500);
  public readonly selectedSiteId = signal<number | null>(null);
  public readonly isDrawerOpen = signal<boolean>(false);
  public readonly isComparisonModalOpen = signal<boolean>(false);
  public readonly comparisonSourceSite = signal<SiteDetail | null>(null);
  public readonly searchQuery = signal<string>('');
  public readonly selectedRegionFilter = signal<string>('All');
  public readonly selectedCategory = signal<string>('all');
  public readonly isSidebarVisible = signal<boolean>(true);
  public readonly isTimeFilterEnabled = signal<boolean>(false);

  // 4D Spatio-Temporal Flight State
  public readonly isPlaying4DFlight = signal<boolean>(false);
  public readonly flightSpeed = signal<number>(1);
  private flightTimer: any = null;

  // Museum Archival Laboratory State
  public readonly allArtefacts = signal<Artefact[]>([]);
  public readonly selectedGalleryArtefact = signal<Artefact | null>(null);

  // Metadata & Loading States
  public readonly civilizations = signal<Civilization[]>([]);
  public readonly periods = signal<HistoricalPeriod[]>([]);
  public readonly isLoading = signal<boolean>(true);
  public readonly loadError = signal<string | null>(null);

  // Methodology Interactive Lab State
  public readonly activeMethodologyStep = signal<number>(0);
  public readonly methodologySteps: MethodologyStep[] = [
    {
      stepNumber: '01',
      title: 'Survey & Landscape Analysis',
      subtitle: 'Remote Sensing, Satellite Imagery & Micro-Topography',
      icon: '🛰️',
      summary: 'Before digging, archaeologists identify subtle surface anomalies, paleo-channels, and soil crop-marks without disturbing the archaeological record.',
      details: 'Non-invasive exploration utilizes multispectral satellite imagery, LiDAR point clouds, drone photogrammetry, and geomagnetic gradiometry to map buried stone walls, water catchments, and street grids across dozens of hectares.',
      realWorldExample: 'At Dholavira, satellite mapping and ground surveys revealed the full bedrock cascade reservoir network fed by seasonal streams Mansar and Manhar.',
      studentKeyPoint: 'Survey preserves context. Digging destroys the context forever, making comprehensive non-destructive survey the primary ethical duty.'
    },
    {
      stepNumber: '02',
      title: 'Stratigraphic Excavation',
      subtitle: 'The Law of Superposition & Harris Matrix',
      icon: '⛏️',
      summary: 'Excavators record sequential depositional soil matrices. Undisturbed lower strata are older than the layers deposited above them.',
      details: 'Using the Wheeler-box grid or open-area excavation, archaeologists uncover layers millimeter by millimeter, recording soil color, composition, grain size, inclusions, and exact 3D coordinates (Total Station) of every diagnostic specimen.',
      realWorldExample: 'At Sir Mortimer Wheeler’s 1945 excavations at Arikamedu, imported Roman Arretine ware layers provided an absolute benchmark dating South Indian Iron Age pottery.',
      studentKeyPoint: 'An artefact without documented stratigraphic provenance loses over 90% of its historical and scientific value.'
    },
    {
      stepNumber: '03',
      title: 'Material & Artefact Analysis',
      subtitle: 'Typology, Lapidary Studies & Residue Science',
      icon: '🏺',
      summary: 'Diagnostic artefacts reveal technological capabilities, dietary habits, trade contacts, aesthetic values, and societal specialization.',
      details: 'Ceramics, lithics, metals, and biological remains undergo optical microscopy, X-ray fluorescence (XRF), Raman spectroscopy, and organic residue gas chromatography to trace raw material sources and manufacturing techniques.',
      realWorldExample: 'Micro-drilled carnelian beads from Lothal have been traced through Mesopotamian cuneiform trade records and royal cemetery excavations at Ur.',
      studentKeyPoint: 'Diagnostic artefacts act as "index fossils" for archaeological horizons, allowing cross-correlation between different excavated settlements.'
    },
    {
      stepNumber: '04',
      title: 'Scientific Chronology',
      subtitle: 'Radiocarbon (14C) AMS, OSL & Epigraphy',
      icon: '⏳',
      summary: 'Archaeologists combine relative dating (seriation, stratigraphy) with absolute scientific dating methods to establish chronological anchors.',
      details: 'Accelerator Mass Spectrometry (AMS) measures the decay of Carbon-14 in organic charred grains and bones. Optically Stimulated Luminescence (OSL) dates when mineral grains were last exposed to sunlight.',
      realWorldExample: 'AMS dating of carbonized paddy grains at Keeladi by Beta Analytic established a secure 6th century BCE date (580 BCE) for urban Sangam literacy.',
      studentKeyPoint: 'Cross-dating synchronizes regional calendars by finding identical trade goods or king lists across disparate cultural spheres.'
    },
    {
      stepNumber: '05',
      title: 'Comparative Synthesis',
      subtitle: 'Reconstructing Inter-Regional Cultural Systems',
      icon: '🌐',
      summary: 'Individual site records are integrated into overarching geopolitical, economic, environmental, and technological models.',
      details: 'By comparing architectural styles, hydraulic models, mortuary customs, and ancient DNA, researchers reconstruct demographic migrations, climate resilience, and maritime trade routes across thousands of kilometers.',
      realWorldExample: 'Comparing the solid-wheeled war chariots of Sinauli with contemporary Bronze Age steppes has reshaped models of early Indian metallurgy.',
      studentKeyPoint: 'Archaeology is not an isolated curiosity; it is the evidence-based science of humanity’s shared environmental and cultural journey.'
    }
  ];

  // Multi-Site Field Stories (General Multi-Site Architecture, Meroë as Site 19)
  public readonly activeStoryIndex = signal<number>(0);
  public readonly expeditionStories: ExpeditionStory[] = [
    {
      id: 'dholavira',
      siteId: 1,
      title: 'Dholavira: Monumental Water Citadels of Kutch',
      ancientName: 'Kotada Timba',
      civilization: 'Indus Valley (Harappan)',
      dates: '3000 BCE – 1500 BCE',
      epoch: 'Mature Bronze Age',
      region: 'Kutch, Gujarat',
      country: 'India',
      latitude: 23.886389,
      longitude: 70.217222,
      narrative: 'Perched on the arid island of Khadir Bet in the Great Rann of Kutch, Dholavira represents the crowning achievement of Bronze Age urban planning and hydraulic engineering. Its builders carved sixteen massive cascade reservoirs into solid bed-rock capable of storing over 250,000 cubic meters of water. At its western gateway hung the world-famous 10-character Indus Signboard, rendered in luminous white crystalline gypsum.',
      highlights: [
        '16 stepped rock-cut cascading reservoirs with stormwater diversion channels',
        'Tripartite stone masonry planning: Castle Citadel, Middle Town, Lower Town',
        'The monumental 10-symbol gypsum Indus Signboard inscription',
        'Sophisticated carnelian, agate, and lapis lazuli bead workshops'
      ],
      artefactName: 'The Dholavira Inscription (The Signboard)',
      artefactModelType: 'stone_stele',
      artefactMaterial: 'Crystalline White Gypsum inlaid in wooden board',
      artefactSummary: 'Ten monumental Indus glyphs discovered fallen face-down inside the Western Gateway of the Citadel.',
      artefactImageUrl: '/images/artefacts/dholavira-signboard.jpg',
      excavatorQuote: '"Dholavira is an unparalleled testament to hydraulic ingenuity and monumental stone masonry in the ancient world." — Dr. R.S. Bisht, ASI'
    },
    {
      id: 'sinauli',
      siteId: 5,
      title: 'Sinauli: The Royal Chariot Necropolis of the Yamuna',
      ancientName: 'Sinauli Royal Necropolis',
      civilization: 'Copper Hoard / OCP Culture',
      dates: '2000 BCE – 1800 BCE',
      epoch: 'Bronze-Copper Transition',
      region: 'Baghpat, Uttar Pradesh',
      country: 'India',
      latitude: 29.136111,
      longitude: 77.208333,
      narrative: 'The 2018 Archaeological Survey of India excavations at Sinauli in the upper Yamuna basin revolutionized South Asian Bronze Age historiography. Excavators uncovered India’s earliest known physical war chariots with solid copper-inlaid wooden wheels, alongside warrior burials entombed in anthropomorphic copper-plated coffins with royal antennae swords, copper helmets, and gold headbands, proving indigenous warrior aristocracy contemporaneous with the late Indus.',
      highlights: [
        'Three full-sized solid-wheeled copper-embossed war chariots',
        'Copper-plated anthropomorphic royal wooden coffins with relief faces',
        'Copper antennae swords, daggers with wooden hilts, and composite shields',
        'Elite warrior female burial adorned with gold bangles and agate beads'
      ],
      artefactName: 'Royal Solid-Wheeled Bronze Age War Chariot',
      artefactModelType: 'bronze_chariot',
      artefactMaterial: 'Hardwood Chassis, Copper Triangle Inlays & Solid Wheels',
      artefactSummary: 'Intact full-scale two-wheeled chariot with copper triangle embossing and draught pole for horses.',
      artefactImageUrl: '/images/artefacts/sinauli-chariot.jpg',
      excavatorQuote: '"The discoveries at Sinauli have provided unequivocal physical evidence of an elite warrior culture around 2000 BCE." — Dr. S.K. Manjul, ASI'
    },
    {
      id: 'meroe',
      siteId: 19,
      title: 'Pyramids of Meroë: Royal City of Kush',
      ancientName: 'Medewi / Meroë',
      civilization: 'Kingdom of Kush (Meroë)',
      dates: '300 BCE – 350 CE',
      epoch: 'Iron Age & Classical Nubia',
      region: 'River Nile State (Begrawiya)',
      country: 'Sudan',
      latitude: 16.938333,
      longitude: 33.749167,
      narrative: 'Deep in the Nubian desert along the eastern banks of the Nile rise the steep-sided sandstone pyramids of Meroë, the royal necropolis of the Kushite Kingdom. Unlike Egyptian pyramids, Meroë’s monuments feature narrow 70-degree angles and ornate east-facing mortuary chapels adorned with bas-reliefs of warrior queens (Kandakes). Here, royal dynasties thrived as ancient Africa’s foremost iron-smelting metropolis and engaged in vibrant trade networks spanning from Alexandria to the Indian Ocean.',
      highlights: [
        'Over 200 steep-angled sandstone pyramids in North & South Cemeteries',
        'Tombs of the Candaces (ruling warrior queens such as Amanishakheto)',
        'Ancient industrial blast furnaces for mass iron production',
        'Inscribed stele in undeciphered Meroitic cursive script'
      ],
      artefactName: 'Golden Armlet of Queen Amanishakheto',
      artefactModelType: 'gold_armlet',
      artefactMaterial: 'Solid Gold with Enamel and Glass Inlay',
      artefactSummary: 'Found in Pyramid Beg. N. 6, depicting the winged goddess Hathor or Mut protecting Candace Amanishakheto.',
      artefactImageUrl: '/images/artefacts/meroe-armlet.jpg',
      excavatorQuote: '"Meroë was one of Africa\'s greatest civilizational powers, blending indigenous Nubian royalty with sophisticated metallurgical science."'
    },
    {
      id: 'keeladi',
      siteId: 8,
      title: 'Keeladi: Sangam Maritime & Literacy Dawn',
      ancientName: 'Keeladi / Vaigai Valley Horizon',
      civilization: 'Sangam Era (Early Historic South India)',
      dates: '600 BCE – 300 CE',
      epoch: 'Second Urbanization & Sangam Era',
      region: 'Sivaganga / Madurai, Tamil Nadu',
      country: 'India',
      latitude: 9.8625,
      longitude: 78.188889,
      narrative: 'Excavations along the Vaigai river basin at Keeladi pushed back the antiquity of the Sangam cultural horizon and Tamil-Brahmi script to the 6th century BCE. Archaeologists revealed a sophisticated brick-built manufacturing hub with dye vats, weaving tools, terracotta pipelines, and over a thousand potsherds inscribed with personal Tamil names, demonstrating widespread literacy and maritime commerce that imported Roman Arretine ware and exported prized Vaigai pearls.',
      highlights: [
        'Over 1,000 potsherds inscribed with early Tamil-Brahmi personal names (Aadhan, Kuviran-Aadhan)',
        'Urban brick architecture with continuous covered drainage conduits and terracotta wells',
        'Carnelian, quartz, and gold bead manufacturing workshops linked to Deccan & Kutch',
        'Direct maritime trade connections to Sri Lanka and Mediterranean ports'
      ],
      artefactName: "Potsherd Inscribed with 'Aadhan' in Tamil-Brahmi",
      artefactModelType: 'sangam_potsherd',
      artefactMaterial: 'Black-and-Red Ware Terracotta with Epigraphic Incision',
      artefactSummary: 'Inscribed ceramic fragment with early 6th-century BCE Tamil-Brahmi script reading personal name Aadhan.',
      artefactImageUrl: '/images/artefacts/keeladi-potsherd.jpg',
      excavatorQuote: '"Keeladi establishes that an egalitarian, literate urban society flourished on the banks of the Vaigai contemporaneous with Gangetic urbanization." — K. Amarnath Ramakrishna'
    },
    {
      id: 'petra',
      siteId: 20,
      title: 'Petra: Rose-Red Oasis of the Nabataean Kings',
      ancientName: 'Raqmu',
      civilization: 'Nabataean Kingdom',
      dates: '400 BCE – 106 CE',
      epoch: 'Hellenistic-Roman Antiquity',
      region: "Ma'an Governorate",
      country: 'Jordan',
      latitude: 30.3285,
      longitude: 35.4444,
      narrative: 'Carved directly into the swirling rose and amber sandstone cliffs of southern Jordan, Petra (Raqmu) controlled the pivotal crossroads of the ancient Incense and Spice Routes connecting the Arabian Sea, India, Egypt, and Rome. Through sophisticated terracotta pressure pipes, high-walled dams in the Siq, and underground cisterns, the Nabataeans transformed a hostile canyon into an oasis emporium celebrated for Al-Khazneh (The Treasury) and eggshell-thin painted ceramics.',
      highlights: [
        'Al-Khazneh (The Treasury) rock-cut facade sculpted into vertical canyon rock',
        'Hydraulic flash-flood diversion tunnels and terracotta pressurised water conduits',
        'Ad-Deir (The Monastery) monumental mountain sanctuary',
        'Fine painted eggshell ceramics and direct trade ties with India & Rome'
      ],
      artefactName: 'Nabataean Painted Fine Ware Bowl',
      artefactModelType: 'sangam_potsherd',
      artefactMaterial: 'Ultra-thin Painted Terracotta with Floral Motifs',
      artefactSummary: 'Eggshell-thin ceramic bowl decorated with delicate palmette and floral motifs unique to Petra.',
      artefactImageUrl: '/images/artefacts/petra-bowl.jpg',
      excavatorQuote: '"A rose-red city half as old as time, carved by master traders who made water flow uphill in a parched desert."'
    },
    {
      id: 'lothal',
      siteId: 2,
      title: 'Lothal: The Earliest Tidal Dockyard of the World',
      ancientName: 'Lothal ("Mound of the Dead")',
      civilization: 'Indus Valley (Harappan)',
      dates: '2400 BCE – 1900 BCE',
      epoch: 'Mature Harappan Maritime',
      region: 'Ahmedabad District, Gujarat',
      country: 'India',
      latitude: 22.522222,
      longitude: 72.249444,
      narrative: 'Situated at the head of the Gulf of Khambhat, Lothal was the premier maritime industrial emporium of the Harappan civilization. Its engineers constructed a monumental fired-brick basin with sluice gates and water locks to dock ocean-going trading vessels during high tide without silt accumulation. Within its warehouses, craftsmen manufactured prized micro-beads of carnelian and steatite that traveled across the Arabian Sea to Dilmun, Magan, and Ur in Mesopotamia.',
      highlights: [
        '216m x 37m engineered tidal dockyard basin with sluice gate and lock mechanism',
        'Central warehouse with 64 mudbrick platforms holding terracotta cargo sealings',
        'Persian Gulf circular steatite seal indicating direct maritime links with Dilmun & Sumer',
        'Micro-drill lapidary factories producing millions of etched carnelian beads'
      ],
      artefactName: 'Persian Gulf Steatite Button Seal',
      artefactModelType: 'seal_cube',
      artefactMaterial: 'Glazed Steatite with Intaglio Carving',
      artefactSummary: 'Circular button seal showing two quadruped animals flanking a central star, diagnostic of Dilmun trade.',
      artefactImageUrl: '/images/artefacts/lothal-seal.jpg',
      excavatorQuote: '"Lothal was the ancient world\'s most sophisticated container port, equipped with tidal engineering millennia ahead of its time." — S.R. Rao, ASI'
    },
    {
      id: 'pompeii',
      siteId: 18,
      title: 'Pompeii: Sealed Time Capsule of the Roman World',
      ancientName: 'Colonia Cornelia Veneria Pompeianorum',
      civilization: 'Roman Civilization',
      dates: '600 BCE – 79 CE',
      epoch: 'Classical Antiquity',
      region: 'Campania (Metropolitan Naples)',
      country: 'Italy',
      latitude: 40.750833,
      longitude: 14.486944,
      narrative: 'Catastrophically sealed beneath six meters of pumice and volcanic ash in 79 CE, Pompeii provides an unprecedented stratigraphic cross-section of daily Roman civic, industrial, and artistic life. Among its grand villas, archaeologists recovered imported Indian ivory statuettes of Lakshmi, showing that maritime spice networks from Arikamedu and Muziris connected directly into the wealthy homes of the Bay of Naples.',
      highlights: [
        'Volcanic tephrochronological sealing preserving plaster cast voids of citizens',
        'The Indian Ivory Statuette of Lakshmi found in the House of the Four Styles',
        'Aqua Augusta hydraulic castellum aquae feeding lead municipal fountains',
        'Vibrant mythological frescoes in the Villa of the Mysteries'
      ],
      artefactName: 'Pompeii Indian Ivory Statuette',
      artefactModelType: 'dancing_girl_bronze',
      artefactMaterial: 'Carved Elephant Ivory with Intricate Jewelry',
      artefactSummary: 'Indian ivory statuette of a female deity/attendant excavated in Pompeii, proving trans-oceanic trade with India before 79 CE.',
      artefactImageUrl: '/images/artefacts/gladiator-helmet.jpg',
      excavatorQuote: '"Pompeii is the ultimate stratigraphic time-machine: not a ruin, but a living moment frozen by geology."'
    }
  ];

  // Computed Properties for Rich Educational Indicators
  public readonly indianSitesCount = computed(() => {
    return this.allSites().filter(s => s.country.toLowerCase() === 'india').length;
  });

  public readonly worldSitesCount = computed(() => {
    return this.allSites().filter(s => s.country.toLowerCase() !== 'india').length;
  });

    public readonly signatureSites = computed(() => {
    // Exactly 6 curated signature sites on the Home page
    const all = this.allSites();
    const signatureSlugs = ['dholavira', 'lothal', 'sinauli', 'keeladi', 'petra', 'pompeii'];
    const matched = signatureSlugs
      .map(slug => all.find(s => s.slug.toLowerCase() === slug))
      .filter((s): s is SiteSummary => s !== undefined);
    return matched.length === 6 ? matched : all.slice(0, 6);
  });

  public readonly featuredSites = computed(() => {
    // Select curated signature sites across civilizations
    const all = this.allSites();
    const featuredSlugs = ['dholavira', 'sinauli', 'pyramids-of-meroe', 'keeladi', 'lothal', 'petra', 'mohenjo-daro', 'pompeii', 'rakhigarhi', 'bhimbetka'];
    const matched = all.filter(s => featuredSlugs.includes(s.slug.toLowerCase()));
    return matched.length > 0 ? matched : all.slice(0, 8);
  });

  public readonly activeEpochName = computed(() => {
    const y = this.currentYear();
    if (y <= -2600) return 'Early Urban & Mature Indus Integration';
    if (y <= -1900) return 'Mature Harappan Metropolis Era';
    if (y <= -1400) return 'Copper Hoard Warrior Horizon (Sinauli)';
    if (y <= -600) return 'Early Iron Age & Painted Grey Ware';
    if (y <= -300) return 'Second Urbanization & Sangam Dawn (Keeladi)';
    if (y <= -185) return 'Mauryan Imperial Horizon (Ashokan Edicts)';
    if (y <= 400) return 'Classical Antiquity & Indo-Roman Maritime Horizon';
    return 'Medieval Temple Cities & Global Empires';
  });

  public readonly discoverySites = computed(() => {
    let list = this.allSites();
    const cat = this.selectedCategory();

    if (cat === 'indus') {
      list = list.filter(s => s.civilizations?.some(c => (c.name || c.civilizationName || '').toLowerCase().includes('indus') || (c.name || c.civilizationName || '').toLowerCase().includes('harappan')));
    } else if (cat === 'copper-vedic') {
      list = list.filter(s => s.civilizations?.some(c => (c.name || c.civilizationName || '').toLowerCase().includes('copper') || (c.name || c.civilizationName || '').toLowerCase().includes('vedic')));
    } else if (cat === 'sangam') {
      list = list.filter(s => s.civilizations?.some(c => (c.name || c.civilizationName || '').toLowerCase().includes('sangam') || (c.name || c.civilizationName || '').toLowerCase().includes('mauryan') || (c.name || c.civilizationName || '').toLowerCase().includes('satavahana') || (c.name || c.civilizationName || '').toLowerCase().includes('chola')));
    } else if (cat === 'world') {
      list = list.filter(s => s.country.toLowerCase() !== 'india');
    }

    const q = this.searchQuery().trim().toLowerCase();
    if (q) {
      list = list.filter(s =>
        s.name.toLowerCase().includes(q) ||
        (s.ancientName && s.ancientName.toLowerCase().includes(q)) ||
        s.region.toLowerCase().includes(q) ||
        s.country.toLowerCase().includes(q) ||
        s.siteType.toLowerCase().includes(q)
      );
    }

    return list;
  });

  ngOnInit(): void {
    this.fetchInitialData();
    this.loadCivilizationsAndPeriods();
  }

  @HostListener('window:scroll')
  onWindowScroll(): void {
    const sections = ['hero', 'featured', 'methodology', 'map-explorer', 'artefacts', 'stories', 'timeline', 'students'];
    const scrollPosition = window.scrollY + 120;

    for (const id of sections) {
      const el = document.getElementById(id);
      if (el) {
        const top = el.offsetTop;
        const height = el.offsetHeight;
        if (scrollPosition >= top && scrollPosition < top + height) {
          this.activeSection.set(id);
          break;
        }
      }
    }
  }

  public openSitesDirectory(): void {
    this.activePage.set('sites');
    this.activeSection.set('sites');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public returnToHome(): void {
    this.activePage.set('home');
    this.activeSection.set('hero');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public scrollToSection(sectionId: string): void {
    if (this.activePage() === 'sites') {
      this.activePage.set('home');
    }
    this.activeSection.set(sectionId);
    setTimeout(() => {
      const element = document.getElementById(sectionId);
      if (element) {
        const navOffset = 70;
        const elementPosition = element.getBoundingClientRect().top + window.scrollY;
        window.scrollTo({
          top: elementPosition - navOffset,
          behavior: 'smooth'
        });
      }
      if (sectionId === 'map-explorer') {
        setTimeout(() => this.mapComponent?.invalidateSize(), 300);
      }
    }, 50);
  }

  /**
   * Fetches sites and artefacts from backend API with complete loading & error handling.
   */
  public fetchInitialData(): void {
    this.isLoading.set(true);
    this.loadError.set(null);

    this.api.getSites({ pageSize: 100 }).subscribe({
      next: (res) => {
        this.allSites.set(res.items);
        this.applyLocalFilters();
        this.isLoading.set(false);
        this.loadError.set(null);
      },
      error: (err) => {
        console.error('Failed to fetch initial sites:', err);
        this.isLoading.set(false);
        this.loadError.set('Unable to connect to archaeological database service at http://localhost:5032. Please ensure the backend is running.');
      }
    });

    this.api.getArtefacts().subscribe({
      next: (arts) => {
        this.allArtefacts.set(arts);
        if (arts.length > 0 && !this.selectedGalleryArtefact()) {
          this.selectedGalleryArtefact.set(arts[0]);
        }
      },
      error: (err) => {
        console.error('Failed to fetch diagnostic artefacts:', err);
      }
    });
  }

  public onImageError(event: Event, name?: string): void {
    const img = event.target as HTMLImageElement;
    if (img && !img.dataset['fallback']) {
      img.dataset['fallback'] = 'true';
      img.src = '/images/fallback.jpg';
    }
  }

  public retryFetchData(): void {
    this.fetchInitialData();
    this.loadCivilizationsAndPeriods();
  }

  public onSiteClicked(siteId: number): void {
    this.selectedSiteId.set(siteId);
    this.isDrawerOpen.set(true);
  }

  public closeDrawer(): void {
    this.isDrawerOpen.set(false);
  }

  public openCompareModal(site: SiteDetail): void {
    this.comparisonSourceSite.set(site);
    this.isComparisonModalOpen.set(true);
  }

  public closeCompareModal(): void {
    this.isComparisonModalOpen.set(false);
  }

  public flyToSite(latitude: number, longitude: number, siteId?: number): void {
    if (this.activePage() === 'sites') {
      this.activePage.set('home');
    }
    this.scrollToSection('map-explorer');
    setTimeout(() => {
      this.mapComponent?.invalidateSize();
      this.mapComponent?.focusSite(latitude, longitude, 8);
      if (siteId) {
        this.onSiteClicked(siteId);
      }
    }, 350);
  }

  public setMethodologyStep(index: number): void {
    this.activeMethodologyStep.set(index);
  }

  public selectStory(index: number): void {
    this.activeStoryIndex.set(index);
  }

  public nextStory(): void {
    const cur = this.activeStoryIndex();
    this.activeStoryIndex.set((cur + 1) % this.expeditionStories.length);
  }

  public prevStory(): void {
    const cur = this.activeStoryIndex();
    this.activeStoryIndex.set(cur === 0 ? this.expeditionStories.length - 1 : cur - 1);
  }

  public selectGalleryArtefact(art: Artefact): void {
    this.selectedGalleryArtefact.set(art);
  }

  public onYearChanged(year: number): void {
    this.currentYear.set(year);
    this.isTimeFilterEnabled.set(true);
    this.applyLocalFilters();
  }

  public toggle4DTimeFlight(): void {
    if (this.isPlaying4DFlight()) {
      this.stop4DTimeFlight();
    } else {
      this.start4DTimeFlight();
    }
  }

  public setFlightSpeed(speed: number): void {
    this.flightSpeed.set(speed);
    if (this.isPlaying4DFlight()) {
      this.stop4DTimeFlight();
      this.start4DTimeFlight();
    }
  }

  public start4DTimeFlight(): void {
    this.isPlaying4DFlight.set(true);
    this.isTimeFilterEnabled.set(true);

    // If currently at or near the end, restart from 3300 BCE
    if (this.currentYear() >= 500) {
      this.currentYear.set(-3300);
      this.applyLocalFilters();
    }

    const intervalMs = Math.max(120, Math.floor(450 / this.flightSpeed()));
    this.flightTimer = setInterval(() => {
      let nextYear = this.currentYear() + 50 * this.flightSpeed();
      if (nextYear > 500) {
        nextYear = -3300; // Seamless loop across history
      }
      this.currentYear.set(nextYear);
      this.applyLocalFilters();
    }, intervalMs);
  }

  public stop4DTimeFlight(): void {
    this.isPlaying4DFlight.set(false);
    if (this.flightTimer) {
      clearInterval(this.flightTimer);
      this.flightTimer = null;
    }
  }

  public ngOnDestroy(): void {
    this.stop4DTimeFlight();
  }

  public applyHorizon(year: number): void {
    this.currentYear.set(year);
    this.isTimeFilterEnabled.set(true);
    this.applyLocalFilters();
    this.scrollToSection('map-explorer');
    setTimeout(() => {
      this.mapComponent?.invalidateSize();
      if (year <= -2000) {
        this.mapComponent?.focusIndia();
      } else {
        this.mapComponent?.focusGlobal();
      }
    }, 350);
  }

  public resetHorizon(): void {
    this.isTimeFilterEnabled.set(false);
    this.applyLocalFilters();
  }

  public toggleTimeFilter(): void {
    this.isTimeFilterEnabled.update(v => !v);
    this.applyLocalFilters();
  }

  public onSearchChange(): void {
    this.applyLocalFilters();
  }

  public onRegionFilterChange(region: string): void {
    this.selectedRegionFilter.set(region);
    this.applyLocalFilters();
  }

  public setCategoryFilter(category: string): void {
    this.selectedCategory.set(category);
  }

  public toggleSidebar(): void {
    this.isSidebarVisible.update(v => !v);
  }

  public onPresetSelected(preset: HorizonPreset): void {
    if (preset.id === 'all-india') {
      this.selectedRegionFilter.set('India');
      this.searchQuery.set('');
      this.isTimeFilterEnabled.set(false);
      this.applyLocalFilters();
    } else if (preset.id === 'global-view') {
      this.selectedRegionFilter.set('All');
      this.searchQuery.set('');
      this.isTimeFilterEnabled.set(false);
      this.applyLocalFilters();
    } else {
      if (preset.targetYear !== undefined) {
        this.currentYear.set(preset.targetYear);
        this.isTimeFilterEnabled.set(true);
      }
      if (preset.search) {
        this.searchQuery.set(preset.search);
      }
      this.selectedRegionFilter.set('All');
      this.applyLocalFilters();
    }
    this.scrollToSection('map-explorer');
  }

  public formatYear(year: number): string {
    if (year < 0) {
      return `${Math.abs(year)} BCE`;
    } else if (year === 0) {
      return '1 BCE / 1 CE';
    } else {
      return `${year} CE`;
    }
  }

  private loadCivilizationsAndPeriods(): void {
    this.api.getCivilizations().subscribe({
      next: (civs) => this.civilizations.set(civs),
      error: (err) => console.error('Failed to load civilizations', err)
    });
    this.api.getPeriods().subscribe({
      next: (p) => this.periods.set(p),
      error: (err) => console.error('Failed to load periods', err)
    });
  }

  public applyLocalFilters(): void {
    let list = this.allSites();

    // 1. Search query
    const query = this.searchQuery().trim().toLowerCase();
    if (query) {
      list = list.filter(s =>
        s.name.toLowerCase().includes(query) ||
        (s.ancientName && s.ancientName.toLowerCase().includes(query)) ||
        s.region.toLowerCase().includes(query) ||
        s.country.toLowerCase().includes(query) ||
        s.siteType.toLowerCase().includes(query) ||
        s.civilizations?.some(c => (c.name || c.civilizationName || '').toLowerCase().includes(query))
      );
    }

    // 2. Region filter
    if (this.selectedRegionFilter() === 'India') {
      list = list.filter(s => s.country.toLowerCase() === 'india');
    }

    // 3. Temporal horizon filter
    if (this.isTimeFilterEnabled()) {
      const year = this.currentYear();
      list = list.filter(s => (s.startYear <= year + 80) && (s.endYear >= year - 80));
    }

    this.filteredSites.set(list);
  }
}
