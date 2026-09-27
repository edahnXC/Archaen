# Archaeological Time Machine - System Architecture

> "Explore the archaeological landscape across time."

## 1. Executive Overview
**Archaeological Time Machine** is an interactive archaeological Geographic Information System (GIS) and 4D temporal exploration platform. It enables researchers, students, and archaeology enthusiasts to navigate global archaeological landscapes through both space (geographic coordinates, spatial proximity, regional clusters) and time (continuous BCE/CE chronological progression).

---

## 2. Core Pillars
1. **Spatio-Temporal Fusion**: Every archaeological site is characterized by physical coordinates (`Latitude`, `Longitude`, SRID 4326 Point geometry) and a chronological span (`StartYear`, `EndYear` in continuous astronomical integer numbering where $-3000 = 3000\text{ BCE}$, $100 = 100\text{ CE}$).
2. **Archaeological Informatics**: Normalized domain entities representing Civilizations, Historical Periods, Excavations, Stratigraphic Horizons/Layers, Discoveries, Diagnostic Artefacts, and Academic Citations.
3. **Interactive 4D GIS**: High-performance interactive Leaflet map linked to a temporal scrubbing engine, dynamic period filtering, site clustering, and cross-site comparative analysis.
4. **Academic Integrity**: Real, verifiable archaeological sites with documented excavation history, stratigraphy, diagnostic artefacts, and academic bibliography.

---

## 3. High-Level Architecture Diagram

```
+---------------------------------------------------------------------------------+
|                                CLIENT LAYER (Angular 21)                        |
|                                                                                 |
|  +---------------------+   +-----------------------+   +---------------------+  |
|  |   Interactive GIS   |   | Temporal Time Machine |   |    Site Explorer    |  |
|  |  (Leaflet / GeoJSON)|   |  (BCE/CE Slider/Play) |   | (Stratigraphy / 3D) |  |
|  +----------+----------+   +-----------+-----------+   +----------+----------+  |
|             \                          |                         /              |
|              +-------------------------+------------------------+               |
|                                        |                                        |
|                          Angular State & API Services                           |
+----------------------------------------+----------------------------------------+
                                         | HTTP / JSON REST
                                         v
+---------------------------------------------------------------------------------+
|                       BACKEND LAYER (ASP.NET Core 10 Web API)                   |
|                                                                                 |
|  +---------------------------------------------------------------------------+  |
|  |                                Controllers                                |  |
|  |  [SitesController] [CivilizationsController] [ArtefactsController] ...   |  |
|  +-------------------------------------+-------------------------------------+  |
|                                        |                                        |
|  +-------------------------------------+-------------------------------------+  |
|  |                           Application & DTO Layer                         |  |
|  |  Temporal Query Validation | Spatial Distance Calculators | Filtering     |  |
|  +-------------------------------------+-------------------------------------+  |
|                                        |                                        |
|  +-------------------------------------+-------------------------------------+  |
|  |                      Domain & Infrastructure (EF Core)                    |  |
|  |   ArchaeologyDbContext | NetTopologySuite Geometry | Spatial Indexes      |  |
|  +-------------------------------------+-------------------------------------+  |
+----------------------------------------+----------------------------------------+
                                         |
                                         v
+---------------------------------------------------------------------------------+
|                            DATA STORAGE LAYER                                   |
|                                                                                 |
|   - Primary: SQLite with Spatial / NetTopologySuite (Zero-config local run)    |
|   - Enterprise/Production: PostgreSQL 16 + PostGIS (Configurable via ConnStr)   |
+---------------------------------------------------------------------------------+
```

---

## 4. Temporal Model Design
Historical dates span both BCE (Before Common Era) and CE (Common Era).
- **Storage**: Signed integers representing astronomical years:
  - $3000\text{ BCE} \rightarrow -3000$
  - $2500\text{ BCE} \rightarrow -2500$
  - $500\text{ BCE} \rightarrow -500$
  - $44\text{ BCE} \rightarrow -44$
  - $1\text{ CE} \rightarrow 1$
  - $200\text{ CE} \rightarrow 200$
- **Temporal Query Predicate**:
  A site active from $[S_{\text{start}}, S_{\text{end}}]$ is active at target year $T$ if:
  $$S_{\text{start}} \le T \le S_{\text{end}}$$
  A site active from $[S_{\text{start}}, S_{\text{end}}]$ overlaps a time window $[W_{\text{start}}, W_{\text{end}}]$ if:
  $$S_{\text{start}} \le W_{\text{end}} \land S_{\text{end}} \ge W_{\text{start}}$$
- **Display Formatting**:
  - Positive $Y$: `"{Y} CE"` (e.g. `"150 CE"`)
  - Negative $Y$: `"{|Y|} BCE"` (e.g. `"2500 BCE"`)

---

## 5. Technology Stack Decisions
- **Backend**: ASP.NET Core 10 Web API (.NET 10.0), C#, Entity Framework Core, NetTopologySuite for spatial calculations.
- **Frontend**: Angular 21 (Standalone Components, Signals, modern reactive UI, Leaflet with custom archaeological SVG markers, Three.js 3D artefact inspection).
- **Database**: SQLite with NetTopologySuite (default out-of-the-box zero-setup), fully compatible with PostgreSQL/PostGIS.
