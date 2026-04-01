# SP-DM-003: 観光モデル仕様

対応要求: FR-004

## エンティティ: Sightseeing

### フィールド定義

| フィールド名 | 型 | 必須 | 説明 |
|---|---|---|---|
| id | string (UUID) | Yes | 一意識別子 |
| context | string | Yes | 活動名/説明 |
| sightseeingType | SightseeigType | Yes | 観光タイプ（自動分類含む） |
| date | Date | Yes | 日付 |
| country | CountryType | Yes | 国 |
| region | string \| null | No | 地域 |
| price | number | Yes | 料金 |
| currency | CurrencyType | Yes | 通貨 |
| memo | string \| null | No | メモ |
| jpyPrice | number | - | 計算値 |
| eurPrice | number | - | 計算値 |
| usdPrice | number | - | 計算値 |

### SightseeigType列挙値

#### 観光系
```
Visiting | Trekking | Walking | Eating | KickBoard | Cycring
| CableCar | Tour | Boat | HotSpring | Museum | Church
| Beach | Zoo | Heritage | Overviewing | Waterfall | Castle
| Nature | Canal | Park | Restaurant | Eatery | Stand
| Bakery | BugerShop | ChainStore | Cafe | Bar
| Food | Drink | Snack | InnBreakfast | InnDinner
| Transportfood | Tourfood | Fesfood | Supermarket | Sovnir
```

#### 経費系（OtherListへ振り分け対象）
```
Insurance | Ticket | Accident | Event | Other | Shopping
| Medical | Washing | Tax | Exchange | Cashing | Haircut
| Tips | PartTimeJob | Toilet
```

### キーワード→タイプ自動分類マッピング

`classifySightseeingByKeyword(context, csvType)` 関数（types.ts）で実行される。

**前提条件**: csvType が `Visiting` の場合のみキーワード分類を適用する。それ以外の csvType はそのまま使用。

| キーワード（大文字比較） | 分類先 | カテゴリ |
|---|---|---|
| BEACH | Beach | 観光系 |
| MUSEUM | Museum | 観光系 |
| CHURCH | Church | 観光系 |
| ZOO | Zoo | 観光系 |
| HERITAGE | Heritage | 観光系 |
| WATERFALL | Waterfall | 観光系 |
| CASTLE | Castle | 観光系 |
| PARK | Park | 観光系 |
| RESTAURANT | Restaurant | 観光系 |
| CAFE | Cafe | 観光系 |
| BAR | Bar | 観光系 |
| CANAL | Canal | 観光系 |
| INSURANCE | Insurance | **経費系** |
| TICKET | Ticket | **経費系** |
| SHOPPING | Shopping | **経費系** |
| MEDICAL | Medical | **経費系** |
| WASHING | Washing | **経費系** |
| EXCHANGE | Exchange | **経費系** |
| HAIRCUT | Haircut | **経費系** |
| SUPERMARKET | Supermarket | 観光系 |

- context文字列の先頭文字は大文字変換される（`capitalizeFirst`）
- キーワードは context を大文字変換して `includes()` で部分一致判定
- 最初にマッチしたキーワードで分類確定（先勝ち）

### 観光/経費振り分けロジック（sightseeingParser）

sightseeingParser の `parseSightseeings()` 関数内で以下の手順で振り分けを行う:

1. CSVの各行をパースし、`parseSightseeingCsvType(cols[1])` で型文字列を `SightseeingCsvType` に変換
2. `classifySightseeingByKeyword(context, rawType)` でキーワード自動分類を適用
3. `isOtherType(classifiedType)` で経費系かどうか判定:
   - **false（観光系）**: `Sightseeing` オブジェクトとして `sightseeings[]` に追加
   - **true（経費系）**: `OtherExpense` オブジェクトとして `otherExpenses[]` に追加
4. 戻り値: `{ sightseeings: Sightseeing[], otherExpenses: OtherExpense[] }`

※ OtherExpense への変換時、context はそのまま引き継がれる。otherType は classifiedType をキャストして設定。

### テスト仕様

#### UT-DM-003-01: Sightseeingエンティティ生成
- 全必須フィールドで生成できること
- context先頭文字が大文字に変換されること

#### UT-DM-003-02: キーワード自動分類
- "BEACH RESORT" → Beach
- "MUSEUM OF ART" → Museum
- "Walking around" → Walking（デフォルト）
- 各キーワードパターンの網羅テスト

#### UT-DM-003-03: 観光/経費振り分け
- type=Museum → 観光リストに残る
- type=Insurance → OtherListへ振り分け
- type=Shopping → OtherListへ振り分け
- 振り分け後のOtherModelのフィールドマッピング
