import { ExchangeRateEntry, ExchangeRateTable } from '../models/exchangeRate';
import { parseCsvLines, parseDate } from './csvReader';

export function parseExchangeRates(csv: string): ExchangeRateTable {
  const rows = parseCsvLines(csv);
  if (rows.length < 2) return { entries: [], dates: [] };

  // First row: JPY, date1, date2, ...
  const headerRow = rows[0];
  const dates = headerRow.slice(1).filter(s => s.length > 0).map(s => parseDate(s));
  const dateKeys = dates.map(d => `${d.getFullYear()}/${d.getMonth() + 1}/1`);

  const entries: ExchangeRateEntry[] = [];
  for (let i = 1; i < rows.length; i++) {
    const cols = rows[i];
    const currency = cols[0].trim();
    if (!currency) continue;

    const rates = new Map<string, number>();
    for (let j = 0; j < dateKeys.length && j + 1 < cols.length; j++) {
      const rate = parseFloat(cols[j + 1]);
      if (!isNaN(rate)) {
        rates.set(dateKeys[j], rate);
      }
    }
    entries.push({ currency, rates });
  }

  return { entries, dates };
}
