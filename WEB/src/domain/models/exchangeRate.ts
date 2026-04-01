import { CurrencyType } from './types';

export interface ExchangeRateEntry {
  currency: CurrencyType;
  rates: Map<string, number>; // key: "YYYY/M" -> rate to JPY
}

export interface ExchangeRateTable {
  entries: ExchangeRateEntry[];
  dates: Date[];
}
