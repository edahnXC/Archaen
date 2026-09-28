import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  SiteSummary,
  SiteDetail,
  NearbySiteResult,
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
  private readonly baseUrl = 'http://localhost:5032/api';

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
    return this.http.get<PagedResult<SiteSummary>>(`${this.baseUrl}/sites`, { params: httpParams });
  }

  /**
   * Gets full detailed site profile including excavations, stratigraphy, artefacts, and citations.
   */
  getSiteById(id: number): Observable<SiteDetail> {
    return this.http.get<SiteDetail>(`${this.baseUrl}/sites/${id}`);
  }

  /**
   * Gets geographic neighbours within radiusKm.
   */
  getNearbySites(id: number, radiusKm: number = 500): Observable<NearbySiteResult[]> {
    const params = new HttpParams().set('radiusKm', radiusKm.toString());
    return this.http.get<NearbySiteResult[]>(`${this.baseUrl}/sites/${id}/nearby`, { params });
  }

  /**
   * Gets contemporaneous co-existing sites.
   */
  getContemporaneousSites(id: number): Observable<SiteSummary[]> {
    return this.http.get<SiteSummary[]>(`${this.baseUrl}/sites/${id}/contemporaneous`);
  }

  /**
   * Side-by-side comparison of 2 sites with distance and temporal overlap calculations.
   */
  compareSites(site1Id: number, site2Id: number): Observable<SiteComparisonResult> {
    const params = new HttpParams()
      .set('site1Id', site1Id.toString())
      .set('site2Id', site2Id.toString());
    return this.http.get<SiteComparisonResult>(`${this.baseUrl}/sitecomparison`, { params });
  }

  /**
   * Lists all civilizations.
   */
  getCivilizations(): Observable<Civilization[]> {
    return this.http.get<Civilization[]>(`${this.baseUrl}/civilizations`);
  }

  /**
   * Lists all historical periods.
   */
  getPeriods(): Observable<HistoricalPeriod[]> {
    return this.http.get<HistoricalPeriod[]>(`${this.baseUrl}/periods`);
  }

  /**
   * Lists diagnostic artefacts.
   */
  getArtefacts(siteId?: number): Observable<Artefact[]> {
    let params = new HttpParams();
    if (siteId) params = params.set('siteId', siteId.toString());
    return this.http.get<Artefact[]>(`${this.baseUrl}/artefacts`, { params });
  }
}
