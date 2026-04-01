import { useState, useMemo } from 'react';
import { Accommodation } from '../../domain/models/accommodation';
import { Transportation } from '../../domain/models/transportation';
import { Sightseeing } from '../../domain/models/sightseeing';
import { OtherExpense } from '../../domain/models/otherExpense';
import { MajorCurrencyType, CountryType } from '../../domain/models/types';
import {
  FilterCriteria,
  filterAccommodations, filterTransportations,
  filterSightseeings, filterOtherExpenses,
  extractCountries, extractRegions,
} from '../../domain/services/filterService';

export interface FilterState {
  criteria: FilterCriteria;
  displayCurrency: MajorCurrencyType;
  countries: CountryType[];
  regions: string[];
  filteredAccommodations: Accommodation[];
  filteredTransportations: Transportation[];
  filteredSightseeings: Sightseeing[];
  filteredOtherExpenses: OtherExpense[];
  setCountry: (country: CountryType | null) => void;
  setRegion: (region: string | null) => void;
  setStartDate: (date: Date | null) => void;
  setEndDate: (date: Date | null) => void;
  setDisplayCurrency: (currency: MajorCurrencyType) => void;
  toggleExcludeAirplane: () => void;
  toggleExcludeInsurance: () => void;
  toggleExcludeCrossBorder: () => void;
  toggleExcludeJapan: () => void;
  clearFilters: () => void;
}

export function useFilterState(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  otherExpenses: OtherExpense[],
): FilterState {
  const [criteria, setCriteria] = useState<FilterCriteria>({
    country: null,
    region: null,
    startDate: null,
    endDate: null,
    excludeAirplane: false,
    excludeInsurance: false,
    excludeCrossBorder: false,
    excludeJapan: false,
  });
  const [displayCurrency, setDisplayCurrency] = useState<MajorCurrencyType>('JPY');

  const countries = useMemo(
    () => extractCountries(accommodations, transportations, sightseeings, otherExpenses),
    [accommodations, transportations, sightseeings, otherExpenses],
  );

  const regions = useMemo(
    () => extractRegions(accommodations, sightseeings, otherExpenses, criteria.country),
    [accommodations, sightseeings, otherExpenses, criteria.country],
  );

  const filteredAccommodations = useMemo(
    () => filterAccommodations(accommodations, criteria),
    [accommodations, criteria],
  );
  const filteredTransportations = useMemo(
    () => filterTransportations(transportations, criteria),
    [transportations, criteria],
  );
  const filteredSightseeings = useMemo(
    () => filterSightseeings(sightseeings, criteria),
    [sightseeings, criteria],
  );
  const filteredOtherExpenses = useMemo(
    () => filterOtherExpenses(otherExpenses, criteria),
    [otherExpenses, criteria],
  );

  return {
    criteria,
    displayCurrency,
    countries,
    regions,
    filteredAccommodations,
    filteredTransportations,
    filteredSightseeings,
    filteredOtherExpenses,
    setCountry: (country) => setCriteria(c => ({ ...c, country, region: null })),
    setRegion: (region) => setCriteria(c => ({ ...c, region })),
    setStartDate: (startDate) => setCriteria(c => ({ ...c, startDate })),
    setEndDate: (endDate) => setCriteria(c => ({ ...c, endDate })),
    setDisplayCurrency,
    toggleExcludeAirplane: () => setCriteria(c => ({ ...c, excludeAirplane: !c.excludeAirplane })),
    toggleExcludeInsurance: () => setCriteria(c => ({ ...c, excludeInsurance: !c.excludeInsurance })),
    toggleExcludeCrossBorder: () => setCriteria(c => ({ ...c, excludeCrossBorder: !c.excludeCrossBorder })),
    toggleExcludeJapan: () => setCriteria(c => ({ ...c, excludeJapan: !c.excludeJapan })),
    clearFilters: () => setCriteria({ country: null, region: null, startDate: null, endDate: null, excludeAirplane: false, excludeInsurance: false, excludeCrossBorder: false, excludeJapan: false }),
  };
}
