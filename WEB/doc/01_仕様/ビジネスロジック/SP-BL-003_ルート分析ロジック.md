# SP-BL-003: ルート分析ロジック仕様

対応要求: FR-009

## 概要

交通記録データからルート情報（入国・出国・国内地域間移動）を分析する。
元コードの TransportationList のクエリメソッドに対応する。

## 国内移動経路取得（GetRoute）

### 入力
- 全交通記録（Transportation[]）
- 対象国（CountryType）

### 処理
```
route = transportations.filter(t =>
  t.startCountry === targetCountry || t.endCountry === targetCountry
).sort(t => t.startDate)
```

- 対象国に「関わる」全交通記録を取得（出発国 OR 到着国が一致）
- 国内移動だけでなく、入国・出国のレコードも含む
- 元コード: TransportationList.GetRoute()

### 出力
```
Transportation[]  // 時系列ソート済み
```

### 各レコードの分類

| 条件 | 分類 | 表示方法 |
|---|---|---|
| startCountry === endCountry | 国内移動 | 出発地域→交通手段(距離/時間)→到着地域 |
| endCountry === targetCountry && startCountry !== targetCountry | 入国 | 入国日・入国地域 |
| startCountry === targetCountry && endCountry !== targetCountry | 出国 | 出国日・出国地域 |

## 入国・出国分析

### 入力
- 全交通記録（Transportation[]）
- 対象国（CountryType）

### 処理: 入国（Arrival）抽出
```
arrivals = transportations.filter(t =>
  t.endCountry === targetCountry &&
  t.startCountry !== targetCountry &&
  !t.isNoEntry
)
```

### 処理: 出国（Departure）抽出
```
departures = transportations.filter(t =>
  t.startCountry === targetCountry &&
  t.endCountry !== targetCountry
)
```

### 出力
```
{
  arrivals: [{ fromCountry: CountryType, date: Date, region: string, type: TransportationType, distance: number, time: number }],
  departures: [{ toCountry: CountryType, date: Date, region: string, type: TransportationType, distance: number, time: number }]
}
```

## 距離・時間の表示フォーマット

対応要求: FR-009-02
元コード: MovingModel

### 距離フォーマット
```
formatDistance(km: number): string
  → "{#,0}km"  // 3桁区切り + "km"
  // 例: 1234 → "1,234km", 50 → "50km"
```

### 時間フォーマット
```
formatTime(minutes: number): string
  days = Math.floor(minutes / (24 * 60))
  hours = Math.floor((minutes % (24 * 60)) / 60)
  min = minutes % 60

  if (days > 0) → "{days}d {hours}h {min}m"    // 例: "1d 2h 30m"
  else if (hours > 0) → "{hours}h {min}m"       // 例: "2h 30m"
  else → "{min}min"                              // 例: "30min"
```

### 使用箇所
- 左右パネル（入国元・出国先）の詳細表示: 距離と時間を個別に表示
- 中央パネル国内移動: "距離,時間" の結合文字列（DistanceAndTimeStr）

## 国滞在期間算出

### 処理
```
routes = getRoute(targetCountry)
startDate = routes[0].endDate       // 最初のレコードの到着日
endDate = routes[last].startDate    // 最後のレコードの出発日
```

元コード: RouteRegionViewModel.StartDate, EndDate

## NO_ENTRY判定

- memo に "NO_ENTRY" を含むレコードは入国（Arrival）から除外
- 訪問国リストには含めるがマーク付き
- 国内移動経路（GetRoute）では除外しない（経由表示として含める）

## 訪問国リスト生成

### 処理
1. 全カテゴリレコードのcountryをユニーク化
2. 交通記録のstartCountry, endCountryも含める
3. NO_ENTRY国はフラグ付きでリストに含める

## テスト仕様

#### UT-BL-003-01: GetRoute（国内移動経路取得）
- country=FRA の場合、startCountry=FRA OR endCountry=FRA の全レコードが返ること
- 結果が startDate で時系列ソートされていること
- 国内移動レコード（SameCountry=true）が含まれること
- 入国レコード（endCountry=FRA, startCountry≠FRA）が含まれること
- 出国レコード（startCountry=FRA, endCountry≠FRA）が含まれること

#### UT-BL-003-02: 入国判定
- start=ESP, end=FRA → FRAへの入国
- start=FRA, end=FRA → 入国ではない（国内移動）
- NO_ENTRYレコード → 入国から除外

#### UT-BL-003-03: 出国判定
- start=FRA, end=DEU → FRAからの出国
- start=FRA, end=FRA → 出国ではない

#### UT-BL-003-04: 国内移動レコード分類
- startCountry=FRA, endCountry=FRA → SameCountry=true、国内移動として表示
- startCountry=ESP, endCountry=FRA → 入国として表示
- startCountry=FRA, endCountry=DEU → 出国として表示

#### UT-BL-003-05: 国滞在期間
- 3件のレコードがある場合、最初のendDateと最後のstartDateが返ること
- 1件のみの場合も正しく返ること

#### UT-BL-003-06: NO_ENTRY
- memo="NO_ENTRY transit" → isNoEntry=true
- 入国リストから除外されること
- GetRoute結果には含まれること（経由表示用）
- 訪問国リストにはNO_ENTRYフラグ付きで含まれること

#### UT-BL-003-07: 訪問国リスト
- 重複排除された国リスト
- 宿泊・観光のcountryも含まれること
- ソート順の一貫性

#### UT-BL-003-08: 距離フォーマット
- 1234 → "1,234km"
- 50 → "50km"
- 0 → "0km"

#### UT-BL-003-09: 時間フォーマット
- 1590 (26h30m) → "1d 2h 30m"
- 150 (2h30m) → "2h 30m"
- 30 → "30min"
- 0 → "0min"
