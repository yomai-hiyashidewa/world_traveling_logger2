import { CountryType, CurrencyType, OtherType } from './types';

export interface OtherExpense {
  id: string;
  otherType: OtherType;
  context: string | null;
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
