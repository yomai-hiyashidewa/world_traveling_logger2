import { MajorCurrencyType, MAJOR_CURRENCIES, CountryType, countryName } from '../../../domain/models/types';
import { CountryFlag } from '../CountryFlag/CountryFlag';
import styles from './FilterPanel.module.css';

interface FilterPanelProps {
  countries: CountryType[];
  regions: string[];
  selectedCountry: CountryType | null;
  selectedRegion: string | null;
  startDate: Date | null;
  endDate: Date | null;
  displayCurrency: MajorCurrencyType;
  onCountryChange: (country: CountryType | null) => void;
  onRegionChange: (region: string | null) => void;
  onStartDateChange: (date: Date | null) => void;
  onEndDateChange: (date: Date | null) => void;
  excludeAirplane: boolean;
  excludeInsurance: boolean;
  excludeCrossBorder: boolean;
  excludeJapan: boolean;
  onCurrencyChange: (currency: MajorCurrencyType) => void;
  onToggleExcludeAirplane: () => void;
  onToggleExcludeInsurance: () => void;
  onToggleExcludeCrossBorder: () => void;
  onToggleExcludeJapan: () => void;
  onClear: () => void;
}

function toInputDate(d: Date | null): string {
  if (!d) return '';
  return d.toISOString().slice(0, 10);
}

export function FilterPanel(props: FilterPanelProps) {
  return (
    <div className={styles.filterPanel}>
      <div className={styles.filterRow}>
        <label>
          Country
          <div className={styles.countrySelect}>
            {props.selectedCountry && <CountryFlag countryCode={props.selectedCountry} size={20} />}
            <select
              value={props.selectedCountry ?? ''}
              onChange={e => props.onCountryChange(e.target.value || null)}
            >
              <option value="">All</option>
              {props.countries.map(c => <option key={c} value={c}>{countryName(c)}</option>)}
            </select>
          </div>
        </label>

        <label>
          Region
          <select
            value={props.selectedRegion ?? ''}
            onChange={e => props.onRegionChange(e.target.value || null)}
          >
            <option value="">All</option>
            {props.regions.map(r => <option key={r} value={r}>{r}</option>)}
          </select>
        </label>

        <label>
          From
          <input
            type="date"
            value={toInputDate(props.startDate)}
            onChange={e => props.onStartDateChange(e.target.value ? new Date(e.target.value) : null)}
          />
        </label>

        <label>
          To
          <input
            type="date"
            value={toInputDate(props.endDate)}
            onChange={e => props.onEndDateChange(e.target.value ? new Date(e.target.value) : null)}
          />
        </label>

        <label>
          Currency
          <select
            value={props.displayCurrency}
            onChange={e => props.onCurrencyChange(e.target.value as MajorCurrencyType)}
          >
            {MAJOR_CURRENCIES.map(c => <option key={c} value={c}>{c}</option>)}
          </select>
        </label>

        <label className={styles.checkLabel}>
          <input
            type="checkbox"
            checked={props.excludeAirplane}
            onChange={props.onToggleExcludeAirplane}
          />
          Exclude Airplane
        </label>

        <label className={styles.checkLabel}>
          <input
            type="checkbox"
            checked={props.excludeInsurance}
            onChange={props.onToggleExcludeInsurance}
          />
          Exclude Insurance
        </label>

        {props.selectedCountry && (
          <label className={styles.checkLabel}>
            <input
              type="checkbox"
              checked={props.excludeCrossBorder}
              onChange={props.onToggleExcludeCrossBorder}
            />
            Exclude Cross-Border
          </label>
        )}

        {!props.selectedCountry && (
          <label className={styles.checkLabel}>
            <input
              type="checkbox"
              checked={props.excludeJapan}
              onChange={props.onToggleExcludeJapan}
            />
            Exclude Japan
          </label>
        )}

        <button onClick={props.onClear} className={styles.clearBtn}>Clear</button>
      </div>
    </div>
  );
}
