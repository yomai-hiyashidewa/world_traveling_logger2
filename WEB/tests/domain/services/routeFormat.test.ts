import { describe, it, expect } from 'vitest';
import {
  formatRouteDistance,
  formatRouteTime,
} from '../../../src/domain/services/routeService';

describe('formatRouteDistance', () => {
  it('formats distance with thousands separator and "km" suffix', () => {
    expect(formatRouteDistance(1234)).toBe('1,234km');
  });

  it('formats small distance without separator', () => {
    expect(formatRouteDistance(50)).toBe('50km');
  });

  it('formats zero', () => {
    expect(formatRouteDistance(0)).toBe('0km');
  });

  it('rounds decimal values', () => {
    expect(formatRouteDistance(1234.6)).toBe('1,235km');
  });
});

describe('formatRouteTime', () => {
  it('formats time with days, hours, and minutes', () => {
    // 1d 2h 30m = 24*60 + 2*60 + 30 = 1590
    expect(formatRouteTime(1590)).toBe('1d 2h 30m');
  });

  it('formats time with hours and minutes when less than a day', () => {
    // 2h 30m = 150
    expect(formatRouteTime(150)).toBe('2h 30m');
  });

  it('formats minutes only when less than an hour', () => {
    expect(formatRouteTime(30)).toBe('30min');
  });

  it('formats zero minutes', () => {
    expect(formatRouteTime(0)).toBe('0min');
  });

  it('formats exactly one day', () => {
    // 24*60 = 1440
    expect(formatRouteTime(1440)).toBe('1d 0h 0m');
  });

  it('formats multi-day duration', () => {
    // 2d 5h 15m = 2*24*60 + 5*60 + 15 = 3195
    expect(formatRouteTime(3195)).toBe('2d 5h 15m');
  });
});
