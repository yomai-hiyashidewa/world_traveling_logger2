# SP-DM-006: 集計モデル仕様

対応要求: FR-008

## 値オブジェクト: CostSummary

### フィールド定義

| フィールド名 | 型 | 説明 |
|---|---|---|
| accommodation | number | 宿泊費合計 |
| transportation | number | 交通費合計 |
| sightseeing | number | 観光費合計 |
| other | number | その他経費合計 |
| total | number | 総合計（4カテゴリ合算） |
| currency | MajorCurrencyType | 表示通貨 |

## 値オブジェクト: TypeSummary

### フィールド定義

| フィールド名 | 型 | 説明 |
|---|---|---|
| type | string | タイプ名 |
| count | number | 件数 |
| totalCost | number | 合計費用 |
| maxCost | number | 最大費用 |
| minCost | number | 最小費用 |
| averageCost | number | 平均費用（totalCost / count） |

## 値オブジェクト: TransportationTypeSummary extends TypeSummary

### 追加フィールド

| フィールド名 | 型 | 説明 |
|---|---|---|
| totalDistance | number | 合計距離(km) |
| maxDistance | number | 最大距離 |
| minDistance | number | 最小距離 |
| averageDistance | number | 平均距離 |
| totalTime | number | 合計時間(分) |
| maxTime | number | 最大時間 |
| minTime | number | 最小時間 |
| averageTime | number | 平均時間 |

## 値オブジェクト: MovingSummary

### フィールド定義

| フィールド名 | 型 | 説明 |
|---|---|---|
| totalDistance | number | 総移動距離(km) |
| totalTime | number | 総移動時間(分) |
| formattedDistance | string | "X,XXX km" |
| formattedTime | string | "X日 X時間 X分" |

## 通貨フォーマット仕様

| 通貨 | フォーマット | 例 |
|---|---|---|
| JPY | ¥{amount} 小数なし | ¥15,000 |
| EUR | €{amount} 小数2桁 | €120.50 |
| USD | ${amount} 小数2桁 | $135.00 |

### テスト仕様

#### UT-DM-006-01: CostSummary計算
- 4カテゴリの合計からtotalが正しく算出されること
- 各通貨でのフォーマット表示

#### UT-DM-006-02: TypeSummary集計
- 3件のデータ → count=3, total/max/min/avgが正しいこと
- 1件のみ → max=min=total=avg
- 0件 → count=0, 費用=0

#### UT-DM-006-03: TransportationTypeSummary
- 距離・時間の集計が費用集計に加えて正しいこと

#### UT-DM-006-04: MovingSummary
- 時間フォーマット: 90分 → "1時間 30分"
- 時間フォーマット: 1500分 → "1日 1時間 0分"
- 距離フォーマット: 12345.6 → "12,346 km"
