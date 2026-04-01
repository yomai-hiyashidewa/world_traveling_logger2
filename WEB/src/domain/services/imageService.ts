import { CountryType } from '../models/types';

function joinPath(base: string, ...parts: string[]): string {
  const b = base.endsWith('/') ? base.slice(0, -1) : base;
  return b + '/' + parts.join('/');
}

export function getFlagPath(countryCode: CountryType, base: string): string {
  return joinPath(base, 'image', 'Flags', `${countryCode}.png`);
}

export function getCountryImagePath(
  countryCode: CountryType | null | undefined,
  base: string,
): string {
  if (!countryCode) return getWorldImagePath(base);
  return joinPath(base, 'image', 'Countries', countryCode, 'zero.jpg');
}

export function getWorldImagePath(base: string): string {
  return joinPath(base, 'image', 'world.JPEG');
}

export function getFaviconPath(base: string): string {
  return joinPath(base, 'image', 'Icon', 'icon.png');
}
