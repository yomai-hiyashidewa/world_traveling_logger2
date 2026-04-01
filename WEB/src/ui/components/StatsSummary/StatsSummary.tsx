import { CostSummary, formatCurrency } from '../../../domain/models/summary';
import { Stats } from '../../hooks/useStats';
import styles from './StatsSummary.module.css';

interface StatsSummaryProps {
  stats: Stats;
}

function CostCard({ label, amount, currency }: { label: string; amount: number; currency: CostSummary['currency'] }) {
  return (
    <div className={styles.card}>
      <div className={styles.cardLabel}>{label}</div>
      <div className={styles.cardValue}>{formatCurrency(amount, currency)}</div>
    </div>
  );
}

export function StatsSummary({ stats }: StatsSummaryProps) {
  const { costSummary, movingSummary, countryCount } = stats;
  const c = costSummary.currency;

  return (
    <div className={styles.container}>
      <div className={styles.section}>
        <h3>Cost Summary ({c})</h3>
        <div className={styles.cardGrid}>
          <CostCard label="Accommodation" amount={costSummary.accommodation} currency={c} />
          <CostCard label="Transportation" amount={costSummary.transportation} currency={c} />
          <CostCard label="Sightseeing" amount={costSummary.sightseeing} currency={c} />
          <CostCard label="Other" amount={costSummary.other} currency={c} />
          <CostCard label="Total" amount={costSummary.total} currency={c} />
        </div>
      </div>

      <div className={styles.section}>
        <h3>Moving Summary</h3>
        <div className={styles.cardGrid}>
          <div className={styles.card}>
            <div className={styles.cardLabel}>Total Distance</div>
            <div className={styles.cardValue}>{movingSummary.formattedDistance}</div>
          </div>
          <div className={styles.card}>
            <div className={styles.cardLabel}>Total Time</div>
            <div className={styles.cardValue}>{movingSummary.formattedTime}</div>
          </div>
          <div className={styles.card}>
            <div className={styles.cardLabel}>Countries Visited</div>
            <div className={styles.cardValue}>{countryCount}</div>
          </div>
        </div>
      </div>

      <div className={styles.section}>
        <h3>By Accommodation Type</h3>
        <table className={styles.summaryTable}>
          <thead>
            <tr><th>Type</th><th>Count</th><th>Total</th><th>Avg</th></tr>
          </thead>
          <tbody>
            {stats.accommodationTypeSummary.map(s => (
              <tr key={s.type}>
                <td>{s.type}</td>
                <td>{s.count}</td>
                <td>{formatCurrency(s.totalCost, c)}</td>
                <td>{formatCurrency(s.averageCost, c)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className={styles.section}>
        <h3>By Transportation Type</h3>
        <table className={styles.summaryTable}>
          <thead>
            <tr><th>Type</th><th>Count</th><th>Total Cost</th><th>Total Dist</th><th>Total Time</th></tr>
          </thead>
          <tbody>
            {stats.transportationTypeSummary.map(s => (
              <tr key={s.type}>
                <td>{s.type}</td>
                <td>{s.count}</td>
                <td>{formatCurrency(s.totalCost, c)}</td>
                <td>{Math.round(s.totalDistance).toLocaleString()} km</td>
                <td>{Math.round(s.totalTime)} min</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className={styles.section}>
        <h3>By Sightseeing Type</h3>
        <table className={styles.summaryTable}>
          <thead>
            <tr><th>Type</th><th>Count</th><th>Total</th><th>Avg</th></tr>
          </thead>
          <tbody>
            {stats.sightseeingTypeSummary.map(s => (
              <tr key={s.type}>
                <td>{s.type}</td>
                <td>{s.count}</td>
                <td>{formatCurrency(s.totalCost, c)}</td>
                <td>{formatCurrency(s.averageCost, c)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className={styles.section}>
        <h3>By Other Expense Type</h3>
        <table className={styles.summaryTable}>
          <thead>
            <tr><th>Type</th><th>Count</th><th>Total</th><th>Avg</th></tr>
          </thead>
          <tbody>
            {stats.otherTypeSummary.map(s => (
              <tr key={s.type}>
                <td>{s.type}</td>
                <td>{s.count}</td>
                <td>{formatCurrency(s.totalCost, c)}</td>
                <td>{formatCurrency(s.averageCost, c)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
