import { Transportation, isCrossBorder, isNoEntry } from '../models/transportation';
import { CountryType } from '../models/types';

// --- Types ---

export interface BorderCrossing {
  date: Date;
  fromCountry: CountryType;
  toCountry: CountryType;
  transportationType: string;
  isNoEntry: boolean;
}

export interface ArrivalRecord {
  fromCountry: CountryType;
  date: Date;
  startDate: Date;
  region: string | null;
  transportationType: string;
  distance: number;
  time: number;
}

export interface DepartureRecord {
  toCountry: CountryType;
  date: Date;
  endDate: Date;
  region: string | null;
  transportationType: string;
  distance: number;
  time: number;
}

export interface RouteDetail {
  region: string | null;
  type: string;
  distance: string;
  time: string;
  isOvernight: boolean;
}

export type RouteRecord = Transportation;

export interface StayPeriod {
  startDate: Date;
  endDate: Date;
}

// --- World mode: border crossings ---

export function extractBorderCrossings(transportations: Transportation[]): BorderCrossing[] {
  return transportations
    .filter(t => isCrossBorder(t))
    .map(t => ({
      date: t.endDate,
      fromCountry: t.startCountry,
      toCountry: t.endCountry,
      transportationType: t.transportationType,
      isNoEntry: isNoEntry(t),
    }))
    .sort((a, b) => a.date.getTime() - b.date.getTime());
}

// --- Country mode: domestic routes ---

export function getRoute(transportations: Transportation[], country: CountryType): Transportation[] {
  return transportations
    .filter(t => t.startCountry === country || t.endCountry === country)
    .sort((a, b) => a.startDate.getTime() - b.startDate.getTime());
}

// --- Country mode: arrivals & departures ---

export function getArrivals(transportations: Transportation[], country: CountryType): ArrivalRecord[] {
  return transportations
    .filter(t =>
      t.endCountry === country &&
      t.startCountry !== country &&
      !isNoEntry(t)
    )
    .map(t => ({
      fromCountry: t.startCountry,
      date: t.endDate,
      startDate: t.startDate,
      region: t.endRegion,
      transportationType: t.transportationType,
      distance: t.distance,
      time: t.time,
    }))
    .sort((a, b) => a.date.getTime() - b.date.getTime());
}

export function getDepartures(transportations: Transportation[], country: CountryType): DepartureRecord[] {
  return transportations
    .filter(t =>
      t.startCountry === country &&
      t.endCountry !== country
    )
    .map(t => ({
      toCountry: t.startCountry === country ? t.endCountry : t.startCountry,
      date: t.startDate,
      endDate: t.endDate,
      region: t.startRegion,
      transportationType: t.transportationType,
      distance: t.distance,
      time: t.time,
    }))
    .sort((a, b) => a.date.getTime() - b.date.getTime());
}

// --- Country mode: stay period ---

export function getCountryStayPeriod(routes: Transportation[]): StayPeriod | null {
  if (routes.length === 0) return null;
  return {
    startDate: routes[0].endDate,
    endDate: routes[routes.length - 1].startDate,
  };
}

// --- Route detail helpers (matches C# RouteCountryViewModel) ---

export function getArrivalDetail(arrival: ArrivalRecord): RouteDetail {
  return {
    region: arrival.region,
    type: arrival.transportationType,
    distance: formatRouteDistance(arrival.distance),
    time: formatRouteTime(arrival.time),
    isOvernight: arrival.startDate.getTime() !== arrival.date.getTime(),
  };
}

export function getDepartureDetail(departure: DepartureRecord): RouteDetail {
  return {
    region: departure.region,
    type: departure.transportationType,
    distance: formatRouteDistance(departure.distance),
    time: formatRouteTime(departure.time),
    isOvernight: departure.date.getTime() !== departure.endDate.getTime(),
  };
}

export function isOvernight(t: Transportation): boolean {
  return t.startDate.getTime() !== t.endDate.getTime();
}

// --- Route display formatting (matches C# MovingModel) ---

export function formatRouteDistance(km: number): string {
  return `${Math.round(km).toLocaleString()}km`;
}

export function formatRouteTime(minutes: number): string {
  const days = Math.floor(minutes / (24 * 60));
  const hours = Math.floor((minutes % (24 * 60)) / 60);
  const min = minutes % 60;
  if (days > 0) return `${days}d ${hours}h ${min}m`;
  if (hours > 0) return `${hours}h ${min}m`;
  return `${min}min`;
}

// --- Utilities ---

export function countCountries(transportations: Transportation[]): number {
  const set = new Set<CountryType>();
  transportations.forEach(t => {
    set.add(t.startCountry);
    set.add(t.endCountry);
  });
  set.delete('UNK');
  return set.size;
}
