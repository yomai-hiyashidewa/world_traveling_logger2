import { describe, it, expect } from 'vitest';
import {
  getFlagPath,
  getCountryImagePath,
  getWorldImagePath,
  getFaviconPath,
} from '../../../src/domain/services/imageService';

describe('imageService', () => {
  const base = '/world_traveling_logger2/';

  describe('getFlagPath', () => {
    it('returns correct path for a known country code', () => {
      expect(getFlagPath('JPN', base)).toBe('/world_traveling_logger2/image/Flags/JPN.png');
    });

    it('returns correct path for another country code', () => {
      expect(getFlagPath('AUS', base)).toBe('/world_traveling_logger2/image/Flags/AUS.png');
    });

    it('handles country code with different casing by returning as-is', () => {
      expect(getFlagPath('usa', base)).toBe('/world_traveling_logger2/image/Flags/usa.png');
    });

    it('works with root base path', () => {
      expect(getFlagPath('FRA', '/')).toBe('/image/Flags/FRA.png');
    });
  });

  describe('getCountryImagePath', () => {
    it('returns country-specific image path when country is provided', () => {
      expect(getCountryImagePath('AUS', base)).toBe('/world_traveling_logger2/image/Countries/AUS/zero.jpg');
    });

    it('returns world image path when country is null', () => {
      expect(getCountryImagePath(null, base)).toBe('/world_traveling_logger2/image/world.JPEG');
    });

    it('returns world image path when country is undefined', () => {
      expect(getCountryImagePath(undefined, base)).toBe('/world_traveling_logger2/image/world.JPEG');
    });
  });

  describe('getWorldImagePath', () => {
    it('returns the world image path', () => {
      expect(getWorldImagePath(base)).toBe('/world_traveling_logger2/image/world.JPEG');
    });
  });

  describe('getFaviconPath', () => {
    it('returns the favicon path', () => {
      expect(getFaviconPath(base)).toBe('/world_traveling_logger2/image/Icon/icon.png');
    });
  });
});
