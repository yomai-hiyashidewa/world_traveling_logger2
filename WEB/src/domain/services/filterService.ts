import { Accommodation } from '../models/accommodation';
import { Transportation } from '../models/transportation';
import { Sightseeing } from '../models/sightseeing';
import { OtherExpense } from '../models/otherExpense';
import { CountryType } from '../models/types';

export interface FilterCriteria {
  country: CountryType | null;
  region: string | null;
  startDate: Date | null;
  endDate: Date | null;
  excludeAirplane: boolean;
  excludeInsurance: boolean;
  excludeCrossBorder: boolean;
  excludeJapan: boolean;
}

export function filterAccommodations(items: Accommodation[], criteria: FilterCriteria): Accommodation[] {
  return items.filter(item => {
    if (!criteria.country && criteria.excludeJapan && item.country === 'JPN') return false;
    if (criteria.country && item.country !== criteria.country) return false;
    if (criteria.region && item.region !== criteria.region) return false;
    if (criteria.startDate && item.date < criteria.startDate) return false;
    if (criteria.endDate && item.date > criteria.endDate) return false;
    return true;
  });
}

export function filterTransportations(items: Transportation[], criteria: FilterCriteria): Transportation[] {
  return items.filter(item => {
    if (criteria.excludeAirplane && item.transportationType === 'AirPlane') return false;
    if (!criteria.country && criteria.excludeJapan && (item.startCountry === 'JPN' || item.endCountry === 'JPN')) return false;
    // Country mode: exclude cross-border transportation when toggle is on
    if (criteria.country && criteria.excludeCrossBorder && item.startCountry !== item.endCountry) return false;
    if (criteria.country) {
      if (item.startCountry !== criteria.country && item.endCountry !== criteria.country) return false;
    }
    if (criteria.region) {
      if (item.startRegion !== criteria.region && item.endRegion !== criteria.region) return false;
    }
    if (criteria.startDate && item.startDate < criteria.startDate) return false;
    if (criteria.endDate && item.endDate > criteria.endDate) return false;
    return true;
  });
}

export function filterSightseeings(items: Sightseeing[], criteria: FilterCriteria): Sightseeing[] {
  return items.filter(item => {
    if (!criteria.country && criteria.excludeJapan && item.country === 'JPN') return false;
    if (criteria.country && item.country !== criteria.country) return false;
    if (criteria.region && item.region !== criteria.region) return false;
    if (criteria.startDate && item.date < criteria.startDate) return false;
    if (criteria.endDate && item.date > criteria.endDate) return false;
    return true;
  });
}

export function filterOtherExpenses(items: OtherExpense[], criteria: FilterCriteria): OtherExpense[] {
  return items.filter(item => {
    if (criteria.excludeInsurance && item.otherType === 'Insurance') return false;
    if (!criteria.country && criteria.excludeJapan && item.country === 'JPN') return false;
    if (criteria.country && item.country !== criteria.country) return false;
    if (criteria.region && item.region !== criteria.region) return false;
    if (criteria.startDate && item.date < criteria.startDate) return false;
    if (criteria.endDate && item.date > criteria.endDate) return false;
    return true;
  });
}

export function extractCountries(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  others: OtherExpense[],
): CountryType[] {
  const set = new Set<CountryType>();
  accommodations.forEach(a => set.add(a.country));
  transportations.forEach(t => { set.add(t.startCountry); set.add(t.endCountry); });
  sightseeings.forEach(s => set.add(s.country));
  others.forEach(o => set.add(o.country));
  return Array.from(set).sort();
}

export function extractRegions(
  accommodations: Accommodation[],
  sightseeings: Sightseeing[],
  others: OtherExpense[],
  country: CountryType | null,
): string[] {
  const set = new Set<string>();
  const add = (r: string | null, c: CountryType) => {
    if (r && (!country || c === country)) set.add(r);
  };
  accommodations.forEach(a => add(a.region, a.country));
  sightseeings.forEach(s => add(s.region, s.country));
  others.forEach(o => add(o.region, o.country));
  return Array.from(set).sort();
}
