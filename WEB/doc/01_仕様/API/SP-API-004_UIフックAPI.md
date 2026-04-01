# SP-API-004: UI フックインターフェース仕様

対応要求: FR-001, FR-007, FR-008, FR-009

React Custom Hooks は元WPFの ViewModel 層に対応する。
本仕様はクライアント内部の Hooks インターフェースを定義する。

## useTravelData

元コード: MainModel（データ読み込み・保持）

### インターフェース

```typescript
interface TravelData {
  accommodations: Accommodation[];
  transportations: Transportation[];
  sightseeings: Sightseeing[];
  otherExpenses: OtherExpense[];
  exchangeRates: ExchangeRateTable;
  loading: boolean;
  error: string | null;
}

function useTravelData(): TravelData
```

### 責務

- アプリ起動時（マウント時）に4つのCSVファイルを並列 fetch
- パース → 為替レート適用 → React State に格納
- loading / error 状態の管理

### データ取得フロー

1. `useEffect` で初回マウント時に実行
2. `Promise.all` で4ファイルを並列取得:
   - `{BASE_URL}data/accommodations.csv`
   - `{BASE_URL}data/transportations.csv`
   - `{BASE_URL}data/sightseeing.csv`
   - `{BASE_URL}data/exchange_rates.csv`
3. 為替レートをパース → `ExchangeRateTable` を生成
4. 各CSVをパース → `applyExchangeRates` で通貨変換
5. 全データを `setState` で格納、`loading: false`
6. エラー時は `error` にメッセージを設定

### 初期値

| フィールド | 初期値 |
|---|---|
| accommodations | `[]` |
| transportations | `[]` |
| sightseeings | `[]` |
| otherExpenses | `[]` |
| exchangeRates | `{ entries: [], dates: [] }` |
| loading | `true` |
| error | `null` |

## useFilterState

元コード: ControlModel + SideViewModel（フィルタ条件管理・ビューモード）

### インターフェース

```typescript
interface FilterCriteria {
  country: CountryType | null;
  region: string | null;
  startDate: Date | null;
  endDate: Date | null;
  excludeAirplane: boolean;
  excludeInsurance: boolean;
  excludeCrossBorder: boolean;
  excludeJapan: boolean;
}

interface FilterState {
  // 状態
  criteria: FilterCriteria;
  displayCurrency: MajorCurrencyType;
  countries: CountryType[];
  regions: string[];
  filteredAccommodations: Accommodation[];
  filteredTransportations: Transportation[];
  filteredSightseeings: Sightseeing[];
  filteredOtherExpenses: OtherExpense[];

  // アクション
  setCountry: (country: CountryType | null) => void;
  setRegion: (region: string | null) => void;
  setStartDate: (date: Date | null) => void;
  setEndDate: (date: Date | null) => void;
  setDisplayCurrency: (currency: MajorCurrencyType) => void;
  toggleExcludeAirplane: () => void;
  toggleExcludeInsurance: () => void;
  toggleExcludeCrossBorder: () => void;
  toggleExcludeJapan: () => void;
  clearFilters: () => void;
}

function useFilterState(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  otherExpenses: OtherExpense[],
): FilterState
```

### 責務

- フィルタ条件（FilterCriteria）の保持と更新
- 表示通貨の管理
- 国リスト・地域リストの動的算出（`useMemo`）
- フィルタ適用済みレコードの算出（`useMemo`）

### ビューモード判定

FilterCriteria の状態から暗黙的にビューモードが決定される:

| criteria.country | criteria.region | ビューモード |
|---|---|---|
| `null` | `null` | ワールドモード |
| 値あり | `null` | カントリーモード |
| 値あり | 値あり | リージョンモード |

### 国選択時の副作用

`setCountry` は国変更と同時に `region: null` にリセットする。
これにより、カントリーモード切替時にリージョン選択がクリアされる。

### 初期値

