# SP-DM-002: 交通モデル仕様

対応要求: FR-003

## エンティティ: Transportation

### フィールド定義

| フィールド名 | 型 | 必須 | 説明 |
|---|---|---|---|
| id | string (UUID) | Yes | 一意識別子（WEB版追加） |
| startDate | Date | Yes | 出発日 |
| startCountry | CountryType | Yes | 出発国 |
| startRegion | string \| null | No | 出発地域 |
| startPlace | PlaceType | Yes | 出発場所タイプ |
| endDate | Date | Yes | 到着日 |
| endCountry | CountryType | Yes | 到着国 |
| endRegion | string \| null | No | 到着地域 |
| endPlace | PlaceType | Yes | 到着場所タイプ |
| transportationType | Transportationtype | Yes | 交通手段 |
| distance | number | Yes | 距離(km) |
| time | number | Yes | 所要時間(分) |
| price | number | Yes | 料金 |
| currency | CurrencyType | Yes | 通貨 |
| memo | string \| null | No | メモ |
| jpyPrice | number | - | 計算値 |
| eurPrice | number | - | 計算値 |
| usdPrice | number | - | 計算値 |

### Transportationtype列挙値

```
Train | Bus | AirPlane | Ferry | Subway | Taxi | UBER | UBERMoto
| Car | Tram | Bike | MotorCycle | Tuktuk | BycleTaxi | Boat
| Ropeway | Cesna | Track | Geepny | Walking
| LocalBus | MiddleDistanceBus | LongDistanceBus
| LocalTrain | MiddleDistanceTrain | LongDistanceTrain
```

### PlaceType列挙値

```
Station | Terminal | Inn | AirPort | Port | Central | Stop
| Beach | Suberb | Museum | Park | Heritage | Border
| Hospital | Lake | Castle | Catedral | Palace | Church | Dep
```

### 距離ベース自動分類ロジック

元コードの`ResetType()`に基づく:
- Train → distance閾値で LocalTrain / MiddleDistanceTrain / LongDistanceTrain
- Bus → distance閾値で LocalBus / MiddleDistanceBus / LongDistanceBus

### 派生プロパティ

| プロパティ | 計算ロジック |
|---|---|
| isCrossBorder | startCountry !== endCountry |
| isDeparture(country) | startCountry === country && endCountry !== country |
| isArrival(country) | endCountry === country && startCountry !== country |
| isNoEntry | memo contains "NO_ENTRY" |
| isSameDate | startDate === endDate |

### CSV入力フォーマット

```
start_date, start_country, start_region, start_place,
end_date, end_country, end_region, end_place,
transportation_type, distance, time, price, currency, memo
```

### テスト仕様

#### UT-DM-002-01: Transportationエンティティ生成
- 全必須フィールドを指定して生成できること
- 出発・到着の全フィールドが独立して保持されること

#### UT-DM-002-02: 距離ベース自動分類
- Train + 短距離 → LocalTrain
- Train + 中距離 → MiddleDistanceTrain
- Train + 長距離 → LongDistanceTrain
- Bus + 短距離 → LocalBus（同上パターン）
- 鉄道・バス以外は再分類されないこと

#### UT-DM-002-03: 国境越え判定
- startCountry=JPN, endCountry=KOR → isCrossBorder=true
- startCountry=JPN, endCountry=JPN → isCrossBorder=false

#### UT-DM-002-04: 出国・入国判定
- isDeparture(JPN): start=JPN, end=KOR → true
- isDeparture(JPN): start=KOR, end=JPN → false
- isArrival(JPN): start=KOR, end=JPN → true

#### UT-DM-002-05: NO_ENTRY判定
- memo="NO_ENTRY transit" → isNoEntry=true
- memo="Normal trip" → isNoEntry=false
- memo=null → isNoEntry=false

#### UT-DM-002-06: MovingModel集計
- 複数レコードのdistance合算が正しいこと
- time合算と日時分フォーマット変換
