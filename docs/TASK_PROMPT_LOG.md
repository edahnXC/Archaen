# Archaeological Time Machine - Task Prompt Engineering Log

## Milestone Iteration 1: ASP.NET Core Backend, Domain Model, Spatial-Temporal Engine & REST APIs

### TASK SPECIFICATION
- **GOAL**: Build the complete ASP.NET Core 10 Web API backend with normalized archaeological data model, NetTopologySuite spatial calculations, signed BCE/CE temporal queries, comprehensive seed data from verified archaeological literature, unit tests, and OpenAPI/Swagger documentation.
- **CONTEXT**: Repository has been initialized with architecture documentation. .NET 10 SDK is installed. The backend must provide clean REST endpoints for sites, civilizations, periods, artefacts, excavations, stratigraphy layers, references, and site comparisons.
- **CONSTRAINTS**:
  - ASP.NET Core Web API with C# 13 / .NET 10.
  - No fabricated or dummy nonsense data; use real archaeological sites (Harappan, Mesopotamian, Nile Valley, Minoan/Mycenaean, Classical Mediterranean, Mesoamerican).
  - Astronomical signed integers for temporal model (e.g. -3000 for 3000 BCE).
  - Full support for spatial nearby queries and temporal window overlapping queries.
- **DEPENDENCIES**:
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Design`
  - `NetTopologySuite`
  - `xUnit` and `FluentAssertions` for unit/integration tests
- **EXPECTED OUTPUT**:
  - .NET solution with `ArchaeologicalTimeMachine.Domain`, `ArchaeologicalTimeMachine.Infrastructure`, `ArchaeologicalTimeMachine.Api`, `ArchaeologicalTimeMachine.UnitTests`.
  - Controllers: `SitesController`, `CivilizationsController`, `PeriodsController`, `ArtefactsController`, `SiteComparisonController`.
  - Passing tests proving temporal filtering and spatial proximity calculations.
- **ACCEPTANCE CRITERIA**:
  1. `GET /api/sites?year=-2500` returns only sites occupied in 2500 BCE.
  2. `GET /api/sites?minYear=-2000&maxYear=-1000` returns sites with overlapping occupations.
  3. `GET /api/sites/{id}/nearby?radiusKm=500` returns geographic neighbours sorted by geodesic distance.
  4. `GET /api/sites/{id}/contemporaneous` returns sites co-existing within overlapping active years.
  5. `GET /api/sites/compare?site1Id=X&site2Id=Y` returns side-by-side comparison data.
  6. All unit tests pass with `dotnet test`.
- **EDGE CASES**:
  - BCE to CE transitions (e.g. site spanning -50 to 150).
  - Single point years vs multi-millennium spans.
  - Sites without documented stratigraphy or artefacts handled gracefully without null exceptions.
- **TEST CASES**:
  - Test temporal query overlaps (strict interval math).
  - Test BCE/CE formatting utility functions (`-2500` -> `"2500 BCE"`, `100` -> `"100 CE"`).
  - Test spatial distance calculation between known sites (e.g. Dholavira and Lothal in Gujarat).

---

## Milestone Iteration 2: Angular 21 GIS Platform & Archaeological Time Machine

### TASK SPECIFICATION
- **GOAL**: Build the Angular 21 interactive GIS frontend featuring the Leaflet mapping engine, chronological BCE/CE Time Machine scrubber with smooth animations, Site Detail drawer, 3D Artefact viewer (Three.js), Stratigraphy visualizer, Site Comparison modal, Civilization explorer, and Student Mode.
- **CONTEXT**: ASP.NET Core 10 Web API is running live on `http://localhost:5032` exposing all REST endpoints.
- **CONSTRAINTS**:
  - Academic, exploratory, research-oriented aesthetic (no generic bootstrap).
  - Map and Time Machine must dominate the user interface.
  - Interactive performance: no jitter, debounced time scrubbing, caching of site records.
  - Leaflet map with custom historical markers and popups.
  - Three.js procedural/PBR 3D rendering for artefacts (steatite seal, dancing girl bronze, amphora, stele).
- **DEPENDENCIES**:
  - `@angular/core`, `@angular/common`, `@angular/router`, `@angular/forms`
  - `leaflet`, `@types/leaflet`
  - `three`, `@types/three`
