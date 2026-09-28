# Archaeological Time Machine - System Progress & Indian Regional Archaeological Compendium

> **Authoritative Technical & Archaeological Reference**  
> *Last Updated: September 2026*  
> *System Focus: Global Spatio-Temporal GIS with Comprehensive Prioritization of Indian Archaeological Sites*

---

## 1. System Status & Live Engineering Progress Tracker

### 1.1 Architectural Overview
The **Archaeological Time Machine** is an interactive 4D Geographic Information System (GIS) and temporal exploration engine allowing researchers, students, and archaeology enthusiasts to traverse ancient civilizations across space (geographic coordinates, spatial proximity, regional clusters) and time (continuous signed BCE/CE chronological progression).

```
+------------------------------------------------------------------------------------+
|                             FRONTEND LAYER (Angular 21)                            |
|                                                                                    |
|  +------------------------+   +-----------------------+   +---------------------+  |
|  |    Interactive GIS     |   | Temporal Time Machine |   |   3D Artefact Lab   |  |
|  | (Leaflet / GeoJSON /   |   | (BCE/CE Scrubber /    |   |  (Three.js WebGL /  |  |
|  |  Indian Horizon Layers)|   |  Continuous Playback) |   |  PBR Surface Shaders|  |
|  +-----------+------------+   +-----------+-----------+   +----------+----------+  |
|              \                            |                          /             |
|               +---------------------------+-------------------------+              |
|                                           |                                        |
|                          Angular Signals & Reactive State                          |
+-------------------------------------------+----------------------------------------+
                                            | HTTP / JSON REST (CORS: 5032 <-> 4200)
                                            v
+------------------------------------------------------------------------------------+
|                         BACKEND LAYER (ASP.NET Core 10 Web API)                    |
|                                                                                    |
|  - [SitesController]          : Temporal filtering (?year=-2500, ?minYear, ?region)|
|  - [SiteComparisonController] : Geodesic distance & chronological overlap matrix   |
|  - [CivilizationsController]  : Cultural traditions, eras, and geographic boundaries|
|  - [PeriodsController]        : Stratigraphic & chronological epochs               |
|  - [ArtefactsController]      : Diagnostic finds & 3D model metadata               |
+-------------------------------------------+----------------------------------------+
                                            |
                                            v
+------------------------------------------------------------------------------------+
|                        PERSISTENCE & SPATIAL ENGINE                                |
|                                                                                    |
|  - SQLite + NetTopologySuite (SRID 4326 WGS-84 Point Geometry)                     |
|  - Microsecond geodesic proximity calculations (Haversine & ellipsoidal geodesic)  |
|  - Complete Academic Seed Engine with Prioritized Indian Archaeological Sites      |
+------------------------------------------------------------------------------------+
```

### 1.2 Development Progress & Milestones

| Milestone / Component | Technology | Status | Key Features & Output |
| :--- | :--- | :--- | :--- |
| **Milestone 1: Web API Backend** | ASP.NET Core 10, C# 13, EF Core | **COMPLETED** | Normalized domain entities, spatial queries, signed BCE/CE math, REST endpoints. |
| **Unit Test Suite** | xUnit, FluentAssertions | **COMPLETED** | 16/16 tests passing cleanly. Temporal boundary math and spatial distance validated. |
| **Indian Archaeological Expansion** | EF Core, DatabaseSeeder | **COMPLETED** | Extensive expansion of Indian sites (Rakhigarhi, Kalibangan, Sinauli, Keeladi, Arikamedu, Bhimbetka, Pataliputra, Sannati, Surkotada, Inamgaon). |
| **Milestone 2: Angular 21 GIS Frontend** | Angular 21, Leaflet, Three.js | **IN PROGRESS** | Interactive map, BCE/CE time scrubber (-4000 to 500 CE), Indian site drawer, 3D artefact inspection. |
| **3D Artefact Viewer** | Three.js WebGL procedural shaders | **IN PROGRESS** | Real-time 3D rotation, inspection of seals, bronzes, inscribed stelae, terracotta. |
| **Site Comparison Matrix** | Angular Standalone Components | **PLANNED** | Side-by-side comparison of 2 sites with distance, stratigraphy alignment, and co-existence duration. |

