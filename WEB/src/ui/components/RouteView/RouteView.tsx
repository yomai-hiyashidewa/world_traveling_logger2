import { useState } from 'react';
import { Transportation } from '../../../domain/models/transportation';
import {
  BorderCrossing, ArrivalRecord, DepartureRecord, StayPeriod, RouteDetail,
  getArrivalDetail, getDepartureDetail, formatRouteDistance, formatRouteTime,
} from '../../../domain/services/routeService';
import { countryName, CountryType } from '../../../domain/models/types';
import { CountryFlag } from '../CountryFlag/CountryFlag';
import styles from './RouteView.module.css';

interface RouteViewProps {
  selectedCountry: CountryType | null;
  borderCrossings: BorderCrossing[];
  countryRoutes: Transportation[];
  arrivals: ArrivalRecord[];
  departures: DepartureRecord[];
  stayPeriod: StayPeriod | null;
}

function fmtDate(d: Date): string {
  return `${d.getFullYear()}/${d.getMonth() + 1}/${d.getDate()}`;
}

// --- World mode: border crossing list ---

function WorldRouteView({ borderCrossings }: { borderCrossings: BorderCrossing[] }) {
  return (
    <div>
      <h3>Border Crossings ({borderCrossings.length})</h3>
      <div className={styles.timeline}>
        {borderCrossings.map((bc, i) => (
          <div key={i} className={`${styles.crossing} ${bc.isNoEntry ? styles.noEntry : ''}`}>
            <div className={styles.date}>{fmtDate(bc.date)}</div>
            <div className={styles.route}>
              <CountryFlag countryCode={bc.fromCountry} size={20} />
              <span className={styles.country}>{countryName(bc.fromCountry)}</span>
              <span className={styles.arrow}>&rarr;</span>
              <CountryFlag countryCode={bc.toCountry} size={20} />
              <span className={styles.country}>{countryName(bc.toCountry)}</span>
            </div>
            <div className={styles.transport}>{bc.transportationType}</div>
            {bc.isNoEntry && <div className={styles.noEntryBadge}>NO ENTRY</div>}
          </div>
        ))}
        {borderCrossings.length === 0 && <div className={styles.empty}>No border crossings</div>}
      </div>
    </div>
  );
}

// --- Country mode: arrivals/departures panel ---

function CountryListPanel({
  title, items, getCountry, getDetail, variant = 'arrival',
}: {
  title: string;
  items: (ArrivalRecord | DepartureRecord)[];
  getCountry: (item: ArrivalRecord | DepartureRecord) => CountryType;
  getDetail: (item: ArrivalRecord | DepartureRecord) => RouteDetail;
  variant?: 'arrival' | 'departure';
}) {
  const [selectedIdx, setSelectedIdx] = useState<number | null>(items.length > 0 ? 0 : null);
  const selected = selectedIdx !== null ? items[selectedIdx] : null;
  const detail = selected ? getDetail(selected) : null;

  return (
    <div className={styles.sidePanel}>
      <h4>{title} ({items.length})</h4>
      <div className={styles.countryList}>
        {items.map((item, i) => {
          const c = getCountry(item);
          return (
            <div
              key={i}
              className={`${styles.countryItem} ${selectedIdx === i ? (variant === 'departure' ? styles.selectedDep : styles.selected) : ''}`}
              onClick={() => setSelectedIdx(selectedIdx === i ? null : i)}
            >
              <CountryFlag countryCode={c} size={18} />
              <span>{countryName(c)}</span>
            </div>
          );
        })}
        {items.length === 0 && <div className={styles.empty}>None</div>}
      </div>
      {detail && (
        <div className={styles.detail}>
          <div>{detail.region ?? '-'}</div>
          <div className={styles.detailType}>{detail.type}</div>
          <div className={styles.detailMetrics}>{detail.distance} / {detail.time}</div>
          {detail.isOvernight && <div className={styles.overnightBadge}>zzz</div>}
        </div>
      )}
    </div>
  );
}

// --- Country mode: domestic route list ---

