# 状態遷移図

## 表示モード状態遷移

```mermaid
stateDiagram-v2
    [*] --> WorldMode : 初期表示

    WorldMode --> CountryMode : 国を選択
    CountryMode --> RegionMode : 地域を選択
    CountryMode --> WorldMode : ワールドに戻る
    RegionMode --> CountryMode : 国レベルに戻る
    RegionMode --> WorldMode : ワールドに戻る

    state WorldMode {
        [*] --> WithJapan
        WithJapan --> WithoutJapan : 日本除外ON
        WithoutJapan --> WithJapan : 日本除外OFF
    }

    state CountryMode {
        [*] --> CountrySelected
        CountrySelected --> CountrySelected : 別の国を選択
    }

    state RegionMode {
        [*] --> RegionSelected
        RegionSelected --> RegionSelected : 別の地域を選択
    }
```

## アプリケーション初期化状態

```mermaid
stateDiagram-v2
    [*] --> Idle : アプリ起動

    Idle --> Loading : データ読み込み開始
    Loading --> RatesLoaded : 為替レート読み込み完了
    RatesLoaded --> Converting : 通貨変換中
    Converting --> DataReady : 全データ読み込み・変換完了
    DataReady --> Calculating : 初回集計
    Calculating --> Ready : 表示準備完了

    Loading --> Error : 読み込みエラー
    Error --> Loading : リトライ

    Ready --> Filtering : フィルタ変更
    Filtering --> Calculating : 再集計
    Calculating --> Ready : 計算完了
```

## CSVインポート状態

```mermaid
stateDiagram-v2
    [*] --> FileSelect : インポート画面表示

    FileSelect --> Uploading : ファイル選択・送信
    Uploading --> Parsing : アップロード完了
    Parsing --> Validating : パース完了
    Validating --> ValidationResult : バリデーション完了

    state ValidationResult {
        [*] --> HasErrors : エラーあり
        [*] --> AllValid : 全行正常
    }

    ValidationResult --> Importing : ユーザー確認・実行
    Importing --> ImportComplete : インポート完了
    ImportComplete --> [*]

    Uploading --> Error : アップロードエラー
    Parsing --> Error : パースエラー
    Error --> FileSelect : 再試行
```

## useTravelData 状態遷移

```mermaid
stateDiagram-v2
    [*] --> Idle : hook マウント

    Idle --> Fetching : useEffect 発火
    Fetching --> Parsing : 4ファイル fetch 完了
    Parsing --> Converting : パース完了
    Converting --> Ready : applyExchangeRates 完了

    Fetching --> Error : fetch 失敗
    Parsing --> Error : パースエラー

    state Idle {
        loading = true
        error = null
    }

    state Ready {
        loading = false
        error = null
    }

    state Error {
        loading = false
        error = メッセージ
    }
```

## useFilterState 状態遷移

```mermaid
stateDiagram-v2
    [*] --> NoFilter : 初期状態

    NoFilter --> CountrySelected : setCountry(国)
    CountrySelected --> RegionSelected : setRegion(地域)
    CountrySelected --> NoFilter : setCountry(null)
    RegionSelected --> CountrySelected : setRegion(null)
    RegionSelected --> NoFilter : clearFilters

    CountrySelected --> CountrySelected : setCountry(別の国)\n※ region は null にリセット
    CountrySelected --> NoFilter : clearFilters

    state NoFilter {
        country = null
        region = null
        ---
        ワールドモード
    }

    state CountrySelected {
        country = 値あり
        region_ = null
        ---
        カントリーモード
    }

    state RegionSelected {
        country_ = 値あり
        region = 値あり
        ---
        リージョンモード
    }

    note right of NoFilter
        各トグル・日付変更は
        モード遷移に影響しない
        （フィルタ済みデータのみ変化）
    end note
```

## useStats 再計算トリガー

```mermaid
stateDiagram-v2
    [*] --> Computed : 初回計算

    Computed --> Recomputing : 依存変更

    state Recomputing {
        [*] --> CheckDeps
        CheckDeps --> CalcCost : useMemo 再評価
        CalcCost --> CalcType
        CalcType --> CalcMoving
        CalcMoving --> CalcRoute
        CalcRoute --> Done
    }

    Recomputing --> Computed : 計算完了

    note right of Computed
        依存: accommodations, transportations,
        sightseeings, otherExpenses,
        currency, selectedCountry
    end note
```

## タブ切替状態

```mermaid
stateDiagram-v2
    [*] --> AccommodationTab : 初期表示

    AccommodationTab --> TransportationTab : 交通タブ選択
    AccommodationTab --> SightseeingTab : 観光タブ選択
    AccommodationTab --> OtherTab : その他タブ選択
    AccommodationTab --> RouteTab : ルートタブ選択

    TransportationTab --> AccommodationTab : 宿泊タブ選択
    TransportationTab --> SightseeingTab : 観光タブ選択
    TransportationTab --> OtherTab : その他タブ選択
    TransportationTab --> RouteTab : ルートタブ選択

    SightseeingTab --> AccommodationTab : 宿泊タブ選択
    SightseeingTab --> TransportationTab : 交通タブ選択
    SightseeingTab --> OtherTab : その他タブ選択
    SightseeingTab --> RouteTab : ルートタブ選択

    OtherTab --> AccommodationTab : 宿泊タブ選択
    OtherTab --> TransportationTab : 交通タブ選択
    OtherTab --> SightseeingTab : 観光タブ選択
    OtherTab --> RouteTab : ルートタブ選択

    RouteTab --> AccommodationTab : 宿泊タブ選択
    RouteTab --> TransportationTab : 交通タブ選択
    RouteTab --> SightseeingTab : 観光タブ選択
    RouteTab --> OtherTab : その他タブ選択
```