---

## 2. Regional Archaeological Compendium: India Priority

The Indian subcontinent possesses one of the deepest, richest, and most continuous archaeological records on Earth. This compendium documents the primary sites prioritized within the Archaeological Time Machine, arranged chronologically and geographically.

```
                                CHRONOLOGICAL HORIZONS OF INDIA
+---------------------------------------------------------------------------------------------------------+
| Paleolithic/Mesolithic | Early Agricultural/Neolithic | Mature Indus Valley | Late Harappan/Copper Age  |
| (-100,000 to -3000)    | (-7000 to -3300)             | (-2600 to -1900)    | (-2000 to -1400)          |
| Bhimbetka              | Mehrgarh / Burzahom          | Dholavira, Lothal,  | Sinauli, Inamgaon,        |
|                        |                              | Rakhigarhi, Kaliban.| Daimabad                  |
+---------------------------------------------------------------------------------------------------------+
                                                     |
                                                     v
+---------------------------------------------------------------------------------------------------------+
| Painted Grey Ware / Vedic | NBPW / Second Urbanization | Mauryan & Early Imperial | Sangam Age / Maritime  |
| (-1200 to -500)           | (-600 to -300)             | (-322 to -185)           | (-600 to 300 CE)       |
| Hastinapur, Ahichchhatra  | Rajgir, Sisupalgarh        | Pataliputra, Sannati,    | Keeladi, Arikamedu,    |
|                           |                            | Barabar Caves            | Kodumanal, Muziris     |
+---------------------------------------------------------------------------------------------------------+
```

---

### 2.1 Prehistoric & Rock Art Horizon

#### 1. Bhimbetka Rock Shelters (Madhya Pradesh)
- **Ancient / Local Name**: Bhimbetka (associated with *Bhima* of Mahabharata tradition)
- **Location**: Raisen District, Madhya Pradesh, India (Vindhyan Mountain range)
- **Coordinates**: `22.9375° N, 77.6133° E`
- **Chronological Span**: `-100,000 to 1000` (Middle Paleolithic through Medieval era)
- **Excavator / History**: Discovered by Dr. V. S. Wakankar in 1957; extensive research by Archaeological Survey of India.
- **UNESCO World Heritage**: Inscribed 2003.
- **Key Architectural & Physical Features**:
  - Over 750 sandstone rock shelters scattered over 10 km².
  - Natural auditorium cave with massive quartz and sandstone monoliths.
  - Habitation deposits showing continuous succession from Acheulian handaxe tool industries, through Middle Paleolithic flake tools, Upper Paleolithic blade industries, to Mesolithic microliths.
- **Diagnostic Art & Artefacts**:
  - Polychrome rock paintings executed in mineral pigments (hematite red, vegetal white, yellow, green).
  - Famous "Zoo Rock" depicting 252 animals across 16 species (bison, tigers, rhinoceros, elephants).
  - Ritual dance scenes, warrior stick figures with bows and daggers, childbirth scenes.
- **Significance in Time Machine**: Acts as the deep chronological anchor for human cultural continuity in the Indian subcontinent.

---

### 2.2 Indus Valley (Harappan) Civilization — Indian Metropolises

#### 2. Dholavira (Kotada Timba, Gujarat)
- **Ancient / Local Name**: Kotada Timba ("Large Fort")
- **Location**: Khadir Bet island, Great Rann of Kutch, Gujarat, India
- **Coordinates**: `23.8864° N, 70.2172° E`
- **Chronological Span**: `-3000 to -1500` (Stages I to VII)
- **Excavator / History**: Discovered by J.P. Joshi (1967-68); excavated by Dr. R.S. Bisht (1990-2005) for ASI.
- **UNESCO World Heritage**: Inscribed 2021.
- **Key Architectural & Urban Planning Features**:
  - **Tri-Partite Urban Division**: Fortified Citadel (Castle & Bailey), Middle Town, and Lower Town, enclosed within a massive outer rectangular stone fortification wall (771 m x 616 m).
  - **Monumental Hydraulic Engineering**: 16 cascade water reservoirs cut directly into bed-rock and stone masonry, collecting seasonal runoff from the Mansar and Manhar streams; total storage capacity exceeded 250,000 m³.
  - **Ceremonial Ground / Stadium**: Expansive open stadium (283 m x 47 m) with tiered stone seating for royal ceremonies and games.
