import { CountryType, CurrencyType, SightseeingType } from './types';

export interface Sightseeing {
  id: string;
  context: string;
  sightseeingType: SightseeingType;
  date: Date;
  country: CountryType;
  region: string | null;
  price: number;
  currency: CurrencyType;
  memo: string | null;
  jpyPrice: number;
  eurPrice: number;
  usdPrice: number;
}
