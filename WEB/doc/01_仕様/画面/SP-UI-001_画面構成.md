# SP-UI-001: 画面構成仕様

対応要求: FR-002〜FR-011, NFR-003

## 画面構成（元WPFアプリのMainWindow構成を踏襲）

```
+----------------------------------------------------------+
| ヘッダー（アプリタイトル、設定、ログイン）                |
+----------+-----------------------------------------------+
| サイド   | メインコンテンツ                               |
| バー     |                                               |
|          | +-------------------------------------------+ |
| - 国     | | アッパーパネル（フィルタコントロール）      | |
|   リスト | +-------------------------------------------+ |
|          | | タブパネル                                 | |
| - 地域   | |  [宿泊] [交通] [観光] [その他] [ルート]   | |
|   リスト | |                                           | |
|          | |  +---------------------------------------+| |
| - 概要   | |  | データテーブル / 集計表示              || |
|   統計   | |  |                                       || |
|          | |  +---------------------------------------+| |
+----------+-----------------------------------------------+
```

## 画面一覧

### P-001: ダッシュボード（メイン画面）

元: MainViewPanel + SideView + UpperView

| 要素 | 説明 | 対応要求 |
|---|---|---|
| ヘッダー | アプリタイトル、件数サマリ、国別メイン写真 | FR-011-02 |
| アッパーパネル | フィルタコントロール | FR-007 |
| タブパネル | カテゴリ別データ表示 | FR-002〜005, FR-009 |

### P-002: 宿泊タブ

元: AccommodationView

| 要素 | 説明 |
|---|---|
| データテーブル | 宿泊記録一覧（日付、国、地域、タイプ、料金） |
| タイプ集計テーブル | AccommodationType別の統計 |
| 費用合計表示 | 選択通貨での合計 |

### P-003: 交通タブ

元: TransportationView

| 要素 | 説明 |
|---|---|
| データテーブル | 交通記録一覧（出発、到着、タイプ、距離、時間、料金） |
| タイプ集計テーブル | Transportationtype別の統計（費用+距離+時間） |
| 移動サマリ | 合計距離・合計時間 |

### P-004: 観光タブ

元: SightseeingView

| 要素 | 説明 |
|---|---|
| データテーブル | 観光記録一覧（コンテキスト、タイプ、日付、国、料金） |
| タイプ集計テーブル | SightseeigType別の統計 |

### P-005: その他経費タブ

| 要素 | 説明 |
|---|---|
| データテーブル | その他経費一覧 |
| タイプ集計テーブル | OtherType別の統計 |

### P-005b: 集計タブ（Summary）

元: MainViewPanel の集計表示部分

StatsSummary コンポーネントで表示する。フィルタ済みデータに対する集計結果を表示。

| セクション | 表示内容 | データソース |
|---|---|---|
| Cost Summary | 宿泊・交通・観光・その他・合計の費用（選択通貨） | `stats.costSummary` |
| Moving Summary | 合計距離（km）、合計時間（フォーマット済）、訪問国数 | `stats.movingSummary`, `stats.countryCount` |
| By Accommodation Type | タイプ別: 件数, 合計費用, 平均費用 | `stats.accommodationTypeSummary` |
| By Transportation Type | タイプ別: 件数, 合計費用, 合計距離, 合計時間 | `stats.transportationTypeSummary` |
| By Sightseeing Type | タイプ別: 件数, 合計費用, 平均費用 | `stats.sightseeingTypeSummary` |
| By Other Expense Type | タイプ別: 件数, 合計費用, 平均費用 | `stats.otherTypeSummary` |

### P-006: ルートタブ

元: RouteView（3パネル構成）

#### ワールドモード時（国未選択）

| 要素 | 説明 |
|---|---|
| 国境越え一覧 | 全世界の国境越え移動を時系列表示。国旗+国名付き（FR-009-05） |

#### カントリーモード時（国選択時） — 3パネル構成

```
+------------------+---------------------------+------------------+
| 入国元一覧       | 国内移動経路              | 出国先一覧       |
| (左パネル)       | (中央パネル)              | (右パネル)       |
|                  |                           |                  |
| 国旗+国名リスト  | 滞在期間ヘッダー          | 国旗+国名リスト  |
| →選択で詳細表示  | (国旗+国名+開始日〜終了日)|→選択で詳細表示   |
|                  |                           |                  |
| 地域             | 移動経路リスト:           | 地域             |
| 交通手段         | ・国内: 地域→交通→地域   | 交通手段         |
| 距離(#,0km形式)  | ・入国: 入国日+地域       | 距離(#,0km形式)  |
| 時間(Xd Yh Zm)  | ・出国: 出国日+地域       | 時間(Xd Yh Zm)  |
| zzz(日またぎ時)  |                           | zzz(日またぎ時)  |
| ※最初の国を      |                           | ※最初の国を      |
|  自動選択        |                           |  自動選択        |
+------------------+---------------------------+------------------+
```

| パネル | 要素 | 説明 | 元コード |
|---|---|---|---|
| 左 | 入国元一覧 | 選択国への入国元の国リスト+国旗。**初期表示時は最初の国を自動選択**し詳細表示 | RouteCountryViewModel(isArrival=true) |
| 中央上 | 滞在期間 | 国旗+国名+滞在開始日〜終了日 | RouteRegionViewModel |
| 中央下 | 国内移動経路 | 選択国内の全移動を時系列表示（FR-009-03） | RouteRegionMiniViewModel |
| 右 | 出国先一覧 | 選択国からの出国先の国リスト+国旗。**初期表示時は最初の国を自動選択**し詳細表示 | RouteCountryViewModel(isArrival=false) |

