import { MajorCurrencyType } from './types';

export interface CostSummary {
  accommodation: number;
  transportation: number;
  sightseeing: number;
  other: number;
  total: number;
  currency: MajorCurrencyType;
}

export interface TypeSummary {
  type: string;
  count: number;
  totalCost: number;
  maxCost: number;
  minCost: number;
  averageCost: number;
}

export interface TransportationTypeSummary extends TypeSummary {
  totalDistance: number;
  maxDistance: number;
  minDistance: number;
  averageDistance: number;
  totalTime: number;
  maxTime: number;
  minTime: number;
  averageTime: number;
}

export interface MovingSummary {
  totalDistance: number;
  totalTime: number;
  formattedDistance: string;
  formattedTime: string;
}

export function formatDistance(km: number): string {
  return `${Math.round(km).toLocaleString()} km`;
}

export function formatTime(minutes: number): string {
  const days = Math.floor(minutes / (24 * 60));
  const hours = Math.floor((minutes % (24 * 60)) / 60);
  const mins = Math.round(minutes % 60);
  const parts: string[] = [];
  if (days > 0) parts.push(`${days}日`);
  parts.push(`${hours}時間`);
  parts.push(`${mins}分`);
  return parts.join(' ');
}

export function formatCurrency(amount: number, currency: MajorCurrencyType): string {
  switch (currency) {
    case 'JPY': return `¥${Math.round(amount).toLocaleString()}`;
    case 'EUR': return `€${amount.toFixed(2)}`;
    case 'USD': return `$${amount.toFixed(2)}`;
  }
}
