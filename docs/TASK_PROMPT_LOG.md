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