function DomesticRouteList({
  routes, country, stayPeriod,
}: {
  routes: Transportation[];
  country: CountryType;
  stayPeriod: StayPeriod | null;
}) {
  return (
    <div className={styles.centerPanel}>
      <div className={styles.stayHeader}>
        <CountryFlag countryCode={country} size={24} />
        <span className={styles.stayCountry}>{countryName(country)}</span>
        {stayPeriod && (
          <span className={styles.stayDates}>
            {fmtDate(stayPeriod.startDate)} ~ {fmtDate(stayPeriod.endDate)}
          </span>
        )}
      </div>
      <div className={styles.routeList}>
        {routes.map((t, i) => {
          const isDomestic = t.startCountry === t.endCountry;
          const isArrival = t.endCountry === country && t.startCountry !== country;
          const isDeparture = t.startCountry === country && t.endCountry !== country;
          const isOvernight = t.startDate.getTime() !== t.endDate.getTime();

          if (isArrival) {
            return (
              <div key={i} className={`${styles.routeItem} ${styles.arrivalItem}`}>
                <div className={styles.routeDate}>{fmtDate(t.endDate)}</div>
                <div className={styles.routeLabel}>
                  <CountryFlag countryCode={t.startCountry} size={16} />
                  <span>Arrival from {countryName(t.startCountry)}</span>
                  {isOvernight && <span className={styles.overnightBadge}>zzz</span>}
                </div>
                <div className={styles.routeRegion}>{t.endRegion ?? '-'}</div>
              </div>
            );
          }

          if (isDeparture) {
            return (
              <div key={i} className={`${styles.routeItem} ${styles.departureItem}`}>
                <div className={styles.routeDate}>{fmtDate(t.startDate)}</div>
                <div className={`${styles.routeLabel} ${styles.departureLabelRight}`}>
                  <CountryFlag countryCode={t.endCountry} size={16} />
                  <span>Departure to {countryName(t.endCountry)}</span>
                  {isOvernight && <span className={styles.overnightBadge}>zzz</span>}
                </div>
                <div className={styles.routeRegion}>{t.startRegion ?? '-'}</div>
              </div>
            );
          }

          // Domestic
          return (
            <div key={i} className={styles.routeItem}>
              <div className={styles.routeDate}>{fmtDate(t.startDate)}</div>
              <div className={styles.domesticRoute}>
                <span className={styles.regionLabel}>{t.startRegion ?? '-'}</span>
                <span className={styles.placeLabel}>{t.startPlace}</span>
                <div className={styles.transportBox}>
                  <span className={styles.transportType}>{t.transportationType}</span>
                  <span className={styles.transportMetrics}>
                    {formatRouteDistance(t.distance)}, {formatRouteTime(t.time)}
                  </span>
                  {isOvernight && <span className={styles.overnightBadge}>zzz</span>}
                </div>
                <span className={styles.placeLabel}>{t.endPlace}</span>
                <span className={styles.regionLabel}>{t.endRegion ?? '-'}</span>
              </div>
            </div>
          );
        })}
        {routes.length === 0 && <div className={styles.empty}>No routes</div>}
      </div>
    </div>
  );
}

// --- Main RouteView ---

export function RouteView(props: RouteViewProps) {
  if (!props.selectedCountry) {
    return <WorldRouteView borderCrossings={props.borderCrossings} />;
  }

  return (
    <div className={styles.threePanel}>
      <CountryListPanel
        title="Arrivals"
        items={props.arrivals}
        getCountry={item => (item as ArrivalRecord).fromCountry}
        getDetail={item => getArrivalDetail(item as ArrivalRecord)}
      />
      <DomesticRouteList
        routes={props.countryRoutes}
        country={props.selectedCountry}
        stayPeriod={props.stayPeriod}
      />
      <CountryListPanel
        title="Departures"
        variant="departure"
        items={props.departures}
        getCountry={item => (item as DepartureRecord).toCountry}
        getDetail={item => getDepartureDetail(item as DepartureRecord)}
      />
    </div>
  );
}
