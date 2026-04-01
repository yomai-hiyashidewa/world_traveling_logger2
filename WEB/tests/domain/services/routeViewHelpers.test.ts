import { describe, it, expect } from 'vitest';
import { Transportation } from '../../../src/domain/models/transportation';
import {
  getArrivals,
  getDepartures,
  getArrivalDetail,
  getDepartureDetail,
  isOvernight,
  formatRouteDistance,
  formatRouteTime,
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

const arrivalFromESP = makeTrans({
  id: 'arr1',
  startDate: new Date(2023, 0, 8),
  startCountry: 'ESP', startRegion: 'Barcelona', startPlace: 'Station',
  endDate: new Date(2023, 0, 9),
  endCountry: 'FRA', endRegion: 'Paris', endPlace: 'Station',
  transportationType: 'LongDistanceTrain',
  distance: 1000, time: 360,
});

const arrivalFromITA = makeTrans({
  id: 'arr2',
  startDate: new Date(2023, 0, 20),
  startCountry: 'ITA', startRegion: 'Milan', startPlace: 'Station',
  endDate: new Date(2023, 0, 20),
  endCountry: 'FRA', endRegion: 'Nice', endPlace: 'Station',
  transportationType: 'Bus',
  distance: 300, time: 240,
});

const departureToDE = makeTrans({
  id: 'dep1',
  startDate: new Date(2023, 0, 15),
  startCountry: 'FRA', startRegion: 'Marseille', startPlace: 'Station',
  endDate: new Date(2023, 0, 15),
  endCountry: 'DEU', endRegion: 'Munich', endPlace: 'Station',
  transportationType: 'AirPlane',
  distance: 800, time: 90,
});

const domestic = makeTrans({ id: 'dom', distance: 465, time: 120 });

const allRoutes = [arrivalFromESP, arrivalFromITA, departureToDE, domestic];

describe('getArrivalDetail', () => {
  it('returns region, type, formatted distance and time for an arrival record', () => {
    const arrivals = getArrivals(allRoutes, 'FRA');
    const detail = getArrivalDetail(arrivals[0]);
    expect(detail.region).toBe('Paris');
    expect(detail.type).toBe('LongDistanceTrain');
    expect(detail.distance).toBe('1,000km');
    expect(detail.time).toBe('6h 0m');
    expect(detail.isOvernight).toBe(true);
  });

  it('returns isOvernight false when same date', () => {
    const arrivals = getArrivals(allRoutes, 'FRA');
    const detail = getArrivalDetail(arrivals[1]);
    expect(detail.isOvernight).toBe(false);
  });
});

describe('getDepartureDetail', () => {
  it('returns region, type, formatted distance and time for a departure record', () => {
    const departures = getDepartures(allRoutes, 'FRA');
    const detail = getDepartureDetail(departures[0]);
    expect(detail.region).toBe('Marseille');
    expect(detail.type).toBe('AirPlane');
    expect(detail.distance).toBe('800km');
    expect(detail.time).toBe('1h 30m');
    expect(detail.isOvernight).toBe(false);
  });
});

describe('isOvernight', () => {
  it('returns true when startDate and endDate differ', () => {
    expect(isOvernight(arrivalFromESP)).toBe(true);
  });

  it('returns false when startDate and endDate are the same', () => {
    expect(isOvernight(domestic)).toBe(false);
  });
});

describe('auto-selection: first item detail', () => {
  it('arrivals list is sorted by date so first item is auto-selectable', () => {
    const arrivals = getArrivals(allRoutes, 'FRA');
    expect(arrivals.length).toBeGreaterThanOrEqual(1);
    // First arrival should be the earliest (from ESP, Jan 9)
    const detail = getArrivalDetail(arrivals[0]);
    expect(detail.region).toBe('Paris');
    expect(detail.type).toBe('LongDistanceTrain');
    expect(detail.distance).toBe('1,000km');
    expect(detail.time).toBe('6h 0m');
  });

  it('departures list is sorted by date so first item is auto-selectable', () => {
    const departures = getDepartures(allRoutes, 'FRA');
    expect(departures.length).toBeGreaterThanOrEqual(1);
    const detail = getDepartureDetail(departures[0]);
    expect(detail.region).toBe('Marseille');
    expect(detail.type).toBe('AirPlane');
    expect(detail.distance).toBe('800km');
    expect(detail.time).toBe('1h 30m');
  });
});
