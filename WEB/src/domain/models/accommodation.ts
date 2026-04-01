import { AccommodationType, CountryType, CurrencyType } from './types';

export interface Accommodation {
  id: string;
  date: Date;
  country: CountryType;
  region: string | null;
  accommodationType: AccommodationType;
  price: number;
  currency: CurrencyType;
  memo: string | null;
  jpyPrice: number;
  eurPrice: number;
  usdPrice: number;
}
