import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, tap, throwError } from 'rxjs';
import {
  SiteSummary,
  SiteDetail,
  NearbySiteResult,
  ContemporaneousSiteResult,
  SiteComparisonResult,
  Civilization,
  HistoricalPeriod,
  Artefact,
  PagedResult
} from '../models/archaeology.models';

@Injectable({
  providedIn: 'root'
})
export class ArchaeologyApiService {
  private readonly http = inject(HttpClient);
  public readonly baseUrl = 'http://localhost:5032/api';
  public readonly isApiOnline = signal<boolean>(true);

  /**
   * Helper to normalize civilization & period tags across models
   */
  private normalizeSiteSummary(s: SiteSummary): SiteSummary {
    if (s.civilizations) {
      s.civilizations = s.civilizations.map(c => ({
        ...c,
        civilizationId: c.id ?? c.civilizationId,
        civilizationName: c.name ?? c.civilizationName
      }));
    }
    if (s.periods) {
      s.periods = s.periods.map(p => ({
        ...p,
        periodId: p.id ?? p.periodId,
        periodName: p.name ?? p.periodName
      }));
    }
    return s;
  }

  private normalizeSiteDetail(detail: SiteDetail): SiteDetail {
    this.normalizeSiteSummary(detail);
    const combined = [
      ...(detail.outgoingRelationships || []),
      ...(detail.incomingRelationships || [])
    ];
    detail.relationships = combined;
    return detail;
  }

  /**
   * Fetches sites with dynamic filtering: year, range, civilization, region, search.
   */
  getSites(params?: {
    year?: number;
    minYear?: number;
    maxYear?: number;
    civilizationId?: number;
    periodId?: number;
    region?: string;
    search?: string;
    page?: number;
    pageSize?: number;
  }): Observable<PagedResult<SiteSummary>> {
    let httpParams = new HttpParams();
    if (params) {
      if (params.year !== undefined && params.year !== null) httpParams = httpParams.set('year', params.year.toString());
      if (params.minYear !== undefined && params.minYear !== null) httpParams = httpParams.set('minYear', params.minYear.toString());
      if (params.maxYear !== undefined && params.maxYear !== null) httpParams = httpParams.set('maxYear', params.maxYear.toString());
      if (params.civilizationId) httpParams = httpParams.set('civilizationId', params.civilizationId.toString());
      if (params.periodId) httpParams = httpParams.set('periodId', params.periodId.toString());
      if (params.region) httpParams = httpParams.set('region', params.region);
      if (params.search) httpParams = httpParams.set('search', params.search);
      if (params.page) httpParams = httpParams.set('page', params.page.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    }

    return this.http.get<PagedResult<SiteSummary>>(`${this.baseUrl}/sites`, { params: httpParams }).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(res => ({
        ...res,
        items: (res.items || []).map(s => this.normalizeSiteSummary(s))
      })),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Gets full detailed site profile including excavations, stratigraphy, artefacts, and citations.
   */
  getSiteById(id: number): Observable<SiteDetail> {
    return this.http.get<SiteDetail>(`${this.baseUrl}/sites/${id}`).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(detail => this.normalizeSiteDetail(detail)),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Gets geographic neighbours within radiusKm.
   */
  getNearbySites(id: number, radiusKm: number = 1000): Observable<NearbySiteResult[]> {
    const params = new HttpParams().set('radiusKm', radiusKm.toString());
    return this.http.get<NearbySiteResult[]>(`${this.baseUrl}/sites/${id}/nearby`, { params }).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(list => (list || []).map(n => ({
        ...n,
        nearbySiteId: n.id,
        nearbySiteName: n.name,
        startYearFormatted: n.chronologicalSpan ? n.chronologicalSpan.split(' – ')[0] : '',
        endYearFormatted: n.chronologicalSpan ? n.chronologicalSpan.split(' – ')[1] : ''
      }))),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Gets contemporaneous co-existing sites.
   */
  getContemporaneousSites(id: number): Observable<ContemporaneousSiteResult[]> {
    return this.http.get<ContemporaneousSiteResult[]>(`${this.baseUrl}/sites/${id}/contemporaneous`).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(list => (list || []).map(c => ({
        ...c,
        startYearFormatted: c.chronologicalSpan ? c.chronologicalSpan.split(' – ')[0] : '',
        endYearFormatted: c.chronologicalSpan ? c.chronologicalSpan.split(' – ')[1] : ''
      }))),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Side-by-side comparison of 2 sites with distance and temporal overlap calculations.
   */
  compareSites(site1Id: number, site2Id: number): Observable<SiteComparisonResult> {
    const params = new HttpParams()
      .set('site1Id', site1Id.toString())
      .set('site2Id', site2Id.toString());

    return this.http.get<any>(`${this.baseUrl}/sites/compare`, { params }).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(res => ({
        site1: this.normalizeSiteDetail(res.site1),
        site2: this.normalizeSiteDetail(res.site2),
        distanceKm: res.distanceKm,
        chronologicalOverlapYears: res.chronologicalOverlapYears,
        isContemporaneous: res.isContemporaneous,
        sharedCivilizations: res.sharedCivilizations || [],
        sharedPeriods: res.sharedPeriods || [],
        geodesicDistanceKm: res.distanceKm,
        temporalOverlapYears: res.chronologicalOverlapYears,
        hasTemporalOverlap: res.isContemporaneous,
        overlapSpanText: res.isContemporaneous
          ? `${res.chronologicalOverlapYears} Years Synchronous Horizon`
          : 'Non-overlapping Chronological Horizons',
        directRelationships: [
          ...(res.site1?.outgoingRelationships || []).filter((r: any) => r.relatedSiteId === res.site2?.id),
          ...(res.site2?.outgoingRelationships || []).filter((r: any) => r.relatedSiteId === res.site1?.id)
        ]
      })),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Lists all civilizations.
   */
  getCivilizations(): Observable<Civilization[]> {
    return this.http.get<Civilization[]>(`${this.baseUrl}/civilizations`).pipe(
      tap(() => this.isApiOnline.set(true)),
      map(list => (list || []).map(c => ({
        ...c,
        startYearFormatted: c.chronologicalSpan ? c.chronologicalSpan.split(' – ')[0] : `${c.startYear}`,
        endYearFormatted: c.chronologicalSpan ? c.chronologicalSpan.split(' – ')[1] : `${c.endYear}`
      }))),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Lists all historical periods.
   */
  getPeriods(): Observable<HistoricalPeriod[]> {
    return this.http.get<HistoricalPeriod[]>(`${this.baseUrl}/periods`).pipe(
      tap(() => this.isApiOnline.set(true)),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Lists diagnostic artefacts.
   */
  getArtefacts(siteId?: number): Observable<Artefact[]> {
    let params = new HttpParams();
    if (siteId) params = params.set('siteId', siteId.toString());
    return this.http.get<Artefact[]>(`${this.baseUrl}/artefacts`, { params }).pipe(
      tap(() => this.isApiOnline.set(true)),
      catchError(err => {
        this.isApiOnline.set(false);
        return throwError(() => err);
      })
    );
  }
}
