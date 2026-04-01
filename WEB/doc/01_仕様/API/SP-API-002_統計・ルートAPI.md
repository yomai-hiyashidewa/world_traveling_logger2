# SP-API-002: 統計・ルートサービスインターフェース仕様

対応要求: FR-008, FR-009

※ GitHub Pages（静的ホスティング）のためREST APIは存在しない。
本仕様はクライアント内部のサービスインターフェースを定義する。

## StatsService インターフェース

### calcCostSummary

```typescript
function calcCostSummary(
  accommodations: Accommodation[],
  transportations: Transportation[],
  sightseeings: Sightseeing[],
  others: OtherExpense[],
  currency: MajorCurrencyType
): CostSummary
```

- 各カテゴリの指定通貨金額を合算
- totalは4カテゴリ合計

### calcTypeSummary

```typescript
function calcAccommodationTypeSummary(
  records: Accommodation[],
  currency: MajorCurrencyType
): TypeSummary[]

function calcTransportationTypeSummary(
  records: Transportation[],
  currency: MajorCurrencyType
): TransportationTypeSummary[]

function calcSightseeingTypeSummary(
  records: Sightseeing[],
  currency: MajorCurrencyType
): TypeSummary[]

function calcOtherTypeSummary(
  records: OtherExpense[],
  currency: MajorCurrencyType
): TypeSummary[]
```

- レコードをタイプごとにグルーピング
- count, totalCost, maxCost, minCost, averageCost を算出
- TransportationTypeSummary は距離・時間の集計も含む

### calcMovingSummary

```typescript
function calcMovingSummary(
  transportations: Transportation[]
): MovingSummary
```

- 全交通記録の distance 合計、time 合計
- フォーマット文字列の生成

### calcOverview

```typescript
interface TravelOverview {
  countriesCount: number;
  regionsCount: number;
  totalDays: number;
  startDate: Date;
  endDate: Date;
}

function calcOverview(
  allRecords: IContext[],
  selectedCountry?: CountryType
): TravelOverview
```

- countriesCount: ユニーク国数
- regionsCount: selectedCountry 指定時のユニーク地域数
- totalDays: 最早日付〜最遅日付
- startDate / endDate: 全レコードの最小/最大日付

## RouteService インターフェース

### getArrivals

```typescript
interface Arrival {
  from: CountryType;
  date: Date;
  transportationType: Transportationtype;
}

function getArrivals(
  transportations: Transportation[],
  country: CountryType
): Arrival[]
```

- endCountry === country && startCountry !== country && !isNoEntry

### getDepartures

```typescript
interface Departure {
  to: CountryType;
  date: Date;
  transportationType: Transportationtype;
}

function getDepartures(
  transportations: Transportation[],
  country: CountryType
): Departure[]
```

- startCountry === country && endCountry !== country

### getRegionRoutes

```typescript
interface RegionRoute {
  region: string;
  startDate: Date;
  endDate: Date;
  routes: Transportation[];
}

function getRegionRoutes(
  transportations: Transportation[],
  country: CountryType
): RegionRoute[]
```

- 対象国内の交通記録を日付順ソート
- 地域ごとにグルーピング、滞在期間算出

### getVisitedCountries

```typescript
interface VisitedCountry {
  country: CountryType;
  isNoEntry: boolean;
}

function getVisitedCountries(
  allRecords: IContext[],
  transportations: Transportation[]
): VisitedCountry[]
```

## FilterService インターフェース

### applyFilters

```typescript
function applyFilters<T extends IContext>(
  records: T[],
  filterState: FilterState
): T[]
```

- FilterState に基づいてレコードをフィルタリング

### getCountryList

```typescript
function getCountryList(records: IContext[]): CountryType[]
```

- 全レコードからユニーク国リスト（ソート済み）

### getRegionList

```typescript
function getRegionList(
  records: IContext[],
  country: CountryType
): string[]
```

- 指定国のレコードからユニーク地域リスト（ソート済み）

## テスト仕様

#### UT-STATS-001-01: calcCostSummary
- 各カテゴリ費用の合算が正しいこと
- 通貨切替（JPY/EUR/USD）で金額が変わること
- 空リストで全0

#### UT-STATS-001-02: calcTypeSummary
- 同一タイプ複数件: count, total, max, min, avg
- 交通タイプ: 距離・時間も含む集計
- 0件: 空配列

#### UT-STATS-001-03: calcMovingSummary
- 距離合計・時間合計
- formattedTime: 1500分 → "1日 1時間 0分"

#### UT-STATS-001-04: calcOverview
- countriesCount: ユニーク国数
- totalDays: 日付差

#### UT-ROUTE-001-01: getArrivals
- 入国レコードの抽出
- NO_ENTRY 除外

#### UT-ROUTE-001-02: getDepartures
- 出国レコードの抽出

#### UT-ROUTE-001-03: getRegionRoutes
- 地域グルーピング・時系列ソート

#### UT-ROUTE-001-04: getVisitedCountries
- ユニーク国リスト + NO_ENTRYフラグ

#### UT-FILTER-001-01: applyFilters
- 各モード・各条件のフィルタリング（SP-BL-001 参照）
