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
- **Indian Archaeology Expansion**: Complete prioritized compendium in `docs/INDIAN_ARCHAEOLOGY_AND_SYSTEM_PROGRESS.md` with 12 Indian archaeological sites (Rakhigarhi, Kalibangan, Sinauli, Bhimbetka, Pataliputra, Keeladi, Arikamedu, Sannati, Dholavira, Lothal, Surkotada, Inamgaon).
- **Frontend**: Live on `http://localhost:4200` (Angular 21 + Leaflet GIS + Three.js 3D Artefact Viewer + BCE/CE Time Machine Scrubber + Comparison Modal + Indian Priority Quick-Focus Bar). Production bundle builds cleanly (`ng build` passing).
- **Status**: PAUSED FOR COMPREHENSIVE BREAK.
- **Next Steps Upon Resuming**:
  1. Interactive browser UX walk-through and verification of Leaflet markers, 3D WebGL rotation, and side-by-side comparison modal.
  2. Polish fine UI animations (marker pulsing, time scrubber transitions).
  3. Student Mode / Quiz Mode features if desired.


