import { useState, useEffect } from 'react';
import { Accommodation } from '../../domain/models/accommodation';
import { Transportation } from '../../domain/models/transportation';
import { Sightseeing } from '../../domain/models/sightseeing';
import { OtherExpense } from '../../domain/models/otherExpense';
import { ExchangeRateTable } from '../../domain/models/exchangeRate';
import { parseAccommodations } from '../../domain/parsers/accommodationParser';
import { parseTransportations } from '../../domain/parsers/transportationParser';
import { parseSightseeings } from '../../domain/parsers/sightseeingParser';
import { parseExchangeRates } from '../../domain/parsers/exchangeRateParser';
import { applyExchangeRates } from '../../domain/services/exchangeRateService';

export interface TravelData {
  accommodations: Accommodation[];
  transportations: Transportation[];
  sightseeings: Sightseeing[];
  otherExpenses: OtherExpense[];
  exchangeRates: ExchangeRateTable;
  loading: boolean;
  error: string | null;
}

const BASE = import.meta.env.BASE_URL;

async function fetchCsv(filename: string): Promise<string> {
  const resp = await fetch(`${BASE}data/${filename}`);
  if (!resp.ok) throw new Error(`Failed to fetch ${filename}: ${resp.status}`);
  return resp.text();
}

export function useTravelData(): TravelData {
  const [data, setData] = useState<TravelData>({
    accommodations: [],
    transportations: [],
    sightseeings: [],
    otherExpenses: [],
    exchangeRates: { entries: [], dates: [] },
    loading: true,
    error: null,
  });

  useEffect(() => {
    (async () => {
      try {
        const [accCsv, transCsv, sightCsv, exchCsv] = await Promise.all([
          fetchCsv('accommodations.csv'),
          fetchCsv('transportations.csv'),
          fetchCsv('sightseeing.csv'),
          fetchCsv('exchange_rates.csv'),
        ]);

        const exchangeRates = parseExchangeRates(exchCsv);
        const rawAccommodations = parseAccommodations(accCsv);
        const rawTransportations = parseTransportations(transCsv);
        const { sightseeings: rawSightseeings, otherExpenses: rawOtherExpenses } = parseSightseeings(sightCsv);

        const accommodations = applyExchangeRates(rawAccommodations, exchangeRates, a => a.date);
        const transportations = applyExchangeRates(rawTransportations, exchangeRates, t => t.startDate);
        const sightseeings = applyExchangeRates(rawSightseeings, exchangeRates, s => s.date);
        const otherExpenses = applyExchangeRates(rawOtherExpenses, exchangeRates, o => o.date);

        setData({
          accommodations,
          transportations,
          sightseeings,
          otherExpenses,
          exchangeRates,
          loading: false,
          error: null,
        });
      } catch (e) {
        setData(prev => ({ ...prev, loading: false, error: String(e) }));
      }
    })();
  }, []);

  return data;
}