- **Diagnostic Artefacts**:
  - **The Dholavira Signboard**: 10 gigantic Indus signs (each ~37 cm high) made from crystalline white gypsum, originally mounted above the Western Gateway of the Citadel.
  - Spooled copper bangles, polished stone pillars and dam components, micro-carnelian beads.
- **Significance in Time Machine**: The premier archetype of Bronze Age sustainable water architecture and master-planned stone masonry.

#### 3. Lothal (Gujarat)
- **Ancient / Local Name**: Lothal ("Mound of the Dead")
- **Location**: Dholka Taluka, Ahmedabad District, Gujarat, India (Bhogavo River basin)
- **Coordinates**: `22.5222° N, 72.2486° E`
- **Chronological Span**: `-2400 to -1900`
- **Excavator / History**: Discovered November 1954; excavated by Dr. S.R. Rao (1955-1962).
- **Key Architectural & Industrial Features**:
  - **World's Earliest Engineered Tidal Dockyard**: Colossal trapezoidal kiln-fired brick basin (214 m x 36 m, depth 4.5 m) engineered with a sluice gate and spillway connected via an inlet channel to the Gulf of Khambhat tides.
  - **Acropolis & Warehouse**: Raised mudbrick platform with 64 cubical mudbrick blocks for inspecting and sealing export-import cargo.
  - **Bead-Making Factory**: Multi-roomed bead workshop with central courtyard, kiln, and raw chert, agate, and jasper cores.
- **Diagnostic Artefacts**:
  - **Persian Gulf Button Seal**: Proves direct maritime trade with Dilmun (Bahrain) and Mesopotamia.
  - Ivory measuring scale (smallest division ~1.704 mm).
  - Terracotta models of Egyptian mummies and domesticated horses.
  - Double burial (joint male-female interment).
- **Significance in Time Machine**: Demonstrates India's pivotal position as the Bronze Age trade hub connecting the subcontinent to the Persian Gulf and Red Sea.

#### 4. Rakhigarhi (Haryana)
- **Ancient / Local Name**: Rakhigarhi (Mounds 1 through 9)
- **Location**: Hisar District, Haryana, India (Drishadvati / Ghaggar basin)
- **Coordinates**: `29.2882° N, 76.1130° E`
- **Chronological Span**: `-3300 to -1500` (Pre-Harappan, Early, Mature, and Late Harappan)
- **Excavator / History**: Surveyed by Suraj Bhan (1969); excavated by Amarendra Nath (1997-2000), Dr. Vasant Shinde (Deccan College, 2011-2017), and ASI (2021-present).
- **Key Architectural & Urban Features**:
  - **Largest Harappan Metropolis**: Spans an estimated 350 to 550 hectares, exceeding Mohenjo-daro and Harappa in total area.
  - Monumental mudbrick granary with specialized clay lime aeration trenches to preserve grain.
  - Interconnected network of burnt-brick storm drains with soakage jars at intervals.
  - Extensive residential quarters with distinct lapidary workshops and furnace hearths.
- **Diagnostic Discoveries & Scientific Milestones**:
  - **Ancient DNA Breakthrough (Individual I6113)**: Female skeleton from Mound 7 cemetery sequenced in 2019 by Shinde, Reich et al., demonstrating genetic continuity of indigenous South Asian populations without Steppe pastoralist ancestry during the Mature Harappan phase.
  - Shell and copper bangles, terracotta animal figurines, inscribed steatite intaglio seals.
- **Significance in Time Machine**: Anchors the inland agrarian-industrial power of the Harappan civilization on the Indian side of the border.