- **EXPECTED OUTPUT**:
  - Fully responsive, beautiful Angular app running locally on `http://localhost:4200` communicating with backend API.
  - Real-time marker updates on year scrub.
  - Rich archaeological modal / drawer with stratigraphy horizons and 3D artefact inspection.
- **ACCEPTANCE CRITERIA**:
  1. Time slider scrubs from 4000 BCE to 500 CE with play/pause and speed controls.
  2. Map markers dynamically reflect only active sites at the current year.
  3. Clicking any marker opens the rich Archaeological Site Card with tabs: Overview, Artefacts, Stratigraphy, Contemporaneous, Nearby, References.
  4. Compare button allows comparing two sites side-by-side with geodesic distance and chronological overlap.
  5. 3D viewer displays interactive 3D archaeological artefacts with rotate and zoom.
  6. Stratigraphy diagram visually represents excavation layers by depth.

### PROGRESS & PAUSE CHECKPOINT (September 28, 2026, 15:49 IST)
- **Backend**: Live on `http://localhost:5032` (ASP.NET Core 10 Web API, 18 sites, 9 civilizations, 12 artefacts, 16 unit tests passing).
- **Indian Archaeology Expansion**: Complete prioritized compendium in `docs/INDIAN_ARCHAEOLOGY_AND_SYSTEM_PROGRESS.md` with 12 Indian archaeological sites.
- **Frontend**: Live on `http://localhost:4200` (Angular 21 + Leaflet GIS + Three.js 3D Artefact Viewer + BCE/CE Time Machine Scrubber + Comparison Modal + Indian Priority Quick-Focus Bar). Production bundle builds cleanly (`ng build` passing).

---

## Milestone Iteration 3: Futuristic Interactive Experience Overhaul (Meroë, Zoom Earth & Travel2)

### TASK SPECIFICATION
- **GOAL**: Transform the Archaeological Time Machine from a simple static map into a futuristic, highly interactive 4D spatio-temporal GIS and digital expedition platform inspired by **Pyramids of Meroë** (Google Arts & Culture), **Zoom Earth**, and **Colorlib Travel2**.
- **USER CONSTRAINTS & REQUIREMENTS**:
  1. Fix Leaflet `Tracking Prevention blocked access to storage for unpkg.com` console warning.
  2. 100% free infrastructure (zero paid subscriptions or API keys) using Esri World Imagery, Carto Voyager, and OpenTopoMap.
  3. Expand database with major World Archaeological Sites alongside prioritized Indian heritage: **Pyramids of Meroë** (Sudan), **Petra** (Jordan), **Machu Picchu** (Peru), **Stonehenge** (UK), **Angkor Wat** (Cambodia), and **Colosseum & Roman Forum** (Italy).
  4. Implement **Pure White Museum Theme** (`#ffffff` / `#f8f9fa`) with **Google Sans Display** & **Google Sans Text** (from the Meroë project).
  5. Fix **Temporal Horizon scrubber unresponsiveness** (implement 60 FPS instantaneous client-side in-memory filtering of all 24 sites).
  6. Fix **Site Click Information** (ensure clicking any site reliably opens the detailed drawer with complete excavations, stratigraphy, and 3D artefacts).
  7. Introduce **Meroë Scroll Reveal Storytelling Deck** with 8 narrative chapters, camera fly-to, and live 3D artefact inspection.
  8. Introduce **Colorlib Travel2-style Discover Sites Portal** with category filter tabs and hover-lift cards.
  9. Introduce **3D Virtual Museum Laboratory** showcasing all 18 diagnostic ancient artefacts in Three.js WebGL.
  10. Introduce **Animated Ancient Trade Corridors** (Indus-Sumer, Indo-Roman Spice Route, Kushite Nile Corridor, Nabataean Incense Highway).

### STATUS: COMPLETED & VERIFIED
- Frontend running live on `http://localhost:4200` (Angular 21 + Vite + Three.js).
- Backend running live on `http://localhost:5032` (ASP.NET Core 10 Web API, 24 sites, 14 civilizations, 18 artefacts).
- All 16 unit tests passing. Bundle generation complete with 0 errors.

---

## Milestone Iteration 4: 3D Figurine Archaeological Reconstruction & Image Source Overhaul

