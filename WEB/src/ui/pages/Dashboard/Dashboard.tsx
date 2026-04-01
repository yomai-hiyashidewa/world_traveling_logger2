import { useState } from 'react';
import { TabType, countryName, MajorCurrencyType } from '../../../domain/models/types';
import { formatCurrency } from '../../../domain/models/summary';
import { FilterPanel } from '../../components/FilterPanel/FilterPanel';
import { DataTable, Column } from '../../components/DataTable/DataTable';
import { StatsSummary } from '../../components/StatsSummary/StatsSummary';
import { RouteView } from '../../components/RouteView/RouteView';
import { FilterState } from '../../hooks/useFilterState';
import { Stats } from '../../hooks/useStats';
import { Accommodation } from '../../../domain/models/accommodation';
import { Transportation } from '../../../domain/models/transportation';
import { Sightseeing } from '../../../domain/models/sightseeing';
import { OtherExpense } from '../../../domain/models/otherExpense';
import styles from './Dashboard.module.css';

interface DashboardProps {
  filterState: FilterState;
  stats: Stats;
}

function fmtDate(d: Date): string {
  return `${d.getFullYear()}/${d.getMonth() + 1}/${d.getDate()}`;
}

function fmtCur(amount: number, currency: MajorCurrencyType): string {
  return formatCurrency(amount, currency);
}

const TABS: { key: TabType; label: string }[] = [
  { key: 'accommodation', label: 'Accommodation' },
  { key: 'transportation', label: 'Transportation' },
  { key: 'sightseeing', label: 'Sightseeing' },
  { key: 'other', label: 'Other Expenses' },
  { key: 'summary', label: 'Summary' },
  { key: 'route', label: 'Route' },
];

