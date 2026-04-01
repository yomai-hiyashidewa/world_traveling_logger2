# SP-API-001: データ読み込みサービスインターフェース仕様

対応要求: FR-001

※ GitHub Pages（静的ホスティング）のためREST APIは存在しない。
本仕様はクライアント内部のサービスインターフェースを定義する。

## CSV読み込みインターフェース

### csvReader

全パーサーの基盤となるユーティリティモジュール。

#### parseCsvLines

```typescript
function parseCsvLines(text: string): string[][]
```

- 入力: CSVテキスト全体
- 出力: 2次元文字列配列（行×列）
- 処理: `\r` 除去 → `\n` で行分割 → 空行除外 → カンマ分割 → 各セルのダブルクォート除去・trim

#### parseDate

```typescript
function parseDate(s: string): Date
```

- 入力: `yyyy/M/d` 形式の日付文字列（例: `2023/3/15`）
- 出力: `Date` オブジェクト（ローカル時間）
- `/` で分割し3パートでなければ `Error` をスロー

#### dateToMonthKey

```typescript
function dateToMonthKey(d: Date): string
```

- 入力: `Date` オブジェクト
- 出力: `yyyy/M/1` 形式の月キー文字列（例: `2023/3/1`）
- 月別集計のグルーピングキーとして使用

#### genId

```typescript
function genId(): string
```

- 出力: `id-{連番}-{ランダム6文字}` 形式のユニークID
- 内部カウンタ + `Math.random()` で一意性を保証
- CSVにIDカラムがないため、パース時に各レコードへ付与

#### cleanValue

```typescript
function cleanValue(s: string): string | null
```

- 入力: CSVセル文字列
- 出力: trim後の文字列。`-` または空文字の場合は `null`
- region, memo など省略可能フィールドの変換に使用

### accommodationParser

```typescript
interface ParseResult<T> {
  data: T[];
  errors: FileError[];
}

interface FileError {
  line: number;
  column: number;
  message: string;
}

function parseAccommodations(
  rows: string[][],
  exchangeRater: ExchangeRateService
): ParseResult<Accommodation>
```

### transportationParser

```typescript
function parseTransportations(
  rows: string[][],
  exchangeRater: ExchangeRateService
): ParseResult<Transportation>
```

### sightseeingParser

```typescript
interface SightseeingParseResult {
  sightseeings: Sightseeing[];
  others: OtherExpense[];       // 経費系として振り分けられたもの
  errors: FileError[];
}

function parseSightseeings(
  rows: string[][],
  exchangeRater: ExchangeRateService
): SightseeingParseResult
```

### exchangeRateParser

```typescript
function parseExchangeRates(
  rows: string[][]
): ParseResult<ExchangeRate>
```

## データ取得フロー

```typescript
// 起動時のデータ取得
async function loadAllData(): Promise<TravelData> {
  const [accCsv, transCsv, sightCsv, exchCsv] = await Promise.all([
    fetch("./data/accommodations.csv").then(r => r.text()),
    fetch("./data/transportations.csv").then(r => r.text()),
    fetch("./data/sightseeing.csv").then(r => r.text()),
    fetch("./data/exchange_rates.csv").then(r => r.text()),
  ]);

  const exchRates = parseExchangeRates(parseCsv(exchCsv));
  const exchService = new ExchangeRateService(exchRates.data);

  const accommodations = parseAccommodations(parseCsv(accCsv), exchService);
  const transportations = parseTransportations(parseCsv(transCsv), exchService);
  const sightseeings = parseSightseeings(parseCsv(sightCsv), exchService);

  return { accommodations, transportations, sightseeings, exchService };
}
```

## テスト仕様

#### UT-PARSE-001-01: csvReader
- 正常CSV → 2次元配列
- 空行の扱い
- カンマを含まない1列のみの行

#### UT-PARSE-001-02: accommodationParser
- 正常行 → Accommodation オブジェクト
- 不正日付 → errors に FileError 追加
- 未知国コード → errors に FileError 追加
- 未知宿泊タイプ → errors に FileError 追加

#### UT-PARSE-001-03: transportationParser
- 正常行 → Transportation オブジェクト（全フィールド）
- 距離ベース自動分類が適用されること
- 不正フォーマットのエラー収集

#### UT-PARSE-001-04: sightseeingParser
- 観光系タイプ → sightseeings に格納
- 経費系タイプ → others に振り分け
- キーワード自動分類の適用

#### UT-PARSE-001-05: exchangeRateParser
- 正常行 → ExchangeRate オブジェクト
- 不正レート値のエラー

#### IT-LOAD-001-01: loadAllData 統合テスト
- documents/English/ のCSVを使った全データ読み込み
- エラーなく全データがパースされること
- 通貨変換が適用されていること
