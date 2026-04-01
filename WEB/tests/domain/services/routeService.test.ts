import { describe, it, expect } from 'vitest';
import { Transportation } from '../../../src/domain/models/transportation';
import {
  getRoute,
  getArrivals,
  getDepartures,
  getCountryStayPeriod,
  extractBorderCrossings,
  RouteRecord,
} from '../../../src/domain/services/routeService';

function makeTrans(overrides: Partial<Transportation>): Transportation {
  return {
    id: 'test',
    transportationType: 'Train',
    startDate: new Date(2023, 0, 1),
    startCountry: 'FRA',
    startRegion: 'Paris',
    startPlace: 'Station',
    endDate: new Date(2023, 0, 1),
    endCountry: 'FRA',
    endRegion: 'Lyon',
    endPlace: 'Station',
    distance: 500,
    time: 120,
    price: 50,
    currency: 'EUR',
    memo: null,
    jpyPrice: 7500,
    eurPrice: 50,
    usdPrice: 55,
    ...overrides,
  };
}

const domestic1 = makeTrans({
  id: 'd1',
  startDate: new Date(2023, 0, 10),
  startCountry: 'FRA', startRegion: 'Paris', startPlace: 'Station',
  endDate: new Date(2023, 0, 10),
  endCountry: 'FRA', endRegion: 'Lyon', endPlace: 'Station',
  distance: 465, time: 120,
});

const domestic2 = makeTrans({
  id: 'd2',
  startDate: new Date(2023, 0, 12),
  startCountry: 'FRA', startRegion: 'Lyon', startPlace: 'Station',
  endDate: new Date(2023, 0, 12),
  endCountry: 'FRA', endRegion: 'Marseille', endPlace: 'Station',
  distance: 315, time: 100,
});

const arrivalToFRA = makeTrans({
  id: 'arr1',
  startDate: new Date(2023, 0, 8),
  startCountry: 'ESP', startRegion: 'Barcelona', startPlace: 'Station',
  endDate: new Date(2023, 0, 9),
  endCountry: 'FRA', endRegion: 'Paris', endPlace: 'Station',
  transportationType: 'LongDistanceTrain',
  distance: 1000, time: 360,
});

const departureFromFRA = makeTrans({
  id: 'dep1',
  startDate: new Date(2023, 0, 15),
  startCountry: 'FRA', startRegion: 'Marseille', startPlace: 'Station',
  endDate: new Date(2023, 0, 15),
  endCountry: 'DEU', endRegion: 'Munich', endPlace: 'Station',
  transportationType: 'LongDistanceTrain',
  distance: 800, time: 300,
});

const noEntryTransit = makeTrans({
  id: 'ne1',
  startDate: new Date(2023, 0, 5),
  startCountry: 'ITA', startRegion: 'Milan', startPlace: 'AirPort',
  endDate: new Date(2023, 0, 5),
  endCountry: 'FRA', endRegion: 'Paris', endPlace: 'AirPort',
  transportationType: 'AirPlane',
  memo: 'NO_ENTRY transit only',
  distance: 850, time: 90,
});

const unrelatedRoute = makeTrans({
  id: 'unr1',
  startDate: new Date(2023, 0, 20),
  startCountry: 'DEU', startRegion: 'Berlin', startPlace: 'Station',
  endDate: new Date(2023, 0, 20),
  endCountry: 'DEU', endRegion: 'Hamburg', endPlace: 'Station',
});

const overnightDomestic = makeTrans({
  id: 'on1',
  startDate: new Date(2023, 0, 11),
  startCountry: 'FRA', startRegion: 'Lyon', startPlace: 'Station',
  endDate: new Date(2023, 0, 12),
  endCountry: 'FRA', endRegion: 'Nice', endPlace: 'Station',
  transportationType: 'Bus',
  distance: 470, time: 480,
});

const allRoutes = [domestic1, domestic2, arrivalToFRA, departureFromFRA, noEntryTransit, unrelatedRoute, overnightDomestic];

describe('routeService', () => {
  describe('getRoute', () => {
    it('returns all records where startCountry or endCountry matches target', () => {
      const result = getRoute(allRoutes, 'FRA');
      const ids = result.map(r => r.id);
      expect(ids).toContain('d1');
      expect(ids).toContain('d2');
      expect(ids).toContain('arr1');
      expect(ids).toContain('dep1');
      expect(ids).toContain('ne1');
      expect(ids).toContain('on1');
      expect(ids).not.toContain('unr1');
    });

    it('returns results sorted by startDate', () => {
      const result = getRoute(allRoutes, 'FRA');
      for (let i = 1; i < result.length; i++) {
        expect(result[i].startDate.getTime()).toBeGreaterThanOrEqual(result[i - 1].startDate.getTime());
      }
    });

    it('returns empty array for unknown country', () => {
      expect(getRoute(allRoutes, 'ZZZ')).toEqual([]);
    });
  });

  describe('getArrivals', () => {
    it('returns cross-border arrivals to target country', () => {
      const result = getArrivals(allRoutes, 'FRA');
      expect(result.some(a => a.fromCountry === 'ESP')).toBe(true);
    });

    it('excludes NO_ENTRY records', () => {
      const result = getArrivals(allRoutes, 'FRA');
      expect(result.some(a => a.fromCountry === 'ITA')).toBe(false);
    });

    it('excludes domestic routes (same country)', () => {
      const result = getArrivals(allRoutes, 'FRA');
      expect(result.every(a => a.fromCountry !== 'FRA')).toBe(true);
    });
  });

  describe('getDepartures', () => {
    it('returns cross-border departures from target country', () => {
      const result = getDepartures(allRoutes, 'FRA');
      expect(result.some(d => d.toCountry === 'DEU')).toBe(true);
    });

    it('excludes domestic routes', () => {
      const result = getDepartures(allRoutes, 'FRA');
      expect(result.every(d => d.toCountry !== 'FRA')).toBe(true);
    });
  });

  describe('getCountryStayPeriod', () => {
    it('returns first endDate and last startDate of routes for a country', () => {
      const routes = getRoute(allRoutes, 'FRA');
      const period = getCountryStayPeriod(routes);
      expect(period).not.toBeNull();
      // Earliest endDate among FRA routes is noEntryTransit (Jan 5)
      // Latest startDate among FRA routes is departureFromFRA (Jan 15)
      expect(period!.startDate.getDate()).toBe(5);
      expect(period!.endDate.getDate()).toBe(15);
    });

    it('returns null for empty routes', () => {
      expect(getCountryStayPeriod([])).toBeNull();
    });
  });

  describe('RouteRecord classification', () => {
    it('classifies domestic routes correctly', () => {
      const routes = getRoute(allRoutes, 'FRA');
      const d1 = routes.find(r => r.id === 'd1')!;
      expect(d1.startCountry).toBe('FRA');
      expect(d1.endCountry).toBe('FRA');
    });

    it('overnight route has different start and end dates', () => {
      const routes = getRoute(allRoutes, 'FRA');
      const on = routes.find(r => r.id === 'on1')!;
      expect(on.startDate.getDate()).not.toBe(on.endDate.getDate());
    });
  });

  describe('extractBorderCrossings (existing)', () => {
    it('returns all cross-border movements', () => {
      const result = extractBorderCrossings(allRoutes);
      expect(result.length).toBeGreaterThanOrEqual(3);
    });

    it('marks NO_ENTRY correctly', () => {
      const result = extractBorderCrossings(allRoutes);
      const ne = result.find(bc => bc.fromCountry === 'ITA' && bc.toCountry === 'FRA');
      expect(ne?.isNoEntry).toBe(true);
    });
  });
});
