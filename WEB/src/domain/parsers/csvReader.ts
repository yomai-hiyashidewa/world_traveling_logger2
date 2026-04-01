export function parseCsvLines(text: string): string[][] {
  const lines = text.replace(/\r/g, '').split('\n').filter(l => l.trim().length > 0);
  return lines.map(line => line.split(',').map(cell => cell.replace(/^"|"$/g, '').trim()));
}

export function parseDate(s: string): Date {
  // yyyy/M/d format
  const parts = s.split('/');
  if (parts.length !== 3) throw new Error(`Invalid date: ${s}`);
  const [y, m, d] = parts.map(Number);
  return new Date(y, m - 1, d);
}

export function dateToMonthKey(d: Date): string {
  return `${d.getFullYear()}/${d.getMonth() + 1}/1`;
}

let _idCounter = 0;
export function genId(): string {
  return `id-${++_idCounter}-${Math.random().toString(36).slice(2, 8)}`;
}

export function cleanValue(s: string): string | null {
  const v = s.trim();
  return v === '-' || v === '' ? null : v;
}
