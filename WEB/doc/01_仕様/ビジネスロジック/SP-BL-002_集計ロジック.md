# SP-BL-002: 集計ロジック仕様

対応要求: FR-008

## 概要

フィルタ適用済みデータに対する各種集計ロジック。

## カテゴリ別費用集計

### 入力
- フィルタ済み各カテゴリのレコードリスト
- 表示通貨（MajorCurrencyType）

### 処理
1. 各カテゴリのレコードから表示通貨の金額を合算
2. 4カテゴリの合計を算出

### 出力: CostSummary

## タイプ別集計

### 入力
- フィルタ済みカテゴリのレコードリスト
- カテゴリ種別（accommodation/transportation/sightseeing/other）

### 処理
1. レコードをタイプごとにグルーピング
2. 各グループについて:
   - count: レコード数
   - totalCost: 表示通貨金額の合計
   - maxCost: 最大値
   - minCost: 最小値（初回はMAX_VALUEで初期化）
   - averageCost: totalCost / count
3. transportation の場合、追加で距離・時間の同様の集計

### 出力: TypeSummary[] | TransportationTypeSummary[]

## 旅行概要集計

### 入力
- フィルタ済み全カテゴリのレコードリスト

### 処理
- 訪問国数: 全レコードのcountryをユニーク化してカウント
- 訪問地域数: 選択国内のregionをユニーク化してカウント
- 旅行日数: 最早日付〜最遅日付の日数差
- 開始日/終了日: 全レコードの日付の最小/最大

### 出力
```
{ countriesCount, regionsCount, totalDays, startDate, endDate }
```

## テスト仕様

#### UT-BL-002-01: カテゴリ別費用集計
- 宿泊3件(1000,2000,3000)のJPY合計 → 6000
- 4カテゴリ合算のtotal
- 通貨切替時の再計算（JPY→EUR）

#### UT-BL-002-02: タイプ別集計
- 同一タイプ3件: count=3, total=合計, max=最大, min=最小, avg=total/3
- 異なるタイプ混在: タイプ数分のグループ
- 0件タイプは結果に含まれないこと

#### UT-BL-002-03: 交通タイプ別集計（距離・時間）
- distance: [100, 200, 300] → total=600, max=300, min=100, avg=200
- time: [60, 120, 180] → total=360, max=180, min=60, avg=120

#### UT-BL-002-04: 旅行概要
- 3国のデータ → countriesCount=3
- 同一国で5地域 → regionsCount=5
- 2023-01-01〜2023-12-31 → totalDays=365