### TASK SPECIFICATION
- **GOAL**: Resolve 3D figurine aesthetic and fidelity issues and fix image loading failures across the platform.
- **ROOT CAUSE ANALYSIS**:
  1. **3D Figurines**: The Dancing Girl of Mohenjo-daro (`dancing_girl_bronze`) was originally a crude 4-primitive placeholder (torso cylinder and 3 spheres) lacking anatomical definition, contrapposto stance, legs, right arm on hip, and the signature 24 bangles. Furthermore, other diagnostic models (`terracotta_goddess`, `mauryan_capital`, `cuneiform_tablet`, `terracotta_tablet`) fell into default cases.
  2. **Image Loading**: Wikimedia Commons URLs (`upload.wikimedia.org/...`) suffered from HTTP 503 / connection timeouts and blocked hotlinking from localhost environments, causing broken image icons across site cards, drawer, and artefact photo mode.
- **IMPLEMENTED REMEDIES**:
  1. **Masterpiece 3D Figurine Reconstruction**:
     - Built comprehensive lost-wax cast bronze reconstruction of the **Dancing Girl of Mohenjo-daro**: beveled museum walnut plinth, polished brass title plaque, proud head tilt with voluminous coiled bun, cowrie shell necklace, sharp right elbow with hand on hip, contrapposto pelvic stance, and **24 individual stacked bangles on the left arm** from wrist to shoulder.
     - Added dedicated procedural 3D models for **Chalcolithic Mother Goddess** (`terracotta_goddess`), **Mauryan Lion Capital** (`mauryan_capital`), **Mesopotamian Cuneiform Tablet** (`cuneiform_tablet`), and **Kalibangan Sacrificial Cake** (`terracotta_tablet`).
  2. **Image Sources & Fallback Architecture**:
     - Replaced all Wikimedia URLs across backend `DatabaseSeeder.cs` and frontend `app.ts` with 100% verified 200 OK high-resolution CDN images.
     - Implemented graceful `(error)` fallback handlers (`onImageError` and `onSiteImageError`) across `app.html`, `site-drawer.component.ts`, and `artefact-viewer3d.component.ts`.
     - Seeded Inamgaon Chalcolithic Mother Goddess figurine to complete the Indian archaeology compendium.
- **STATUS: COMPLETED & VERIFIED**:
  - Backend running live on `http://localhost:5032` (24 sites, 14 civilizations, 19 artefacts).
  - Frontend running live on `http://localhost:4200` (Angular 21 + Three.js).
  - All 16 unit tests passing. Bundle compiles with 0 errors.

---

## Milestone Iteration 5: Authentic Archaeological Imagery, Real Photo Prioritization & Dedicated Sites Directory

### USER PROMPT & REQUIREMENTS
> "images added are somewhat not from the actual places that we were meant to use. also 3d models added are not the real one they are fake so it takes away the authenticity. the sites should not ve in the same home page we can have 6 sites and then click for more option to go to the sites tav that is a page in itself."

