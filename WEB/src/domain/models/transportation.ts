import { CountryType, CurrencyType, PlaceType, TransportationType } from './types';

export interface Transportation {
  id: string;
  transportationType: TransportationType;
  startDate: Date;
  startCountry: CountryType;
  startRegion: string | null;
  startPlace: PlaceType;
  endDate: Date;
  endCountry: CountryType;
  endRegion: string | null;
  endPlace: PlaceType;
  distance: number;
  time: number;
  price: number;
  currency: CurrencyType;
  memo: string | null;
  jpyPrice: number;
  eurPrice: number;
  usdPrice: number;
}

export function isCrossBorder(t: Transportation): boolean {
  return t.startCountry !== t.endCountry;
}

export function isDeparture(t: Transportation, country: CountryType): boolean {
  return t.startCountry === country && t.endCountry !== country;
}

export function isArrival(t: Transportation, country: CountryType): boolean {
  return t.endCountry === country && t.startCountry !== country;
}

export function isNoEntry(t: Transportation): boolean {
  return t.memo?.includes('NO_ENTRY') ?? false;
}
