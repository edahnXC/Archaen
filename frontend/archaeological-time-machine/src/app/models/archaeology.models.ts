export interface SiteCivilization {
  id: number;
  name: string;
  slug?: string;
  colorHex?: string;
  isPrimary?: boolean;
  // Backward compatibility alias helpers
  civilizationId?: number;
  civilizationName?: string;
}

export interface SitePeriod {
  id: number;
  name: string;
  slug?: string;
  epoch?: string;
  // Backward compatibility alias helpers
  periodId?: number;
  periodName?: string;
}

export interface SiteSummary {
  id: number;
  name: string;
  ancientName?: string;
  slug: string;
  description?: string;
  region: string;
  country: string;
  siteType: string;
  latitude: number;
  longitude: number;
  startYear: number;
  endYear: number;
  startYearFormatted: string;
  endYearFormatted: string;
  chronologicalSpan?: string;
  datingPrecision?: string;
  excavationStatus?: string;
  waterSource?: string;
  architecturalHighlights?: string;
  imageUrl?: string;
  isUnescoWorldHeritage: boolean;
  civilizations: SiteCivilization[];
  periods: SitePeriod[];
}

export interface Artefact {
  id: number;
  siteId: number;
  siteName?: string;
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
  chronologicalSpan?: string;
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
  doiOrIsbn?: string;
  url?: string;
  specificPagesOrPlates?: string;
}

export interface SiteRelationshipDto {
  id: number;
  relatedSiteId: number;
  relatedSiteName: string;
  relatedSiteSlug: string;
  relationshipType: string;
  description: string;
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
  outgoingRelationships: SiteRelationshipDto[];
  incomingRelationships: SiteRelationshipDto[];
  // Unified alias for UI templates
  relationships?: SiteRelationshipDto[];
}

export interface NearbySiteResult {
  id: number;
  name: string;
  slug: string;
  region: string;
  country: string;
  siteType: string;
  distanceKm: number;
  latitude: number;
  longitude: number;
  chronologicalSpan: string;
  imageUrl?: string;
  // Aliases for template compatibility
  nearbySiteId?: number;
  nearbySiteName?: string;
  startYearFormatted?: string;
  endYearFormatted?: string;
}

export interface ContemporaneousSiteResult {
  id: number;
  name: string;
  slug: string;
  region: string;
  country: string;
  siteType: string;
  overlapYears: number;
  chronologicalSpan: string;
  latitude: number;
  longitude: number;
  imageUrl?: string;
  // Aliases for template compatibility
  startYearFormatted?: string;
  endYearFormatted?: string;
}

export interface SiteComparisonResult {
  site1: SiteDetail;
  site2: SiteDetail;
  distanceKm: number;
  chronologicalOverlapYears: number;
  isContemporaneous: boolean;
  sharedCivilizations: string[];
  sharedPeriods: string[];
  // Aliases for compatibility
  geodesicDistanceKm?: number;
  temporalOverlapYears?: number;
  hasTemporalOverlap?: boolean;
  overlapSpanText?: string;
  directRelationships?: {
    relationshipType: string;
    description: string;
    direction?: string;
  }[];
}

export interface Civilization {
  id: number;
  name: string;
  slug: string;
  region: string;
  startYear: number;
  endYear: number;
  chronologicalSpan?: string;
  startYearFormatted?: string;
  endYearFormatted?: string;
  colorHex: string;
  primaryLanguage?: string;
  architecturalTradition?: string;
  description: string;
  siteCount?: number;
}

export interface HistoricalPeriod {
  id: number;
  name: string;
  slug: string;
  epoch: string;
  startYear: number;
  endYear: number;
  chronologicalSpan?: string;
  description: string;
  siteCount?: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