export function Dashboard({ filterState, stats }: DashboardProps) {
  const [activeTab, setActiveTab] = useState<TabType>('accommodation');
  const c = filterState.displayCurrency;

  const accColumns: Column<Accommodation>[] = [
    { key: 'date', header: 'Date', render: a => fmtDate(a.date) },
    { key: 'country', header: 'Country', render: a => countryName(a.country) },
    { key: 'region', header: 'Region', render: a => a.region ?? '-' },
    { key: 'type', header: 'Type', render: a => a.accommodationType },
    { key: 'price', header: 'Price', render: a => `${a.price} ${a.currency}`, align: 'right' },
    { key: 'converted', header: c, render: a => fmtCur(getPriceByC(a, c), c), align: 'right' },
    { key: 'memo', header: 'Memo', render: a => a.memo ?? '-' },
  ];

  const transColumns: Column<Transportation>[] = [
    { key: 'type', header: 'Type', render: t => t.transportationType },
    { key: 'startDate', header: 'Dep Date', render: t => fmtDate(t.startDate) },
    { key: 'from', header: 'From', render: t => `${countryName(t.startCountry)} ${t.startRegion ?? ''}` },
    { key: 'endDate', header: 'Arr Date', render: t => fmtDate(t.endDate) },
    { key: 'to', header: 'To', render: t => `${countryName(t.endCountry)} ${t.endRegion ?? ''}` },
    { key: 'dist', header: 'Dist(km)', render: t => t.distance.toLocaleString(), align: 'right' },
    { key: 'time', header: 'Time(min)', render: t => t.time.toString(), align: 'right' },
    { key: 'price', header: 'Price', render: t => `${t.price} ${t.currency}`, align: 'right' },
    { key: 'converted', header: c, render: t => fmtCur(getPriceByC(t, c), c), align: 'right' },
  ];

  const sightColumns: Column<Sightseeing>[] = [
    { key: 'date', header: 'Date', render: s => fmtDate(s.date) },
    { key: 'context', header: 'Activity', render: s => s.context },
    { key: 'type', header: 'Type', render: s => s.sightseeingType },
    { key: 'country', header: 'Country', render: s => countryName(s.country) },
    { key: 'region', header: 'Region', render: s => s.region ?? '-' },
    { key: 'price', header: 'Price', render: s => `${s.price} ${s.currency}`, align: 'right' },
    { key: 'converted', header: c, render: s => fmtCur(getPriceByC(s, c), c), align: 'right' },
  ];

  const otherColumns: Column<OtherExpense>[] = [
    { key: 'date', header: 'Date', render: o => fmtDate(o.date) },
    { key: 'type', header: 'Type', render: o => o.otherType },
    { key: 'context', header: 'Description', render: o => o.context ?? '-' },
    { key: 'country', header: 'Country', render: o => countryName(o.country) },
    { key: 'region', header: 'Region', render: o => o.region ?? '-' },
    { key: 'price', header: 'Price', render: o => `${o.price} ${o.currency}`, align: 'right' },
    { key: 'converted', header: c, render: o => fmtCur(getPriceByC(o, c), c), align: 'right' },
  ];

  return (
    <div className={styles.dashboard}>
      <FilterPanel
        countries={filterState.countries}
        regions={filterState.regions}
        selectedCountry={filterState.criteria.country}
        selectedRegion={filterState.criteria.region}
        startDate={filterState.criteria.startDate}
        endDate={filterState.criteria.endDate}
        displayCurrency={filterState.displayCurrency}
        excludeAirplane={filterState.criteria.excludeAirplane}
        excludeInsurance={filterState.criteria.excludeInsurance}
        excludeCrossBorder={filterState.criteria.excludeCrossBorder}
        excludeJapan={filterState.criteria.excludeJapan}
        onCountryChange={filterState.setCountry}
        onRegionChange={filterState.setRegion}
        onStartDateChange={filterState.setStartDate}
        onEndDateChange={filterState.setEndDate}
        onCurrencyChange={filterState.setDisplayCurrency}
        onToggleExcludeAirplane={filterState.toggleExcludeAirplane}
        onToggleExcludeInsurance={filterState.toggleExcludeInsurance}
        onToggleExcludeCrossBorder={filterState.toggleExcludeCrossBorder}
        onToggleExcludeJapan={filterState.toggleExcludeJapan}
        onClear={filterState.clearFilters}
      />

      <div className={styles.tabs}>
        {TABS.map(tab => (
          <button
            key={tab.key}
            className={`${styles.tab} ${activeTab === tab.key ? styles.active : ''}`}
            onClick={() => setActiveTab(tab.key)}
          >
            {tab.label}
            {tab.key === 'accommodation' && ` (${filterState.filteredAccommodations.length})`}
            {tab.key === 'transportation' && ` (${filterState.filteredTransportations.length})`}
            {tab.key === 'sightseeing' && ` (${filterState.filteredSightseeings.length})`}
            {tab.key === 'other' && ` (${filterState.filteredOtherExpenses.length})`}
          </button>
        ))}
      </div>

      <div className={styles.content}>
        {activeTab === 'accommodation' && (
          <DataTable columns={accColumns} data={filterState.filteredAccommodations} keyFn={a => a.id} />
        )}
        {activeTab === 'transportation' && (
          <DataTable columns={transColumns} data={filterState.filteredTransportations} keyFn={t => t.id} />
        )}
        {activeTab === 'sightseeing' && (
          <DataTable columns={sightColumns} data={filterState.filteredSightseeings} keyFn={s => s.id} />
        )}
        {activeTab === 'other' && (
          <DataTable columns={otherColumns} data={filterState.filteredOtherExpenses} keyFn={o => o.id} />
        )}
        {activeTab === 'summary' && <StatsSummary stats={stats} />}
        {activeTab === 'route' && (
          <RouteView
            selectedCountry={filterState.criteria.country}
            borderCrossings={stats.borderCrossings}
            countryRoutes={stats.countryRoutes}
            arrivals={stats.arrivals}
            departures={stats.departures}
            stayPeriod={stats.stayPeriod}
          />
        )}
      </div>
    </div>
  );
}

function getPriceByC(item: { jpyPrice: number; eurPrice: number; usdPrice: number }, c: MajorCurrencyType): number {
  switch (c) {
    case 'JPY': return item.jpyPrice;
    case 'EUR': return item.eurPrice;
    case 'USD': return item.usdPrice;
    default: return item.jpyPrice;
  }
}
