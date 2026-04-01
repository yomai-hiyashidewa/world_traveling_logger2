# ER図

## データベースER図

```mermaid
erDiagram
    USERS {
        uuid id PK
        string email
        string password_hash
        string display_name
        timestamp created_at
        timestamp updated_at
    }

    ACCOMMODATIONS {
        uuid id PK
        uuid user_id FK
        date date
        string country
        string region
        string accommodation_type
        decimal price
        string currency
        string memo
        decimal jpy_price
        decimal eur_price
        decimal usd_price
        timestamp created_at
        timestamp updated_at
    }

    TRANSPORTATIONS {
        uuid id PK
        uuid user_id FK
        date start_date
        string start_country
        string start_region
        string start_place
        date end_date
        string end_country
        string end_region
        string end_place
        string transportation_type
        decimal distance
        int time_minutes
        decimal price
        string currency
        string memo
        decimal jpy_price
        decimal eur_price
        decimal usd_price
        timestamp created_at
        timestamp updated_at
    }

    SIGHTSEEINGS {
        uuid id PK
        uuid user_id FK
        string context
        string sightseeing_type
        date date
        string country
        string region
        decimal price
        string currency
        string memo
        decimal jpy_price
        decimal eur_price
        decimal usd_price
        timestamp created_at
        timestamp updated_at
    }

    OTHER_EXPENSES {
        uuid id PK
        uuid user_id FK
        string other_type
        string context
        date date
        string country
        string region
        decimal price
        string currency
        string memo
        decimal jpy_price
        decimal eur_price
        decimal usd_price
        timestamp created_at
        timestamp updated_at
    }

    EXCHANGE_RATES {
        uuid id PK
        uuid user_id FK
        string currency
        decimal rate
        date date
        timestamp created_at
    }

    USER_SETTINGS {
        uuid id PK
        uuid user_id FK
        string display_currency
        string language
        timestamp updated_at
    }

    USERS ||--o{ ACCOMMODATIONS : has
    USERS ||--o{ TRANSPORTATIONS : has
    USERS ||--o{ SIGHTSEEINGS : has
    USERS ||--o{ OTHER_EXPENSES : has
    USERS ||--o{ EXCHANGE_RATES : has
    USERS ||--|| USER_SETTINGS : has
```

## 列挙値テーブル（参考: アプリケーションコードで定義）

| テーブル列 | 列挙型 | 格納形式 |
|---|---|---|
| country | CountryType | 文字列（"JPN", "USA", ...） |
| currency | CurrencyType | 文字列（"JPY", "EUR", ...） |
| accommodation_type | AccommodationType | 文字列（"Hotel", "Domitory", ...） |
| transportation_type | Transportationtype | 文字列（"Train", "Bus", ...） |
| sightseeing_type | SightseeigType | 文字列（"Museum", "Beach", ...） |
| other_type | OtherType | 文字列（"Insurance", "Shopping", ...） |
| start_place / end_place | PlaceType | 文字列（"Station", "Airport", ...） |

## インデックス設計

| テーブル | インデックス | 用途 |
|---|---|---|
| ACCOMMODATIONS | (user_id, date) | 日付範囲検索 |
| ACCOMMODATIONS | (user_id, country) | 国フィルタ |
| TRANSPORTATIONS | (user_id, start_date) | 日付範囲検索 |
| TRANSPORTATIONS | (user_id, start_country, end_country) | ルート分析 |
| SIGHTSEEINGS | (user_id, date) | 日付範囲検索 |
| SIGHTSEEINGS | (user_id, country) | 国フィルタ |
| OTHER_EXPENSES | (user_id, date) | 日付範囲検索 |
| EXCHANGE_RATES | (user_id, currency, date) | レート取得 |
