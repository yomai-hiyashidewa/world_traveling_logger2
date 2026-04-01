import { useState, useEffect } from 'react';
import { useTravelData } from './hooks/useTravelData';
import { useFilterState } from './hooks/useFilterState';
import { useStats } from './hooks/useStats';
import { Dashboard } from './pages/Dashboard/Dashboard';
import { CountryFlag } from './components/CountryFlag/CountryFlag';
import { getCountryImagePath } from '../domain/services/imageService';
import { countryName } from '../domain/models/types';
import styles from './App.module.css';

const BASE = import.meta.env.BASE_URL;

export function App() {
  const [imgError, setImgError] = useState(false);
  const data = useTravelData();
  const filterState = useFilterState(
    data.accommodations,
    data.transportations,
    data.sightseeings,
    data.otherExpenses,
  );
  const stats = useStats(
    filterState.filteredAccommodations,
    filterState.filteredTransportations,
    filterState.filteredSightseeings,
    filterState.filteredOtherExpenses,
    filterState.displayCurrency,
    filterState.criteria.country,
  );

  const selectedCountry = filterState.criteria.country;
  const countryImageSrc = getCountryImagePath(selectedCountry, BASE);
  const fallbackSrc = getCountryImagePath(null, BASE);

  useEffect(() => { setImgError(false); }, [selectedCountry]);

  if (data.loading) {
    return (
      <div className={styles.loadingScreen}>
        <div className={styles.spinner} />
        <p>Loading travel data...</p>
      </div>
    );
  }

  if (data.error) {
    return (
      <div className={styles.errorScreen}>
        <h2>Failed to load data</h2>
        <pre>{data.error}</pre>
      </div>
    );
  }

  return (
    <div className={styles.app}>
      <header className={styles.header}>
        <div className={styles.headerImage}>
          <img
            src={imgError ? fallbackSrc : countryImageSrc}
            alt={selectedCountry ? countryName(selectedCountry) : 'World'}
            onError={() => { if (!imgError) setImgError(true); }}
            key={countryImageSrc}
          />
        </div>
        <div className={styles.headerText}>
          <h1 className={styles.title}>
            {selectedCountry && <CountryFlag countryCode={selectedCountry} size={28} />}
            {' '}
            {selectedCountry ? countryName(selectedCountry) : 'World Traveling Logger'}
          </h1>
          <div className={styles.headerInfo}>
            {data.accommodations.length} stays &middot;{' '}
            {data.transportations.length} trips &middot;{' '}
            {data.sightseeings.length} activities
          </div>
        </div>
      </header>
      <main className={styles.main}>
        <Dashboard filterState={filterState} stats={stats} />
      </main>
    </div>
  );
}
