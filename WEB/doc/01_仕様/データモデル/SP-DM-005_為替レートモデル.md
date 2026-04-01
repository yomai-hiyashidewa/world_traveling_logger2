# SP-DM-005: 為替レートモデル仕様

対応要求: FR-006

## エンティティ: ExchangeRate

### フィールド定義

| フィールド名 | 型 | 必須 | 説明 |
|---|---|---|---|
| id | string (UUID) | Yes | 一意識別子 |
| currency | CurrencyType | Yes | 通貨コード |
| rate | number | Yes | レート（対基準通貨） |
| date | Date | Yes | 適用日 |

## サービス: ExchangeRater

### メソッド仕様

#### getRate(currency: CurrencyType, date: Date): number
- 指定通貨・日付のレートを返す
- 該当日のレートがない場合は最近日のレートを使用

#### getAverageRate(currency: CurrencyType, startDate: Date, endDate: Date): number
- 指定期間の平均レートを返す

#### convertPrice(price: number, currency: CurrencyType, date: Date): { jpy: number, eur: number, usd: number }
- 指定金額をJPY/EUR/USDに変換

### CurrencyType列挙値（主要）

```
JPY | EUR | USD | AUD | CAD | GBP | CHF | CNY | KRW | INR
| THB | VND | IDR | MYR | SGD | PHP | TWD | HKD | NZD
| SEK | NOK | DKK | CZK | PLN | HUF | RON | BGN | HRK
| TRY | RUB | UAH | EGP | ZAR | MAD | TND | KES | TZS
| GHS | NGN | BRL | ARS | CLP | COP | PEN | MXN | BOB | UYU
```

### MajorCurrencyType

```
JPN | USD | EUR
```

### テスト仕様

#### UT-DM-005-01: ExchangeRate生成
- 通貨・レート・日付を指定して生成

#### UT-DM-005-02: getRate
- 存在する日付・通貨のレート取得
- 存在しない日付の近似レート取得

#### UT-DM-005-03: getAverageRate
- 期間内の平均レート算出
- 期間内にデータがない場合のハンドリング

#### UT-DM-005-04: convertPrice
- 100 EUR → JPY変換（レート150の場合→15000）
- 100 EUR → USD変換
- JPY→JPY変換（レート=1）
