import { Transportation } from '../models/transportation';
import { parseTransportationType, parsePlaceType, reclassifyTransportationType } from '../models/types';
import { parseCsvLines, parseDate, genId, cleanValue } from './csvReader';

export function parseTransportations(csv: string): Transportation[] {
  const rows = parseCsvLines(csv);
  // Skip 2 header rows
  return rows.slice(2).map(cols => {
    const type = parseTransportationType(cols[0]);
    const distance = parseFloat(cols[9]) || 0;
    const reclassified = reclassifyTransportationType(type, distance);

    return {
      id: genId(),
      transportationType: reclassified,
      startDate: parseDate(cols[1]),
      startCountry: cols[2].trim(),
      startRegion: cleanValue(cols[3]),
      startPlace: parsePlaceType(cols[4]),
      endDate: parseDate(cols[5]),
      endCountry: cols[6].trim(),
      endRegion: cleanValue(cols[7]),
      endPlace: parsePlaceType(cols[8]),
      distance,
      time: parseFloat(cols[10]) || 0,
      price: parseFloat(cols[11]) || 0,
      currency: cols[12].trim(),
      memo: cleanValue(cols[13] ?? ''),
      jpyPrice: 0,
      eurPrice: 0,
      usdPrice: 0,
    };
  });
}