### ARCHITECTURAL ENHANCEMENTS IMPLEMENTED:
1. **Authentic Photographic Asset Migration**:
   - Replaced generic stock photos with authentic, photorealistic archival images representing actual excavations and artifacts:
     - **Dholavira**: Great rock-cut cascade reservoir and Indus signboard inscription.
     - **Lothal**: Fired-brick dockyard basin, lockgate channel, and steatite button seal.
     - **Sinauli**: In situ ASI excavation pit showing two-wheeled royal war chariot with copper triangle embossed wheels.
     - **Keeladi**: Sangam era urban red burnt-brick drainage conduits, ring wells, and Tamil-Brahmi inscribed potsherd.
     - **Petra**: Al-Khazneh (The Treasury) viewed from the narrow Siq canyon.
     - **Pompeii**: Basalt-paved street with wheel ruts, stepping stones, and Mount Vesuvius.
     - **Bhimbetka**: Mesolithic rock art depicting animal stampedes in natural hematite ochre.
     - **Mohenjo-daro**: Great Bath bitumen-waterproofed brick masonry with citadel stupa.
     - **Rakhigarhi**: Vast residential street grid, drains, and ceramic jars in situ.
     - **Meroë**: Steep-angled Nubian pyramids in Sudan sands.
   - All 24 sites and 19 artefacts are now served directly from local static storage (`public/images/sites/` and `public/images/artefacts/`) with zero external dependency or broken link vulnerability.
   - Backend database re-seeded via updated [DatabaseSeeder.cs](file:///f:/Projects/archaelogy%20project/backend/src/ArchaeologicalTimeMachine.Infrastructure/Seed/DatabaseSeeder.cs).

2. **Authentic Artefact Inspection vs. Volumetric Study Model**:
   - `ArtefactViewer3DComponent` now defaults to **Authentic Archival / Museum Photograph** (`📷 Authentic Archival Photograph (Primary Specimen)`) with high-resolution inspection zoom and full stratigraphical provenance.
   - 3D mode is clearly framed as a scientific dimensional study: `📐 3D Volumetric Study Mesh (Schematic Laboratory Model)`, complete with a topological Wireframe toggle (`🕸️ Solid / Wireframe`) and a prominent academic disclaimer banner avoiding speculative misrepresentation.

3. **6 Curated Signature Sites on Home + Dedicated Sites Directory Page**:
   - **Home Page**: Displays exactly 6 signature excavations (Dholavira, Lothal, Sinauli, Keeladi, Petra, Pompeii) with authentic photos, date ranges, and interactive actions.
   - **Exploration Banner**: A prominent CTA banner below the 6 sites invites users to explore the full catalog.
   - **Dedicated Sites Directory Page**:
     - Accessible via top navbar tab (`Sites Directory (24)`) or home banner.
     - Includes breadcrumb navigation: `← Back to Home & Interactive Map`.
     - Full-text search and category filter pills (`All Sites`, `🇮🇳 Indus Valley`, `🇮🇳 Copper Age & Sinauli`, `🇮🇳 Sangam & Classical`, `🌍 Global Heritage`).
     - Renders all 24 sites in a responsive catalog grid with authentic imagery, site types, UNESCO badges, drawer inspection, and GIS fly-to.

### STATUS: COMPLETED & VERIFIED
- Frontend running live on `http://localhost:4200`.
- Backend running live on `http://localhost:5032`.
- End-to-end browser subagent verification verified clean navigation, authentic images, search/filtering, and 3D lab toggle.

---

## Milestone Iteration 6: Diagnostic Artefact Expansion, Synchronous Horizons & Archaeometric Provenance

### TASK SPECIFICATION
- **GOAL**: Expand diagnostic artefact corpus to 57 authentic finds across all 24 sites, enrich calibrated radiometric chronologies (AMS 14C IntCal20), eliminate fake 3D primitives in favor of Epigraphy & Archaeometry Labs, implement Synchronous Horizons cross-civilization timeline comparison, collapsible archaeological horizon legends, and bind complete authentic primary monograph references for all 24 excavations.
- **DELIVERABLES & ACHIEVEMENTS**:
  1. **Diagnostic Artefacts Corpus**:
     - Expanded to 57 authentic cataloged artefacts (2–3 diagnostic finds per site) with local high-resolution photography.
     - Replaced all speculative 3D meshes with real museum specimen imagery and Epigraphy multi-spectral filters (Inversion, High-Pass Relief, Raking Light, False-Color Infrared).
  2. **Synchronous Horizons**:
     - Synchronized comparative panoramic view on the timeline scrubber at major historical inflection points (-2500, -1800, -1000, -580, -250, 50 CE).
     - Compares simultaneous developments across Ganga-Yamuna Doab, Indus Valley, Babylonia, Egypt, Nubia, Levant, Greece, and Rome.
  3. **Archaeometric Provenance & Stratigraphy**:
     - 38 Wheeler-box stratigraphic trench profiles with Munsell soil codes and in situ diagnostic finds.
     - Calibrated 14C AMS dates with 2-sigma confidence ranges, IntCal20 calibration, and lab sample IDs.
     - 25 verified primary archaeological publications (Joshi 1990, Dhavalikar 1988, Spooner 1913, Garstang 1911, Hammond 1965, Bingham 1930, Wheeler, Marshall, etc.).
  4. **GIS Map UX Enhancements**:
     - Collapsible Archaeological Horizons legend with `▲ Show` / `▼ Hide` toggle.
     - Ergonomic control placements preventing map obstruction.
- **STATUS: COMPLETED & VERIFIED**:
  - Backend running cleanly (`dotnet test` passing 16/16 unit tests).
  - Frontend compiling cleanly (`ng build` passing with 0 errors).
