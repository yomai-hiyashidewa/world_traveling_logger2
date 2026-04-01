import { describe, it, expect } from 'vitest';
import { Accommodation } from '../../../src/domain/models/accommodation';
import { Transportation } from '../../../src/domain/models/transportation';
import { Sightseeing } from '../../../src/domain/models/sightseeing';
import { OtherExpense } from '../../../src/domain/models/otherExpense';
import {
  FilterCriteria,
  filterAccommodations,
  filterTransportations,
  filterSightseeings,
  filterOtherExpenses,
} from '../../../src/domain/services/filterService';

// --- helpers ---

function makeAccommodation(overrides: Partial<Accommodation>): Accommodation {
  return {
    id: 'a1',
    date: new Date(2023, 2, 15),
    country: 'FRA',
    region: 'Paris',
    accommodationType: 'Hotel',
    price: 100,
    currency: 'EUR',
    memo: null,
    jpyPrice: 15000,
    eurPrice: 100,
    usdPrice: 110,
    ...overrides,
  };
}

function makeTransportation(overrides: Partial<Transportation>): Transportation {
  return {
    id: 't1',
    transportationType: 'Train',
    startDate: new Date(2023, 2, 15),
    startCountry: 'FRA',
    startRegion: 'Paris',
    startPlace: 'Station',
    endDate: new Date(2023, 2, 15),
    endCountry: 'FRA',
    endRegion: 'Lyon',
    endPlace: 'Station',
    distance: 500,
    time: 120,
    price: 50,
    currency: 'EUR',
    memo: null,
    jpyPrice: 7500,
    eurPrice: 50,
    usdPrice: 55,
    ...overrides,
  };
}

function makeSightseeing(overrides: Partial<Sightseeing>): Sightseeing {
  return {
    id: 's1',
    context: 'Museum',
    sightseeingType: 'Museum',
    date: new Date(2023, 2, 15),
    country: 'FRA',
    region: 'Paris',
    price: 20,
    currency: 'EUR',
    memo: null,
    jpyPrice: 3000,
    eurPrice: 20,
    usdPrice: 22,
    ...overrides,
  };
}

function makeOtherExpense(overrides: Partial<OtherExpense>): OtherExpense {
  return {
    id: 'o1',
    otherType: 'Shopping',
    context: 'Souvenir',
    date: new Date(2023, 2, 15),
    country: 'FRA',
    region: 'Paris',
    price: 30,
    currency: 'EUR',
    memo: null,
    jpyPrice: 4500,
    eurPrice: 30,
    usdPrice: 33,
    ...overrides,
  };
}

function baseCriteria(overrides: Partial<FilterCriteria> = {}): FilterCriteria {
  return {
    country: null,
    region: null,
    startDate: null,
    endDate: null,
    excludeAirplane: false,
    excludeInsurance: false,
    excludeCrossBorder: false,
    excludeJapan: false,
    ...overrides,
  };
}

// --- test data ---

const jpnAccommodation = makeAccommodation({ id: 'a-jpn', country: 'JPN', region: 'Tokyo' });
const fraAccommodation = makeAccommodation({ id: 'a-fra', country: 'FRA', region: 'Paris' });
const deuAccommodation = makeAccommodation({ id: 'a-deu', country: 'DEU', region: 'Berlin' });

const jpnTransportation = makeTransportation({ id: 't-jpn', startCountry: 'JPN', endCountry: 'JPN' });
const fraTransportation = makeTransportation({ id: 't-fra', startCountry: 'FRA', endCountry: 'FRA' });
const jpnToFraTransportation = makeTransportation({
  id: 't-jpn-fra',
  transportationType: 'AirPlane',
  startCountry: 'JPN',
  endCountry: 'FRA',
});

const jpnSightseeing = makeSightseeing({ id: 's-jpn', country: 'JPN', region: 'Tokyo' });
const fraSightseeing = makeSightseeing({ id: 's-fra', country: 'FRA', region: 'Paris' });

const jpnOther = makeOtherExpense({ id: 'o-jpn', country: 'JPN', region: 'Tokyo' });
const fraOther = makeOtherExpense({ id: 'o-fra', country: 'FRA', region: 'Paris' });

// --- tests ---

