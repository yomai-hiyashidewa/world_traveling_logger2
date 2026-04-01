# システムアーキテクチャ

## Level 0: システムコンテキスト

```mermaid
graph LR
    User[ユーザー]
    CSV["CSV Files<br/>(documents/English/)"]
    GHP[GitHub Pages]

    CSV -->|リポジトリに同梱| GHP
    User -->|ブラウザアクセス| GHP
    GHP -->|SPA配信| User
    GHP -->|fetch| CSV
```

## Level 1: クライアントサイドアーキテクチャ

```mermaid
graph TB
    subgraph Browser["ブラウザ（SPA）"]
        subgraph UI["UI層（React）"]
            Components[React Components]
            Hooks[Custom Hooks]
        end
        subgraph Domain["Domain層（Pure TypeScript）"]
            Services[Services]
            Models[Models / Types]
            Parsers[CSV Parsers]
        end
    end

    subgraph Static["静的ファイル（GitHub Pages）"]
        HTML[index.html + JS bundle]
        CSVFiles["data/*.csv"]
    end

    User((User)) --> Browser
    HTML --> Browser
    Browser -->|fetch| CSVFiles
    Components --> Hooks
    Hooks --> Services
    Services --> Models
    Parsers --> Models
```

## Level 2: Domain層 詳細

```mermaid
graph TB
    subgraph Parsers["parsers/"]
        CsvReader[csvReader]
        AccParser[accommodationParser]
        TransParser[transportationParser]
        SightParser[sightseeingParser]
        ExchParser[exchangeRateParser]
    end

    subgraph Models["models/"]
        Types["types.ts<br/>(CountryType, CurrencyType<br/>AccommodationType, etc.)"]
        AccModel[accommodation.ts]
        TransModel[transportation.ts]
        SightModel[sightseeing.ts]
        OtherModel[otherExpense.ts]
        ExchModel[exchangeRate.ts]
        SummaryModel["summary.ts<br/>(CostSummary, TypeSummary<br/>MovingSummary)"]
    end

    subgraph Services["services/"]
        ExchSvc[exchangeRateService]
        FilterSvc[filterService]
        StatsSvc[statsService]
        RouteSvc[routeService]
        ImgSvc[imageService]
    end

    CsvReader --> AccParser
    CsvReader --> TransParser
    CsvReader --> SightParser
    CsvReader --> ExchParser

    AccParser --> AccModel
    TransParser --> TransModel
    SightParser --> SightModel
    SightParser --> OtherModel
    ExchParser --> ExchModel

    AccModel --> Types
    TransModel --> Types
    SightModel --> Types
    OtherModel --> Types
    ExchModel --> Types

    ExchSvc --> ExchModel
    FilterSvc --> Types
    StatsSvc --> SummaryModel
    RouteSvc --> TransModel
    ImgSvc --> Types
```

## Level 2: UI層 詳細

```mermaid
graph TB
    subgraph Pages["pages/"]
        Dashboard[Dashboard]
    end

    subgraph Components["components/"]
        Sidebar[Sidebar<br/>国・地域リスト]
        FilterPanel["FilterPanel<br/>日付・トグル・通貨"]
        TabPanel[TabPanel<br/>タブ切替]
        DataTable[DataTable<br/>データテーブル]
        StatsSummary[StatsSummary<br/>集計表示]
        RouteView[RouteView<br/>ルート表示]
    end

    subgraph Hooks["hooks/"]
        UseTravelData["useTravelData<br/>CSV読込・全データ保持"]
        UseFilterState["useFilterState<br/>フィルタ条件管理"]
        UseStats["useStats<br/>集計計算"]
    end

    subgraph DomainServices["domain/services/"]
        FilterSvc[filterService]
        StatsSvc[statsService]
        RouteSvc[routeService]
        ExchSvc[exchangeRateService]
        ImgSvc[imageService]
    end

    Dashboard --> Sidebar
    Dashboard --> FilterPanel
    Dashboard --> TabPanel
    TabPanel --> DataTable
    TabPanel --> StatsSummary
    TabPanel --> RouteView

    Sidebar --> UseFilterState
    FilterPanel --> UseFilterState
    DataTable --> UseTravelData
    StatsSummary --> UseStats

    UseTravelData -->|fetch + parse| DomainServices
    UseFilterState -->|filter| FilterSvc
    UseStats -->|calc| StatsSvc
    UseStats -->|calc| RouteSvc
    RouteView -->|国旗パス| ImgSvc
    Dashboard -->|ヘッダー画像パス| ImgSvc
```

## Level 3: データフロー

```mermaid
graph LR
    subgraph Startup["起動時"]
        Fetch["fetch CSV"] --> Parse["CSVパース"]
        Parse --> Validate["バリデーション"]
        Validate --> Convert["通貨変換"]
        Convert --> Store["React State に格納"]
    end

    subgraph Runtime["操作時"]
        UserAction["ユーザー操作"] --> FilterChange["フィルタ変更"]
        FilterChange --> Filter["filterService.apply()"]
        Filter --> Calc["statsService.calc()"]
        Calc --> Render["React 再レンダリング"]
    end

    Store --> Runtime
```

## Level 3: 元WPFとWEBの対応表

| 元WPF層 | 元クラス | WEB版対応 |
|---|---|---|
| View (XAML) | MainWindow, MainViewPanel, SideView, UpperView | React Components (Dashboard, Sidebar, FilterPanel) |
| ViewModel | MainViewPanelVM, SideViewModel, UpperViewModel | React Hooks (useTravelData, useFilterState, useStats) |
| Model/ControlModel | ControlModel | useFilterState + filterService |
| Model/MainModel | MainModel | useTravelData hook |
| Model/ContextList | AccommodationList, TransportationList, etc. | domain/services/ + React State |
| Model/ExchangeRater | ExchangeRater | domain/services/exchangeRateService |
| Model/OptionModel | OptionModel | domain/services/imageService（画像パス生成） |
| Model/Csv | CSVReader, CSVWriter | domain/parsers/ |
| Model/Base/BaseContext | BaseContext | domain/models/ (TypeScript interface/type) |
| Model/Enumeration/ | CountryType, CurrencyType, etc. | domain/models/types.ts |
| Event (FileLoaded_, CalcCompleted_) | C# events | React useEffect + 状態変更による再レンダリング |
| RaisePropertyChanged | INotifyPropertyChanged | React setState |
| DelegateCommand | ICommand | onClick/onChange イベントハンドラ |
| DataGrid (WPF) | System.Windows.Controls.DataGrid | DataTable コンポーネント (HTML table) |