#### 5. Kalibangan (Rajasthan)
- **Ancient / Local Name**: Kalibangan ("Black Bangles", named after the abundant terracotta bangles)
- **Location**: Hanumangarh District, Rajasthan, India (Ghaggar-Hakra paleo-channel)
- **Coordinates**: `29.4731° N, 74.1311° E`
- **Chronological Span**: `-3000 to -1800` (Early & Mature Harappan)
- **Excavator / History**: Identified by Luigi Pio Tessitori (1917); systematically excavated by B.B. Lal and B.K. Thapar (1960-1969) for ASI.
- **Key Discoveries & Architectural Features**:
  - **World's Earliest Ploughed Agricultural Field**: Discovered south of the settlement; preserved criss-cross furrows demonstrating simultaneous dual-cropping (mustard in wider furrows, gram/chickpea in narrower furrows), a technique still used in Rajasthan today.
  - **Fire Altars (Havana Kundas)**: Series of brick-lined sacrificial pits containing charcoal, ash, terracotta animal sacrifice bones, and cylindrical clay cakes, indicating indigenous community or domestic fire rituals.
  - **Citadel & Fortified Lower City**: Separately fortified acropolis and residential town built of standard mudbricks (ratio 1:2:3 in Early Harappan, 1:2:4 in Mature Harappan).
- **Diagnostic Artefacts**:
  - Thousands of blackened terracotta and faience bangles.
  - Terracotta bull figurines, engraved seals, and cylindrical Mesopotamian-style seals.
- **Significance in Time Machine**: Direct proof of agricultural engineering, religious continuity, and dual-crop agronomy in Bronze Age India.

#### 6. Surkotada (Gujarat)
- **Location**: Rapar Taluka, Kutch District, Gujarat, India
- **Coordinates**: `23.6186° N, 70.8383° E`
- **Chronological Span**: `-2300 to -1700`
- **Excavator / History**: Excavated by Dr. J.P. Joshi (1970-1972) for ASI.
- **Key Discoveries**:
  - Fortified stronghold constructed from dressed rubble stone masonry and mud bricks.
  - **Equine Skeletal Debate**: Discovery of genuine horse (*Equus caballus*) teeth and bones at upper and middle occupational levels confirmed by Hungarian archaeozoologist Sándor Bökönyi, sparking substantial academic debate on the chronology of horses in South Asia.
  - Oval potsherds and four unique pot-burials marked with stone boulders.
- **Significance in Time Machine**: Demonstrates Harappan military fortification and strategic garrison control over desert and salt marsh transit routes.

---

### 2.3 Copper Age, Warrior Burials & OCP/Copper Hoard Horizon

#### 7. Sinauli (Baghpat, Uttar Pradesh)
- **Location**: Baraut Tehsil, Baghpat District, Uttar Pradesh, India (Yamuna-Hindon doab)
- **Coordinates**: `29.1353° N, 77.2064° E`
- **Chronological Span**: `-2000 to -1800` (Bronze / Copper Age Horizon)
- **Excavator / History**: Excavated in 2005-2006 and 2018-2019 by Dr. S.K. Manjul (Institute of Archaeology / ASI).
- **Sensational Discoveries**:
  - **Three Full-Scale War Chariots**: World's oldest known Indian wheeled vehicles, featuring solid wooden wheels adorned with triangular copper motifs, chassis, yoke, and pole, designed for two horses or draft animals.
  - **Royal Warrior Burials**: Legged wooden coffins (*manjushas*) decorated with eight anthropomorphic copper headgear figures.
  - **Military Armour & Weaponry**: First physical copper helmets in Indian archaeological history, decorated copper antenna swords, copper shields inlaid with geometric patterns, daggers, and bow sheaths.
- **Significance in Time Machine**: Rewrites the military, martial, and technological history of the 2nd millennium BCE Ganga-Yamuna doab, bridging the Harappan decline and the Vedic heroic age.

