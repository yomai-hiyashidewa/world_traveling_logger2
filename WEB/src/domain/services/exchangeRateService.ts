import { ExchangeRateTable } from '../models/exchangeRate';
import { CurrencyType, MajorCurrencyType } from '../models/types';
import { dateToMonthKey } from '../parsers/csvReader';

export function getRate(table: ExchangeRateTable, currency: CurrencyType, date: Date): number {
  if (currency === 'JPY') return 1;
  const entry = table.entries.find(e => e.currency === currency);
  if (!entry) return 1;

  const key = dateToMonthKey(date);
  if (entry.rates.has(key)) return entry.rates.get(key)!;

  // Find nearest date
  const allKeys = Array.from(entry.rates.keys());
  if (allKeys.length === 0) return 1;
  // Use the last available rate as fallback
  return entry.rates.get(allKeys[allKeys.length - 1]) ?? 1;
}

export interface ConvertedPrices {
  jpyPrice: number;
  eurPrice: number;
  usdPrice: number;
}

export function convertPrice(
  table: ExchangeRateTable,
  price: number,
  currency: CurrencyType,
  date: Date,
): ConvertedPrices {
  // First convert to JPY
  const rateToJpy = getRate(table, currency, date);
  const jpyPrice = price * rateToJpy;

  // Then convert JPY to EUR/USD
  const eurRate = getRate(table, 'EUR', date);
  const usdRate = getRate(table, 'USD', date);

  return {
    jpyPrice,
    eurPrice: eurRate > 0 ? jpyPrice / eurRate : 0,
    usdPrice: usdRate > 0 ? jpyPrice / usdRate : 0,
  };
}

export function applyExchangeRates<T extends { price: number; currency: CurrencyType; jpyPrice: number; eurPrice: number; usdPrice: number }>(
  items: T[],
  table: ExchangeRateTable,
  getDate: (item: T) => Date,
): T[] {
  return items.map(item => {
    const prices = convertPrice(table, item.price, item.currency, getDate(item));
    return { ...item, ...prices };
  });
}
