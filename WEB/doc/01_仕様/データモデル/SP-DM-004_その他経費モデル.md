# SP-DM-004: その他経費モデル仕様

対応要求: FR-005

## エンティティ: OtherExpense

### フィールド定義

| フィールド名 | 型 | 必須 | 説明 |
|---|---|---|---|
| id | string (UUID) | Yes | 一意識別子 |
| otherType | OtherType | Yes | 経費タイプ |
| context | string \| null | No | 説明 |
| date | Date | Yes | 日付 |
| country | CountryType | Yes | 国 |
| region | string \| null | No | 地域 |
| price | number | Yes | 料金 |
| currency | CurrencyType | Yes | 通貨 |
| memo | string \| null | No | メモ |
| jpyPrice | number | - | 計算値 |
| eurPrice | number | - | 計算値 |
| usdPrice | number | - | 計算値 |

### OtherType列挙値

```
Insurance | Ticket | Accident | Event | Other | Shopping | Medical
| Washing | Tax | Exchange | Cashing | Haircut | Tips
| PartTimeJob | Toilet
```

### データ生成元

OtherExpense は専用CSVファイルを持たない。sightseeing.csv から sightseeingParser がパース時に振り分けて生成する。

- 生成フロー: sightseeing.csv → `parseSightseeings()` → `isOtherType()` 判定 → `otherExpenses[]`
- 詳細な振り分けロジックは SP-DM-003 の「観光/経費振り分けロジック」セクションを参照
- 為替レート適用は useTravelData hook 内で `applyExchangeRates()` により一括実行

### テスト仕様

#### UT-DM-004-01: OtherExpenseエンティティ生成
- 全必須フィールドで生成できること
- SightseeingModelからの変換コンストラクタ

#### UT-DM-004-02: OtherType変換
- 全14種の文字列→列挙値変換
