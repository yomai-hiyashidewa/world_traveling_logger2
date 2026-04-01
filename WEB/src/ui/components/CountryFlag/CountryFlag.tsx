import { useState } from 'react';
import { CountryType } from '../../../domain/models/types';
import { getFlagPath } from '../../../domain/services/imageService';

interface CountryFlagProps {
  countryCode: CountryType;
  size?: number;
}

const BASE = import.meta.env.BASE_URL;

export function CountryFlag({ countryCode, size = 22 }: CountryFlagProps) {
  const [error, setError] = useState(false);
  if (error) return null;

  return (
    <img
      src={getFlagPath(countryCode, BASE)}
      alt={countryCode}
      width={size}
      height={Math.round(size * 15 / 22)}
      onError={() => setError(true)}
      style={{ objectFit: 'cover', borderRadius: 2, verticalAlign: 'middle' }}
    />
  );
}
