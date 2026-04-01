import { Accommodation } from '../models/accommodation';
import { parseAccommodationType } from '../models/types';
import { parseCsvLines, parseDate, genId, cleanValue } from './csvReader';

export function parseAccommodations(csv: string): Accommodation[] {
  const rows = parseCsvLines(csv);
  // Skip header row
  return rows.slice(1).map(cols => {
    const [dateStr, country, region, accType, priceStr, currency, memo] = cols;
    return {
      id: genId(),
      date: parseDate(dateStr),
      country: country.trim(),
      region: cleanValue(region),
      accommodationType: parseAccommodationType(accType),
      price: parseFloat(priceStr) || 0,
      currency: currency.trim(),
      memo: cleanValue(memo ?? ''),
      jpyPrice: 0,
      eurPrice: 0,
      usdPrice: 0,
    };
  });
}
