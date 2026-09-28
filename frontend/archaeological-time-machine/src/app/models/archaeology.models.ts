export interface SiteCivilization {
  civilizationId: number;
  civilizationName: string;
  colorHex?: string;
  isPrimary: boolean;
}

export interface SitePeriod {
  periodId: number;
  periodName: string;
  epoch?: string;
}

export interface SiteSummary {
  id: number;
  name: string;
  ancientName?: string;
  slug: string;
  region: string;
  country: string;
  siteType: string;
  latitude: number;
  longitude: number;
  startYear: number;
  endYear: number;
  startYearFormatted: string;
  endYearFormatted: string;
  datingPrecision?: string;
  imageUrl?: string;
  isUnescoWorldHeritage: boolean;
  civilizations: SiteCivilization[];
  periods: SitePeriod[];
}

export interface Artefact {
  id: number;
  siteId: number;
  name: string;
  artefactType: string;
  material: string;
  approximateYear?: number;
  approximateYearFormatted?: string;
  dimensions?: string;
  description: string;
  imageUrl?: string;
  currentLocation?: string;
  discoveryContext?: string;
  model3DType?: string;
}

export interface Finding {
  id: number;
  name: string;
  findingType: string;
  description: string;
  yearFound?: number;
}

export interface ExcavationLayer {
  id: number;
  layerNumber: number;
  layerName: string;
  depthMeters: number;
  soilComposition?: string;
  estimatedStartYear?: number;
  estimatedEndYear?: number;
  estimatedStartYearFormatted?: string;
  estimatedEndYearFormatted?: string;
  culturalAffiliation?: string;
  description: string;
  findings: Finding[];
}

export interface Excavation {
  id: number;
  expeditionName: string;
  leadArchaeologist: string;
  startYear: number;
  endYear?: number;
  organization: string;
  summary: string;
  layers: ExcavationLayer[];
}

export interface ReferenceCitation {
  id: number;
  citationKey: string;
  authors: string;
  publicationYear: number;
  title: string;
  journalOrPublisher: string;
  url?: string;
  specificPagesOrPlates?: string;
}

export interface SiteRelationshipDetail {
  relatedSiteId: number;
  relatedSiteName: string;
  relatedSiteRegion: string;
  relatedSiteCountry: string;
  relationshipType: string;
  description: string;
  direction: string;
}

export interface SiteDetail extends SiteSummary {
  description: string;
  discoveryInformation?: string;
  excavationStatus?: string;
  waterSource?: string;
  architecturalHighlights?: string;
  artefacts: Artefact[];
  excavations: Excavation[];
  references: ReferenceCitation[];
  relationships: SiteRelationshipDetail[];
}

export interface NearbySiteResult {
  sourceSiteId: number;
  sourceSiteName: string;
  nearbySiteId: number;
  nearbySiteName: string;
  region: string;
  country: string;
  distanceKm: number;
  startYear: number;
  endYear: number;
  startYearFormatted: string;
  endYearFormatted: string;
}

export interface SiteComparisonResult {
  site1: SiteDetail;
  site2: SiteDetail;
  geodesicDistanceKm: number;
  temporalOverlapYears: number;
  hasTemporalOverlap: boolean;
  overlapSpanText: string;
  directRelationships: {
    relationshipType: string;
    description: string;
    direction: string;
  }[];
}

export interface Civilization {
  id: number;
  name: string;
  slug: string;
  region: string;
  startYear: number;
  endYear: number;
  startYearFormatted: string;
  endYearFormatted: string;
  colorHex: string;
  primaryLanguage?: string;
  architecturalTradition?: string;
  description: string;
}

export interface HistoricalPeriod {
  id: number;
  name: string;
  slug: string;
  epoch: string;
  startYear: number;
  endYear: number;
  startYearFormatted: string;
  endYearFormatted: string;
  description: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
