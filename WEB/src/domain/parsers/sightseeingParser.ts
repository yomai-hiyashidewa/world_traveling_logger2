import { Sightseeing } from '../models/sightseeing';
import { OtherExpense } from '../models/otherExpense';
import {
  parseSightseeingCsvType, isOtherType, classifySightseeingByKeyword,
  SightseeingType, OtherType,
} from '../models/types';
import { parseCsvLines, parseDate, genId, cleanValue } from './csvReader';

function capitalizeFirst(s: string): string {
  return s.charAt(0).toUpperCase() + s.slice(1);
}

export interface SightseeingParseResult {
  sightseeings: Sightseeing[];
  otherExpenses: OtherExpense[];
}

export function parseSightseeings(csv: string): SightseeingParseResult {
  const rows = parseCsvLines(csv);
  const sightseeings: Sightseeing[] = [];
  const otherExpenses: OtherExpense[] = [];

  // Skip header row
  for (const cols of rows.slice(1)) {
    // context column may have trailing tab from CSV
    const context = capitalizeFirst((cols[0] ?? '').replace(/\t/g, '').trim());
    const rawType = parseSightseeingCsvType(cols[1]);
    const classifiedType = classifySightseeingByKeyword(context, rawType);
    const date = parseDate(cols[2]);
    const country = cols[3].trim();
    const region = cleanValue(cols[4]);
    const price = parseFloat(cols[5]) || 0;
    const currency = cols[6].trim();
    const memo = cleanValue(cols[7] ?? '');

    if (isOtherType(classifiedType)) {
      otherExpenses.push({
        id: genId(),
        otherType: classifiedType as OtherType,
        context,
        date,
        country,
        region,
        price,
        currency,
        memo,
        jpyPrice: 0,
        eurPrice: 0,
        usdPrice: 0,
      });
    } else {
      sightseeings.push({
        id: genId(),
        context,
        sightseeingType: classifiedType as SightseeingType,
        date,
        country,
        region,
        price,
        currency,
        memo,
        jpyPrice: 0,
        eurPrice: 0,
        usdPrice: 0,
      });
    }
  }

  return { sightseeings, otherExpenses };
}
