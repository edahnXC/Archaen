# Archaeological Time Machine - Database Schema & Data Dictionary

## 1. Relational Entity-Relationship Model

```mermaid
erDiagram
    Civilization ||--o{ SiteCivilization : associates
    Site ||--o{ SiteCivilization : has
    HistoricalPeriod ||--o{ SitePeriod : categorizes
    Site ||--o{ SitePeriod : has
    Site ||--o{ Excavation : contains
    Excavation ||--o{ ExcavationLayer : stratifies
    ExcavationLayer ||--o{ Finding : reveals
    Site ||--o{ Artefact : yields
    Site ||--o{ SiteReference : cites
    Reference ||--o{ SiteReference : referenced_in
    Site ||--o{ SiteRelationship : related_from
    Site ||--o{ SiteRelationship : related_to

    Site {
        int Id PK
        string Name
        string AncientName
        string Slug
        string Description
        string Region
        string Country
        string SiteType
        double Latitude
        double Longitude
        int StartYear
        int EndYear
        string DatingMethod
        string ExcavationStatus
        string WaterSource
        string ArchitecturalHighlights
        string ImageUrl
        bool IsUnescoWorldHeritage
    }

    Civilization {
        int Id PK
        string Name
        string Region
        int StartYear
        int EndYear
        string Description
        string ColorHex
        string PrimaryLanguage
    }

    HistoricalPeriod {
        int Id PK
        string Name
        string Epoch
        int StartYear
        int EndYear
        string Description
    }

    Artefact {
        int Id PK
        int SiteId FK
        string Name
        string ArtefactType
        string Material
        int ApproximateYear
        string Dimensions
        string Description
        string CurrentLocation
        string DiscoveryContext
        string ImageUrl
        string Model3DUrl
    }

    Excavation {
        int Id PK
        int SiteId FK
        string ExpeditionName
        string LeadArchaeologist
        int StartYear
        int EndYear
        string Organization
        string Summary
    }

    ExcavationLayer {
        int Id PK
        int ExcavationId FK
        int LayerNumber
        string LayerName
        double DepthMeters
        string SoilComposition
        int EstimatedStartYear
        int EstimatedEndYear
        string CulturalAffiliation
        string Description
    }

    Finding {
        int Id PK
        int ExcavationLayerId FK
        string Name
        string FindingType
        string Description
        int YearFound
    }

    Reference {
        int Id PK
        string Citation
        string Authors
        int PublicationYear
        string Title
        string JournalOrBook
        string DoiOrIsbn
        string Url
    }

    SiteRelationship {
        int Id PK
        int SourceSiteId FK
        int TargetSiteId FK
        string RelationshipType
        string Description
    }
```

## 2. Key Archaeological Informatics Design Principles
1. **Signed Temporal Indexing**: `StartYear` and `EndYear` on both `Site`, `Civilization`, `HistoricalPeriod`, and `ExcavationLayer` allow blazing fast interval overlapping queries: `StartYear <= targetYear AND EndYear >= targetYear`.
2. **Spatial Geographic Coordinates**: Precision `Latitude` and `Longitude` with NetTopologySuite spherical calculations (Haversine / geodesic distance in km for "Nearby sites" queries).
3. **Stratigraphic Sequence**: The `ExcavationLayer` entities allow visual representation of archaeological stratigraphy according to the law of superposition (Layer I at top, deeper layers below).
4. **Citation & Provenance**: Academic citations (`Reference`) are linked to archaeological sites through `SiteReference` ensuring scholarly veracity.
