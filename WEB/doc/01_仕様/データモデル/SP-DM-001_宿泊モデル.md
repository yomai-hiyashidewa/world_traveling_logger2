# SP-DM-001: 宿泊モデル仕様

対応要求: FR-002

## エンティティ: Accommodation

### フィールド定義

| フィールド名 | 型 | 必須 | 説明 |
|---|---|---|---|
| id | string (UUID) | Yes | 一意識別子（WEB版追加） |
| date | Date | Yes | 宿泊日 |
| country | CountryType | Yes | 国コード（ISO 3166-1準拠） |
| region | string \| null | No | 地域名 |
| accommodationType | AccommodationType | Yes | 宿泊タイプ |
| price | number | Yes | 料金（元通貨） |
| currency | CurrencyType | Yes | 元通貨 |
| memo | string \| null | No | メモ |
| jpyPrice | number | - | 計算値: JPY換算額 |
| eurPrice | number | - | 計算値: EUR換算額 |
| usdPrice | number | - | 計算値: USD換算額 |

### AccommodationType列挙値

```
ParentsHouse | Domitory | SingleRoom | Airplane | Bus | Ferry
| Train | Airport | FriendHouse | Hotel
```

### CSV入力フォーマット

```
date, country, region, accommodation_type, price, currency, memo
```

- date: yyyy/MM/dd 形式
- country: CountryType文字列（大文字）
- accommodation_type: AccommodationType文字列

### 国名表示

- UI上のCountryカラムには国コードではなく `countryName(country)` で変換した国名を表示する
- 変換マッピングは元WPFアプリの CountryType.cs `[Display(Name=...)]` 属性と同一
- マッピングに存在しない国コードの場合はコード文字列をそのまま表示する

### テスト仕様

#### UT-DM-001-01: Accommodationエンティティ生成
- 全必須フィールドを指定して生成できること
- region, memoがnullでも生成できること
- dateが不正形式の場合エラーとなること
- countryが未知の国コードの場合エラーとなること

#### UT-DM-001-02: AccommodationType変換
- 文字列"ParentsHouse"→AccommodationType.ParentsHouse
- 全10種の変換が正しいこと
- 未知文字列の場合エラーとなること

#### UT-DM-001-03: 通貨変換
- ExchangeRaterを使用してprice→jpyPrice/eurPrice/usdPriceが計算されること
- 為替レートが存在しない場合のハンドリング