| フィールド | 初期値 |
|---|---|
| criteria.country | `null` |
| criteria.region | `null` |
| criteria.startDate | `null` |
| criteria.endDate | `null` |
| criteria.excludeAirplane | `false` |
| criteria.excludeInsurance | `false` |
| criteria.excludeCrossBorder | `false` |
| criteria.excludeJapan | `false` |
| displayCurrency | `'JPY'` |

### メモ化（useMemo）

以下の値は依存配列に基づき再計算される:

| 値 | 依存 |
|---|---|
| countries | accommodations, transportations, sightseeings, otherExpenses |
| regions | accommodations, sightseeings, otherExpenses, criteria.country |
| filteredAccommodations | accommodations, criteria |
| filteredTransportations | transportations, criteria |
| filteredSightseeings | sightseeings, criteria |
| filteredOtherExpenses | otherExpenses, criteria |

## useStats

元コード: MainViewPanelVM（集計・ルート分析計算）

### インターフェース

```typescript
interface Stats {
  costSummary: CostSummary;
  accommodationTypeSummary: TypeSummary[];
  transportationTypeSummary: TransportationTypeSummary[];
  sightseeingTypeSummary: TypeSummary[];
  otherTypeSummary: TypeSummary[];
  movingSummary: MovingSummary;
  borderCrossings: BorderCrossing[];
  countryCount: number;
  countryRoutes: Transportation[];
  arrivals: ArrivalRecord[];
  departures: DepartureRecord[];
  stayPeriod: StayPeriod | null;
}

function useStats(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  otherExpenses: OtherExpense[],
  currency: MajorCurrencyType,
  selectedCountry: CountryType | null,
): Stats
```

### 責務

- フィルタ済みデータに対する全集計値の一括算出
- `useMemo` による再計算の最適化
- ルート分析（入国・出国・滞在期間）の計算

### 算出項目

| 項目 | サービス呼び出し | 条件 |
|---|---|---|
| costSummary | `calcCostSummary(acc, trans, sight, other, currency)` | 常時 |
| accommodationTypeSummary | `calcTypeSummary(acc, a => a.accommodationType, currency)` | 常時 |
| transportationTypeSummary | `calcTransportationTypeSummary(trans, currency)` | 常時 |
| sightseeingTypeSummary | `calcTypeSummary(sight, s => s.sightseeingType, currency)` | 常時 |
| otherTypeSummary | `calcTypeSummary(other, o => o.otherType, currency)` | 常時 |
| movingSummary | `calcMovingSummary(trans)` | 常時 |
| borderCrossings | `extractBorderCrossings(trans)` | 常時 |
| countryCount | `countCountries(trans)` | 常時 |
| countryRoutes | `getRoute(trans, selectedCountry)` | カントリーモード時のみ（それ以外は `[]`） |
| arrivals | `getArrivals(trans, selectedCountry)` | カントリーモード時のみ |
| departures | `getDepartures(trans, selectedCountry)` | カントリーモード時のみ |
| stayPeriod | `getCountryStayPeriod(countryRoutes)` | 常時（countryRoutes依存） |

### メモ化（useMemo）

全算出項目は単一の `useMemo` ブロックで一括計算される。
依存: `[accommodations, transportations, sightseeings, otherExpenses, currency, selectedCountry]`

## テスト仕様

#### UT-HOOK-001-01: useTravelData
- マウント時にCSVを並列 fetch すること
- パース → 為替レート適用の順序が正しいこと
- fetch 失敗時に error が設定されること
- loading 状態が正しく遷移すること

#### UT-HOOK-001-02: useFilterState
- 初期状態でフィルタなし（全レコード返却）
- setCountry で国フィルタが適用されること
- setCountry で region が null にリセットされること
- toggleExcludeAirplane で飛行機レコードが除外されること
- clearFilters で全条件が初期値に戻ること
- countries / regions が入力データから正しく算出されること

#### UT-HOOK-001-03: useStats
- フィルタ済みデータから全集計値が算出されること
- 通貨変更で costSummary が再計算されること
- selectedCountry 指定時に arrivals/departures が算出されること
- selectedCountry が null のとき arrivals/departures/countryRoutes が空配列であること
