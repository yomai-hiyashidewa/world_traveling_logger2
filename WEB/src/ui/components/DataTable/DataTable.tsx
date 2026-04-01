import styles from './DataTable.module.css';

export interface Column<T> {
  key: string;
  header: string;
  render: (item: T) => React.ReactNode;
  align?: 'left' | 'right' | 'center';
}

interface DataTableProps<T> {
  columns: Column<T>[];
  data: T[];
  keyFn: (item: T) => string;
}

export function DataTable<T>({ columns, data, keyFn }: DataTableProps<T>) {
  return (
    <div className={styles.tableWrapper}>
      <table className={styles.table}>
        <thead>
          <tr>
            {columns.map(col => (
              <th key={col.key} style={{ textAlign: col.align ?? 'left' }}>{col.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map(item => (
            <tr key={keyFn(item)}>
              {columns.map(col => (
                <td key={col.key} style={{ textAlign: col.align ?? 'left' }}>{col.render(item)}</td>
              ))}
            </tr>
          ))}
          {data.length === 0 && (
            <tr>
              <td colSpan={columns.length} className={styles.empty}>No data</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