#### 国内移動経路の各レコード表示（FR-009-03）

| レコード種別 | 判定条件 | 表示内容 | 配置 | ボーダー |
|---|---|---|---|---|
| 国内移動 | startCountry === endCountry | 出発地域/場所 → 交通手段（距離km, 時間min） → 到着場所/地域 | **中央寄せ** | 左・グレー |
| 入国 | endCountry === 選択国 && startCountry ≠ 選択国 | 入国日、入国地域 | 左寄せ | 左・緑(#22c55e) |
| 出国 | startCountry === 選択国 && endCountry ≠ 選択国 | 出国日、出国地域 | **右寄せ** | **右・緑(#22c55e)** |
| 日またぎ | startDate ≠ endDate | "zzz" マーカー表示（元コード準拠） |

### P-007: CSVインポート画面

元: OptionModelの設定 + ファイル読み込み

| 要素 | 説明 |
|---|---|
| ファイルアップロード | 各CSV種別のアップロード |
| バリデーション結果 | エラー行の表示 |
| インポート確認 | 取り込み件数の確認・実行 |

### P-008: 設定画面

| 要素 | 説明 |
|---|---|
| 表示通貨設定 | JPY/EUR/USD切替 |
| 言語設定 | English/Japanese |
| データエクスポート | CSV一括ダウンロード |

## 画像表示仕様（FR-011）

### 画像パス解決（元コード: CountryViewModel.ImagePath, SideViewModel.CountryImagePath）

| 画像種別 | パス | 用途 | 元WPFコード |
|---|---|---|---|
| 国旗 | `image/Flags/{CountryCode}.png` | 国選択ドロップダウン、ルート表示 | CountryViewModel.ImagePath |
| ワールド画像 | `image/world.JPEG` | ワールドモード時のヘッダー画像 | SideViewModel.CountryImagePath (world) |
| 国別写真 | `image/Countries/{CountryCode}/zero.jpg` | カントリーモード時のヘッダー画像 | SideViewModel.CountryImagePath (country) |
| favicon | `image/Icon/icon.png` | ブラウザタブアイコン | - |

### 国旗表示箇所

- **フィルタパネル国選択ドロップダウン**: 各国名の左に国旗アイコン（22x15px相当）
- **ルートタブ国境越え一覧**: 出発国・到着国の表示に国旗アイコン
- 元コード: CountryListView.xaml `<Image Width="22" Height="15" Source="{Binding ImagePath}"/>`

### ヘッダー画像表示

- **ワールドモード**（国未選択）: `image/world.JPEG` を表示
- **カントリーモード**（国選択時）: `image/Countries/{CountryCode}/zero.jpg` を表示
- 画像が見つからない場合はワールド画像にフォールバック
- 元コード: SideView.xaml `<Image Source="{Binding CountryImagePath}"/>`

### 画像ファイルの配置

- ビルド時に `documents/image/` を `public/image/` にコピー（CSVと同様のプラグインで自動化）
- WEB版では画像パスは `{BASE_URL}image/...` で固定（OptionModel.ImagePathによる動的設定は不要）

## アッパーパネル（フィルタ）仕様

| コントロール | 型 | 説明 | 表示条件 |
|---|---|---|---|
| 国選択 | Dropdown | 国名で表示（`countryName()`変換）。選択で カントリーモード遷移 | 常時 |
| 地域選択 | Dropdown | 選択国の地域リスト | 常時 |
| 日付範囲（From） | DatePicker | 開始日 | 常時 |
| 日付範囲（To） | DatePicker | 終了日 | 常時 |
| 表示通貨 | Dropdown | JPY / EUR / USD | 常時 |
| Exclude Airplane | Checkbox | 飛行機移動の除外（FR-007-04） | 常時 |
| Exclude Insurance | Checkbox | 保険費用の除外（FR-007-05） | 常時 |
| Exclude Cross-Border | Checkbox | 国境越え移動の除外（FR-007-04） | **カントリーモード時のみ表示** |
| Exclude Japan | Checkbox | 日本データの除外（FR-007-06） | **ワールドモード時のみ表示** |
| Clear | Button | 全フィルタ条件を初期値にリセット | 常時 |

### 元コードとの対応

| 元WPFコントロール | WEB版 | 備考 |
|---|---|---|
| IsWithAirplane Toggle | Exclude Airplane Checkbox | 論理反転（含む→除外） |
| IsWithCrossBorder Toggle | Exclude Cross-Border Checkbox | 論理反転、カントリーモード時のみ |
| IsWithInsurance Toggle | Exclude Insurance Checkbox | 論理反転 |
| 日本除外 Toggle | Exclude Japan Checkbox | 論理反転、ワールドモード時のみ |

## テスト仕様

#### E2E-UI-001-01: タブ切替
- 各タブクリックで対応データが表示されること
- タブ切替後もフィルタ状態が維持されること

#### E2E-UI-001-02: サイドバー操作
- 国クリックでカントリーモード遷移
- 地域クリックでリージョンモード遷移
- 戻り操作でワールドモード復帰

#### E2E-UI-001-03: フィルタ操作
- 各トグルのON/OFFでデータが再フィルタされること
- 日付範囲変更でデータが絞り込まれること
- 通貨切替で金額表示が変わること