#### 8. Inamgaon (Maharashtra)
- **Location**: Shirur Taluka, Pune District, Maharashtra, India (Ghod River, tributary of Bhima)
- **Coordinates**: `18.5992° N, 74.5242° E`
- **Chronological Span**: `-1600 to -700` (Malwa, Early Jorwe, and Late Jorwe Cultures)
- **Excavator / History**: Excavated by M.K. Dhavalikar, H.D. Sankalia, and Z.D. Ansari (Deccan College, 1968-1982).
- **Key Discoveries**:
  - Detailed horizontal excavation of over 130 mud houses (rectangular in Early Jorwe, circular in Late Jorwe).
  - Massive stone and mud embankment dam and irrigation diversion canal (118 m long, 3.5 m deep).
  - Mother goddess clay figurines without heads or with bull mounts, placed in clay receptacles.
  - Extended adult burials inside houses with four-legged clay burial urns and funerary vessels.
- **Significance in Time Machine**: Standard reference site for Deccan Chalcolithic socio-economic organization and the transition into the Early Iron Age.

---

### 2.4 Second Urbanization, Imperial Capitals & Sangam Maritime Age

#### 9. Pataliputra & Kumrahar (Patna, Bihar)
- **Ancient Name**: Pataliputra / Kusumapura / Palibothra (Greek)
- **Location**: Patna, Bihar, India (Confluence of the Ganga, Son, and Gandak rivers)
- **Coordinates**: `25.5975° N, 85.1764° E`
- **Chronological Span**: `-500 to 550` (Haryanka, Nanda, Mauryan, Shunga, and Gupta Empires)
- **Excavator / History**: Described by Megasthenes; excavated by L.A. Waddell (1892), D.B. Spooner (1912-1913), and A.S. Altekar (1951-1955).
- **Monumental Architectural Remains**:
  - **Mauryan 80-Pillared Hypostyle Hall (Kumrahar)**: Colossal royal audience hall supported by polished black-spotted Chunar sandstone monolithic pillars, reflecting Mauryan imperial grandeur.
  - **Defensive Wooden Palisades (Bulandi Bagh)**: Massive teak-wood fortification walls with loopholes for archers and water-drainage sluices, matching Megasthenes' eyewitness description of Palibothra.
  - **Arogya Vihara**: Gupta-period monastic hospital complex led by royal physician Dhanvantari, complete with medical seals.
- **Diagnostic Artefacts**:
  - Polished Mauryan Didarganj Yakshi (life-sized polished sandstone sculpture).
  - Bull and lion pillar capitals with honeysuckle and palmette motifs.