describe('excludeJapan filter', () => {
  describe('filterAccommodations', () => {
    const items = [jpnAccommodation, fraAccommodation, deuAccommodation];

    it('excludeJapan=false: JPN レコードを含む', () => {
      const result = filterAccommodations(items, baseCriteria({ excludeJapan: false }));
      expect(result).toHaveLength(3);
      expect(result.map(r => r.country)).toContain('JPN');
    });

    it('excludeJapan=true (ワールドモード): JPN レコードが除外される', () => {
      const result = filterAccommodations(items, baseCriteria({ excludeJapan: true }));
      expect(result).toHaveLength(2);
      expect(result.map(r => r.country)).not.toContain('JPN');
    });

    it('excludeJapan=true (カントリーモード country=JPN): 日本除外は無効、JPNデータが返る', () => {
      const result = filterAccommodations(items, baseCriteria({ excludeJapan: true, country: 'JPN' }));
      expect(result).toHaveLength(1);
      expect(result[0].country).toBe('JPN');
    });

    it('excludeJapan=true (カントリーモード country=FRA): 日本除外は無効、FRAデータが返る', () => {
      const result = filterAccommodations(items, baseCriteria({ excludeJapan: true, country: 'FRA' }));
      expect(result).toHaveLength(1);
      expect(result[0].country).toBe('FRA');
    });
  });

  describe('filterTransportations', () => {
    const items = [jpnTransportation, fraTransportation, jpnToFraTransportation];

    it('excludeJapan=true (ワールドモード): JPN 国内移動が除外される', () => {
      const result = filterTransportations(items, baseCriteria({ excludeJapan: true }));
      expect(result.find(r => r.id === 't-jpn')).toBeUndefined();
    });

    it('excludeJapan=true (ワールドモード): JPN発着の国境越えも除外される', () => {
      const result = filterTransportations(items, baseCriteria({ excludeJapan: true }));
      expect(result.find(r => r.id === 't-jpn-fra')).toBeUndefined();
    });

    it('excludeJapan=true (ワールドモード): FRA国内移動は残る', () => {
      const result = filterTransportations(items, baseCriteria({ excludeJapan: true }));
      expect(result.find(r => r.id === 't-fra')).toBeDefined();
    });

    it('excludeJapan=true (カントリーモード): 日本除外は無効', () => {
      const result = filterTransportations(items, baseCriteria({ excludeJapan: true, country: 'JPN' }));
      expect(result.find(r => r.id === 't-jpn')).toBeDefined();
    });
  });

  describe('filterSightseeings', () => {
    const items = [jpnSightseeing, fraSightseeing];

    it('excludeJapan=true (ワールドモード): JPN レコードが除外される', () => {
      const result = filterSightseeings(items, baseCriteria({ excludeJapan: true }));
      expect(result).toHaveLength(1);
      expect(result[0].country).toBe('FRA');
    });

    it('excludeJapan=false: 全レコードが返る', () => {
      const result = filterSightseeings(items, baseCriteria({ excludeJapan: false }));
      expect(result).toHaveLength(2);
    });
  });

  describe('filterOtherExpenses', () => {
    const items = [jpnOther, fraOther];

    it('excludeJapan=true (ワールドモード): JPN レコードが除外される', () => {
      const result = filterOtherExpenses(items, baseCriteria({ excludeJapan: true }));
      expect(result).toHaveLength(1);
      expect(result[0].country).toBe('FRA');
    });

    it('excludeJapan=false: 全レコードが返る', () => {
      const result = filterOtherExpenses(items, baseCriteria({ excludeJapan: false }));
      expect(result).toHaveLength(2);
    });
  });

  describe('excludeJapan + 他フィルタの組み合わせ', () => {
    it('excludeJapan=true + excludeAirplane=true: 両方適用される', () => {
      const items = [jpnTransportation, fraTransportation, jpnToFraTransportation];
      const result = filterTransportations(items, baseCriteria({
        excludeJapan: true,
        excludeAirplane: true,
      }));
      // jpn国内: excludeJapanで除外, jpn-fra: excludeJapan+excludeAirplaneで除外, fra国内: 残る
      expect(result).toHaveLength(1);
      expect(result[0].id).toBe('t-fra');
    });

    it('excludeJapan=true + excludeInsurance=true: 両方適用される', () => {
      const jpnInsurance = makeOtherExpense({ id: 'o-jpn-ins', country: 'JPN', otherType: 'Insurance' });
      const fraInsurance = makeOtherExpense({ id: 'o-fra-ins', country: 'FRA', otherType: 'Insurance' });
      const items = [jpnOther, fraOther, jpnInsurance, fraInsurance];
      const result = filterOtherExpenses(items, baseCriteria({
        excludeJapan: true,
        excludeInsurance: true,
      }));
      // jpnOther: excludeJapanで除外, fraOther: 残る, jpnInsurance: excludeJapanで除外, fraInsurance: excludeInsuranceで除外
      expect(result).toHaveLength(1);
      expect(result[0].id).toBe('o-fra');
    });
  });
});
