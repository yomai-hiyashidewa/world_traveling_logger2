import { useMemo } from 'react';
import { Accommodation } from '../../domain/models/accommodation';
import { Transportation } from '../../domain/models/transportation';
import { Sightseeing } from '../../domain/models/sightseeing';
import { OtherExpense } from '../../domain/models/otherExpense';
import { CostSummary, TransportationTypeSummary, TypeSummary, MovingSummary } from '../../domain/models/summary';
import { MajorCurrencyType, CountryType } from '../../domain/models/types';
import {
  calcCostSummary, calcTypeSummary,
  calcTransportationTypeSummary, calcMovingSummary,
} from '../../domain/services/statsService';
import {
  extractBorderCrossings, BorderCrossing, countCountries,
  getRoute, getArrivals, getDepartures, getCountryStayPeriod,
  ArrivalRecord, DepartureRecord, StayPeriod,
} from '../../domain/services/routeService';

export interface Stats {
  costSummary: CostSummary;
  accommodationTypeSummary: TypeSummary[];
  transportationTypeSummary: TransportationTypeSummary[];
  sightseeingTypeSummary: TypeSummary[];
  otherTypeSummary: TypeSummary[];
  movingSummary: MovingSummary;
  borderCrossings: BorderCrossing[];
  countryCount: number;
  countryRoutes: Transportation[];
  arrivals: ArrivalRecord[];
  departures: DepartureRecord[];
  stayPeriod: StayPeriod | null;
}

export function useStats(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  otherExpenses: OtherExpense[],
  currency: MajorCurrencyType,
  selectedCountry: CountryType | null,
): Stats {
  return useMemo(() => {
    const countryRoutes = selectedCountry ? getRoute(transportations, selectedCountry) : [];
    return {
      costSummary: calcCostSummary(accommodations, transportations, sightseeings, otherExpenses, currency),
      accommodationTypeSummary: calcTypeSummary(accommodations, a => a.accommodationType, currency),
      transportationTypeSummary: calcTransportationTypeSummary(transportations, currency),
      sightseeingTypeSummary: calcTypeSummary(sightseeings, s => s.sightseeingType, currency),
      otherTypeSummary: calcTypeSummary(otherExpenses, o => o.otherType, currency),
      movingSummary: calcMovingSummary(transportations),
      borderCrossings: extractBorderCrossings(transportations),
      countryCount: countCountries(transportations),
      countryRoutes,
      arrivals: selectedCountry ? getArrivals(transportations, selectedCountry) : [],
      departures: selectedCountry ? getDepartures(transportations, selectedCountry) : [],
      stayPeriod: getCountryStayPeriod(countryRoutes),
    };
  }, [accommodations, transportations, sightseeings, otherExpenses, currency, selectedCountry]);
}