- **Significance in Time Machine**: Epicenter of ancient Indian empire building, statecraft (Kautilya's *Arthashastra*), and Buddhist transmission under Emperor Ashoka.

#### 10. Keeladi (Sivaganga / Vaigai Valley, Tamil Nadu)
- **Location**: Sivaganga District (near Madurai), Tamil Nadu, India (Vaigai River basin)
- **Coordinates**: `9.8631° N, 78.1884° E`
- **Chronological Span**: `-600 to 300` (Sangam Urban Era)
- **Excavator / History**: Excavations by ASI (Phases I-III led by K. Amarnath Ramakrishna) and Tamil Nadu State Department of Archaeology (Phases IV-X, 2018-present).
- **Key Breakthrough Discoveries**:
  - **Urban Sangam Settlement in South India**: Shattered the previous consensus that South Indian urbanization occurred much later; AMS carbon dating at Beta Analytic (Miami) pushed Keeladi's urban timeline to **6th century BCE (580 BCE)**.
  - **Literacy & Tamil-Brahmi Script**: Over 70 potsherds inscribed with personal Tamil names (e.g., *Aadhan*, *Udhiran*) in early Tamil-Brahmi script, proving widespread vernacular literacy.
  - **Industrial Infrastructure**: Brick furnaces, open dye vats, terracotta ring wells, spinning whorls, and advanced drainage channels.
  - **Luxury Gem Industry**: Over 4,000 beads made of carnelian, agate, quartz, amethyst, and glass, alongside ivory dice and combs.
- **Significance in Time Machine**: Crucial evidence proving an independent, literate, urban civilization in deep South India contemporaneous with the Gangetic Mahajanapadas.

#### 11. Arikamedu (Puducherry)
- **Ancient Name**: Podouke (mentioned in the *Periplus of the Erythraean Sea* and Ptolemy's *Geography*)
- **Location**: Ariyankuppam River estuary, Puducherry, India
- **Coordinates**: `11.9022° N, 79.8189° E`
- **Chronological Span**: `-200 to 300`
- **Excavator / History**: French antiquarian Jouveau-Dubreuil (1937); landmark scientific stratigraphic excavation by Sir Mortimer Wheeler (1945); further work by J.M. Casal and Vimala Begley.
- **Key Discoveries**:
  - **Premier Indo-Roman Maritime Port**: Large brick warehouse and dye vats directly on the river wharf for loading sea-going merchant vessels.
  - **Imported Roman Commodities**: Fragments of hundreds of stamped Italian amphorae used for transporting Mediterranean wine, olive oil, and garum (fish sauce).
  - **Arretine / Terra Sigillata Pottery**: Fine red-gloss Roman tableware stamped with makers' marks from Italian workshops (Arezzo).
  - **Glass Bead Global Factory**: World's foremost production center of drawn glass beads (*mutisalah*), exported throughout the Indian Ocean to Southeast Asia and Rome.
- **Significance in Time Machine**: Landmark site that established the chronological dating baseline for South Indian historic archaeology via Roman cross-dating.

#### 12. Sannati & Kanaganahalli (Karnataka)
- **Ancient Name**: Chandralamba Kshetra / Suvarnagiri province
- **Location**: Chittapur Taluka, Kalaburagi District, Karnataka, India (Bhima River)
- **Coordinates**: `16.8283° N, 76.9022° E`
- **Chronological Span**: `-300 to 300`
- **Excavator / History**: Accidental discovery in 1986 when a temple idol's collapse revealed an Ashokan edict; excavated by ASI (1994-2002).
- **Sensational Sculptural & Epigraphic Finds**:
  - **The Only Known Portrait of Emperor Ashoka**: Magnificent limestone slab carved with the likeness of Emperor Ashoka surrounded by royal attendants, inscribed in Mauryan Brahmi: `𑀭𑀸𑀜𑁄 𑀅𑀲𑁄𑀓` (*Ranyo Asoko* - "King Ashoka").
  - **Kanaganahalli Maha Stupa (Adholoka Maha Chaitya)**: Colossal drum-stupas covered in 60 limestone panels portraying Jataka tales, life of the Buddha, and Satavahana donors.
  - **Special Rock Edicts of Ashoka**: Major rock edicts XII and XIV inscribed in Prakrit on granite boulders.
- **Significance in Time Machine**: One of the most monumental archaeological discoveries in 20th-century India, providing an indisputable visual face to India's greatest Mauryan emperor.

---

### 2.5 Side-by-Side Archaeological Data Matrix

| Site Name | State / Region | Period / Civilization | Chronological Span | Primary Signature Artefact / Architectural Feat | 3D Asset Available |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Dholavira** | Gujarat, India | Mature Harappan | -3000 to -1500 | 10-Symbol Gypsum Signboard; 16 Rock Reservoirs | `stone_stele` |
| **Lothal** | Gujarat, India | Mature Harappan | -2400 to -1900 | World's 1st Tidal Dockyard; Persian Gulf Seal | `seal_cube` |
| **Rakhigarhi** | Haryana, India | Harappan Metropolis | -3300 to -1500 | Monumental Granary; Ancient DNA Grave I6113 | `stone_stele` |
| **Kalibangan** | Rajasthan, India | Harappan Civilization | -3000 to -1800 | Earliest Ploughed Field; Fire Altars (Havana Kundas) | `terracotta_tablet` |
| **Sinauli** | Uttar Pradesh, India | Copper Age Warrior | -2000 to -1800 | 3 Solid-Wheeled War Chariots; Copper Helmets | `bronze_chariot` |
| **Bhimbetka** | Madhya Pradesh, India| Mesolithic / Prehistoric | -100,000 to 1000 | 750 Rock Painted Shelters; Zoo Rock Hematite Art | `cave_art_slab` |
| **Pataliputra**| Bihar, India | Mauryan & Gupta Empire| -500 to 550 | 80-Pillared Polished Sandstone Hall; Wooden Walls | `mauryan_capital` |
| **Keeladi** | Tamil Nadu, India | Sangam Urban Horizon | -600 to 300 | Tamil-Brahmi inscribed potsherds; Ring Wells | `sangam_potsherd` |
| **Arikamedu** | Puducherry, India | Indo-Roman Port | -200 to 300 | Stamped Roman Amphorae; Terra Sigillata Ware | `pottery_amphora` |
| **Sannati** | Karnataka, India | Mauryan & Satavahana | -300 to 300 | Carved Portrait of Emperor Ashoka (*Ranyo Asoko*) | `ashokan_relief` |
| **Surkotada** | Gujarat, India | Harappan Garrison | -2300 to -1700 | Rubble Stone Fortifications; Equine Remains | `seal_cube` |
| **Inamgaon** | Maharashtra, India | Chalcolithic Jorwe | -1600 to -700 | Irrigation Canal Dam; Mother Goddess Figurine | `terracotta_goddess` |
| **Mohenjo-daro**| Sindh, Pakistan | Mature Harappan | -2500 to -1900 | The Great Bath; Dancing Girl Bronze Statuette | `dancing_girl_bronze` |
| **Harappa** | Punjab, Pakistan | Harappan Civilization | -3300 to -1300 | Great Granary; Circular Working Platforms | `seal_cube` |
| **Ur** | Dhi Qar, Iraq | Sumerian Civilization | -3800 to -500 | Great Ziggurat of Ur; Standard of Ur Mosaic | `cuneiform_tablet` |
| **Giza** | Cairo, Egypt | Old Kingdom Egypt | -2580 to -2150 | Great Pyramid of Khufu; The Sphinx | `pyramid_stele` |
| **Knossos** | Crete, Greece | Minoan Civilization | -2000 to -1380 | Labyrinthine Central Court; Snake Goddess | `pottery_amphora` |
| **Pompeii** | Campania, Italy | Roman Empire | -600 to 79 | Vesuvius Preserved Atrium Domus; Thermopolia | `roman_fresco` |

---

## 3. Web API Endpoints & Verification Guide

### 3.1 Live Endpoints (`http://localhost:5032`)

- `GET /api/sites`: Paginated list of archaeological sites with rich filters:
  - `?year=-2500` : Returns all sites active at 2500 BCE.
  - `?minYear=-2000&maxYear=-1000` : Temporal window query.
  - `?region=India` : **Prioritized query returning all Indian archaeological sites**.
  - `?search=chariot` : Full text keyword search across descriptions and highlights.
- `GET /api/sites/{id}` : Detailed profile containing stratigraphy, diagnostic artefacts, references, and contemporaneous co-existing sites.
- `GET /api/sites/{id}/nearby?radiusKm=500` : Geodesic distance query using spherical trigonometry and NetTopologySuite.
- `GET /api/sites/{id}/contemporaneous` : Co-existing settlements active during overlapping time horizons.
- `GET /api/sites/compare?site1Id={id1}&site2Id={id2}` : Side-by-side comparative analysis of two sites.
- `GET /api/civilizations` : Complete historical civilizations list with color hexes and descriptions.
- `GET /api/periods` : Epochs and historical periods.
- `GET /api/artefacts` : Diagnostic artefacts with 3D model attributes.

---

## 4. Changelog & System Updates

- **2026-09-28 [Backend]**: Implemented ASP.NET Core 10 Web API with NetTopologySuite, EF Core, REST controllers, and 16 passing unit tests.
- **2026-09-28 [Archaeology Research]**: Compiled comprehensive Indian regional compendium prioritizing 12 premier Indian archaeological sites spanning Paleolithic to Sangam horizons.
- **2026-09-28 [Database Engine]**: Expanded `DatabaseSeeder.cs` with full records for Rakhigarhi, Kalibangan, Sinauli, Bhimbetka, Pataliputra, Keeladi, Arikamedu, Sannati, Surkotada, and Inamgaon.
- **2026-09-28 [Documentation]**: Created live side-by-side technical & archaeological knowledge base (`INDIAN_ARCHAEOLOGY_AND_SYSTEM_PROGRESS.md`).
- **2026-09-28 [Frontend]**: Successfully built Angular 21 interactive GIS interface (`http://localhost:4200`) with Leaflet, continuous BCE/CE time machine scrubber, Three.js 3D WebGL PBR Artefact Lab, Analytical Comparator, and Indian Archaeology Priority presets. Both backend and frontend compiling and verified.
- **2026-09-28 [Design System & Interactive UI Overhaul]**: Redesigned UI to a museum-grade **White / Light Theme** with **Google Sans** typography (inspired by Google Arts & Culture's *Pyramids of Meroë*) and a floating **Zoom Earth interactive GIS HUD**:
  - **100% Free Architecture**: Zero subscriptions or paid API keys. Utilizes free public Esri World Imagery (satellite), CartoDB Voyager (cartography), OpenTopoMap (terrain), OpenStreetMap, and Three.js WebGL.
  - **Multi-Layer Switcher**: 1-click toggle between Carto Map 🗺️, Satellite Aerial 🛰️ (Esri), and Topographic Relief 🏔️.
  - **Zoom Earth HUD Controls**: Live mouse coordinate tracker (`Lat/Lng`), zoom level indicator, floating search pill, and clean floating player pill.
  - **Meroë 3D Gallery Studio**: Studio gallery lighting with bright soft key and ambient fill for Three.js 3D artefacts (seals, war chariots, stelae, potsherds).

---

## 5. Checkpoint & Break State

> **Checkpoint Timestamp**: September 28, 2026, 16:50 IST  
> **Status**: Active & Fully Operational. White theme & Zoom Earth HUD live.

### 5.1 Current System State
1. **ASP.NET Core 10 Backend API**: Running live on `http://localhost:5032` (PID daemon active).
   - 18 Archaeological Sites (12 prioritized Indian sites across all major horizons + 6 global comparative sites).
   - 9 Civilizations, 8 Historical Epochs, 12 Diagnostic Artefacts with 3D models.
   - All 16 Unit Tests passing cleanly.
2. **Angular 21 GIS Frontend**: Running live on `http://localhost:4200` (Daemon active).
   - **Theme**: Museum White Ivory (`#ffffff` / `#f8f9fa`) with deep slate text and terracotta/gold accents.
   - **Typography**: `Google Sans Display` & `Google Sans Text` from the Meroë project.
   - **Map Layers**: 100% Free Carto Voyager, Esri Satellite, and OpenTopoMap with Zoom Earth HUD.
   - **Time Scrubber**: Zoom Earth floating player pill with playback and keyframes.
   - **Indian Priority Bar**: 1-click presets for Indus Valley, Sinauli Chariots, Mauryan Empire, Sangam Keeladi, Prehistoric Rock Art.
   - **Production Bundle**: Passing with zero errors (`ng build` complete).
3. **Documentation**:
   - `docs/INDIAN_ARCHAEOLOGY_AND_SYSTEM_PROGRESS.md`: Full compendium and live progress tracker.
   - `docs/TASK_PROMPT_LOG.md`: Milestone logs and acceptance criteria checkpoints.
   - `docs/ARCHITECTURE.md` & `docs/DATABASE_SCHEMA.md`: System architecture and ER models.

### 5.2 Resumption Checklist (When Returning)
1. **Verify Services**:
   - Backend API: `http://localhost:5032` (or launch with `dotnet run --project backend/src/ArchaeologicalTimeMachine.Api --urls "http://localhost:5032"`)
   - Frontend UI: `http://localhost:4200` (or launch with `npx ng serve --port 4200` inside `frontend/archaeological-time-machine`)
2. **Interactive UI Walkthrough**:
   - Run browser testing / visual demo of map markers, 3D artefact inspection, and side-by-side comparison.
3. **Next Feature Enhancements**:
   - Fine-tuning micro-animations and soundscapes (ancient wind/ambient tones if desired).
   - Student Mode / Interactive Archaeological Quiz Mode.

