# SP-BL-001: フィルタリングロジック仕様

対応要求: FR-007

## 概要

ControlModelに相当するフィルタリングロジック。全カテゴリのデータに対して共通のフィルタ条件を適用する。

## フィルタ条件

### 表示モード

| モード | フィルタ動作 |
|---|---|
| World | 全国のデータを表示。excludeJapan=true時はJPN除外 |
| Country | selectedCountryに一致するデータのみ表示 |
| Region | selectedCountryかつselectedRegionに一致するデータのみ表示 |

### 日付範囲フィルタ

```
record.date >= startDate AND record.date <= endDate
```

- startDate/endDateがnullの場合はフィルタなし（全期間）

### 交通フィルタ

| フィルタ | 適用対象 | 動作 | 適用モード |
|---|---|---|---|
| excludeAirplane | 交通記録 | transportationType=AirPlane を除外 | 全モード |
| excludeCrossBorder | 交通記録 | startCountry !== endCountry を除外 | **カントリー/リージョンモードのみ** |

#### excludeCrossBorder の詳細動作（元コード: ControlModel.CheckWithCrossBorder）

- ワールドモード時（country=null）: フィルタ**無効**（UIにチェックボックスを表示しない）
- カントリーモード時（country≠null）: フィルタ有効
  - ON: startCountry ≠ endCountry のレコードを除外
  - OFF: 国境越えレコードも含めて表示
- 国選択時に、選択国に片方でも関わる交通レコード（startCountry=選択国 OR endCountry=選択国）のみが対象

### 経費フィルタ

| フィルタ | 適用対象 | 動作 | 適用モード |
|---|---|---|---|
| excludeInsurance | その他経費 | otherType=Insurance を除外 | 全モード |

## フィルタ適用順序

1. カテゴリ固有の除外フィルタ（飛行機/保険/国境越え）
2. 表示モードフィルタ（国・地域）
3. 日付範囲フィルタ
4. フィルタ済みリストで再計算実行

## 状態管理

```typescript
FilterCriteria {
  country: CountryType | null       // null = ワールドモード
  region: string | null             // null = 国全体
  startDate: Date | null            // null = 制限なし
  endDate: Date | null              // null = 制限なし
  excludeAirplane: boolean          // true = AirPlane除外
  excludeInsurance: boolean         // true = Insurance除外
  excludeCrossBorder: boolean       // true = 国境越え除外（カントリーモード時のみ有効）
  excludeJapan: boolean             // true = JPN除外（ワールドモード時のみ有効）
}
```

### 初期値

| フィールド | 初期値 | 備考 |
|---|---|---|
| country | null | ワールドモード |
| region | null | 全地域 |
| startDate | null | 全期間 |
| endDate | null | 全期間 |
| excludeAirplane | false | 飛行機含む（元コード: isWithAirplane_=true） |
| excludeInsurance | false | 保険含む（元コード: isWithInsurance_=true） |
| excludeCrossBorder | false | 国境越え含む（元コード: isWithCrossBorder_=true） |
| excludeJapan | false | 日本含む（元コード: isWithJapan_=true） |

### Clearボタンの動作

全フィールドを初期値にリセットする。

## 国・地域リスト生成

- 全データから訪問済み国リストを生成（重複排除・ソート）
- 国名は `countryName()` で表示名に変換（例: "JPN" → "Japan"）
- 選択国のデータから地域リストを生成（重複排除・ソート）

## 元WPFコードとの対応

| 元ControlModelプロパティ | WEB版 FilterCriteria | 備考 |
|---|---|---|
| IsWorldMode | country === null | 国未選択=ワールドモード |
| currentCountryType_ | country | 選択国 |
| IsCountryRegion + currentRegion_ | region | 選択地域 |
| IsWithAirplane | !excludeAirplane | 論理が反転（元: 含む=true、WEB: 除外=true） |
| IsWithInsurance | !excludeInsurance | 論理が反転 |
| IsWithCrossBorder | !excludeCrossBorder | 論理が反転。カントリーモード時のみ有効 |
| IsWithJapan | !excludeJapan | 論理が反転。ワールドモード時のみ有効 |

## テスト仕様

#### UT-BL-001-01: 表示モードフィルタ
- World: 全データが返ること
- World + excludeJapan: JPN以外のデータが返ること
- Country(FRA): country=FRAのデータのみ返ること
- Region(FRA, Paris): country=FRA, region=Parisのデータのみ返ること

#### UT-BL-001-02: 日付範囲フィルタ
- startDate=2023-03-01, endDate=2023-03-31 → 3月のデータのみ
- startDate=2023-03-01, endDate=null → 3月1日以降の全データ
- startDate=null, endDate=null → 全データ
- 境界値: startDate当日のレコードが含まれること

#### UT-BL-001-03: 飛行機除外フィルタ
- excludeAirplane=true → transportationType=AirPlane のレコードが除外されること
- excludeAirplane=false → AirPlane を含む全レコードが返ること
- AirPlane 除外は宿泊・観光・その他経費には影響しないこと

#### UT-BL-001-04: 国境越え除外フィルタ
- country=null（ワールド）+ excludeCrossBorder=true → **除外されない**（ワールドモードでは無効）
- country=FRA + excludeCrossBorder=true → startCountry ≠ endCountry のレコードが除外されること
- country=FRA + excludeCrossBorder=false → 国境越えレコードも含まれること

#### UT-BL-001-05: 保険除外フィルタ
- excludeInsurance=true → otherType=Insurance のレコードが除外されること
- excludeInsurance=false → Insurance を含む全レコードが返ること
- Insurance 除外は宿泊・交通・観光には影響しないこと

#### UT-BL-001-06: 複合フィルタ
- Country(FRA) + 日付範囲 + excludeAirplane + excludeCrossBorder の組み合わせ
- 全トグルON + 国選択の状態で正しくフィルタされること

#### UT-BL-001-07: フィルタクリア
- 各種フィルタ設定後にClearで全て初期値に戻ること

#### UT-BL-001-08: 国・地域リスト生成
- 3国のデータ → 3国のリスト（ソート済み）
- 特定国の3地域データ → 3地域リスト（ソート済み）
- 重複国の排除
