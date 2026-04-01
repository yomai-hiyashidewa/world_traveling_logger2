import { Accommodation } from '../models/accommodation';
import { Transportation } from '../models/transportation';
import { Sightseeing } from '../models/sightseeing';
import { OtherExpense } from '../models/otherExpense';
import {
  CostSummary, TypeSummary, TransportationTypeSummary, MovingSummary,
  formatDistance, formatTime,
} from '../models/summary';
import { MajorCurrencyType } from '../models/types';

function getPriceByMajorCurrency<T extends { jpyPrice: number; eurPrice: number; usdPrice: number }>(
  item: T, currency: MajorCurrencyType,
): number {
  switch (currency) {
    case 'JPY': return item.jpyPrice;
    case 'EUR': return item.eurPrice;
    case 'USD': return item.usdPrice;
  }
}

export function calcCostSummary(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  others: OtherExpense[],
  currency: MajorCurrencyType,
): CostSummary {
  const sum = <T extends { jpyPrice: number; eurPrice: number; usdPrice: number }>(items: T[]) =>
    items.reduce((acc, item) => acc + getPriceByMajorCurrency(item, currency), 0);

  const accommodation = sum(accommodations);
  const transportation = sum(transportations);
  const sightseeing = sum(sightseeings);
  const other = sum(others);

  return {
    accommodation,
    transportation,
    sightseeing,
    other,
    total: accommodation + transportation + sightseeing + other,
    currency,
  };
}

export function calcTypeSummary<T extends { jpyPrice: number; eurPrice: number; usdPrice: number }>(
  items: T[],
  getType: (item: T) => string,
  currency: MajorCurrencyType,
): TypeSummary[] {
  const groups = new Map<string, T[]>();
  for (const item of items) {
    const type = getType(item);
    if (!groups.has(type)) groups.set(type, []);
    groups.get(type)!.push(item);
  }

  return Array.from(groups.entries()).map(([type, groupItems]) => {
    const costs = groupItems.map(i => getPriceByMajorCurrency(i, currency));
    const totalCost = costs.reduce((a, b) => a + b, 0);
    return {
      type,
      count: groupItems.length,
      totalCost,
      maxCost: Math.max(...costs),
      minCost: Math.min(...costs),
      averageCost: totalCost / groupItems.length,
    };
  }).sort((a, b) => b.totalCost - a.totalCost);
}

export function calcTransportationTypeSummary(
  items: Transportation[],
  currency: MajorCurrencyType,
): TransportationTypeSummary[] {
  const groups = new Map<string, Transportation[]>();
  for (const item of items) {
    const type = item.transportationType;
    if (!groups.has(type)) groups.set(type, []);
    groups.get(type)!.push(item);
  }

  return Array.from(groups.entries()).map(([type, groupItems]) => {
    const costs = groupItems.map(i => getPriceByMajorCurrency(i, currency));
    const distances = groupItems.map(i => i.distance);
    const times = groupItems.map(i => i.time);
    const totalCost = costs.reduce((a, b) => a + b, 0);
    const totalDistance = distances.reduce((a, b) => a + b, 0);
    const totalTime = times.reduce((a, b) => a + b, 0);

    return {
      type,
      count: groupItems.length,
      totalCost,
      maxCost: Math.max(...costs),
      minCost: Math.min(...costs),
      averageCost: totalCost / groupItems.length,
      totalDistance,
      maxDistance: Math.max(...distances),
      minDistance: Math.min(...distances),
      averageDistance: totalDistance / groupItems.length,
      totalTime,
      maxTime: Math.max(...times),
      minTime: Math.min(...times),
      averageTime: totalTime / groupItems.length,
    };
  }).sort((a, b) => b.totalDistance - a.totalDistance);
}

export function calcMovingSummary(items: Transportation[]): MovingSummary {
  const totalDistance = items.reduce((a, t) => a + t.distance, 0);
  const totalTime = items.reduce((a, t) => a + t.time, 0);
  return {
    totalDistance,
    totalTime,
    formattedDistance: formatDistance(totalDistance),
    formattedTime: formatTime(totalTime),
  };
}
