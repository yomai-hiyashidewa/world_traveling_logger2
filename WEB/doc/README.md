# World Traveling Logger - WEB版 設計ドキュメント

## ドキュメント構成

```
WEB/
├── doc/
│   ├── 00_要求定義/           ... 要求定義（機能・非機能）
│   │   ├── 機能要求/          ... FR-001〜FR-010
│   │   └── 非機能要求/        ... NFR-001〜NFR-006
│   ├── 01_仕様/               ... TDD可能な粒度の仕様
│   │   ├── データモデル/      ... SP-DM-001〜006
│   │   ├── API/               ... SP-API-001〜002（内部サービスI/F）
│   │   ├── 画面/              ... SP-UI-001
│   │   └── ビジネスロジック/  ... SP-BL-001〜003
│   ├── 02_方針/               ... 技術方針・実装順序
│   │   ├── 技術方針.md
│   │   └── 実装順序.md
│   └── README.md              ... 本ファイル
├── model/
│   ├── architecture.md        ... システムアーキテクチャ図
│   ├── domain.md              ... ドメインモデル図
│   ├── usecase.md             ... ユースケース図
│   ├── class_diagram.md       ... クラス図
│   ├── er_diagram.md          ... ER図（将来のDB化に備えた参考）
│   ├── state.md               ... 状態遷移図
│   └── sequence/              ... シーケンス図
│       ├── init.md            ... 起動・データ読み込み
│       ├── data_crud.md       ... CSVパース・振り分け詳細
│       ├── filter_calc.md     ... フィルタ・集計・通貨切替
│       └── route_analysis.md  ... ルート分析
```

## 技術スタック

| 技術 | 用途 |
|---|---|
| TypeScript | 言語 |
| React | UIフレームワーク |
| Vite | ビルドツール |
| Vitest + Testing Library | テスト |
| GitHub Pages | デプロイ先 |

## 設計方針

- **GitHub Pages（静的ホスティング）** — バックエンド・DB不要
- **CSVデータ** — `documents/English/` からfetchで読み込み
- **全ロジックはクライアントサイド** — Domain層はPure TypeScript
- **TDD** — Domain層から先にテストを書いて実装
- **上位→下位の階層的分解** — 要求→仕様→テスト仕様
