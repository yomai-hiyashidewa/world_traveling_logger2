# SP-API-003: 画像サービスインターフェース仕様

対応要求: FR-011

※ GitHub Pages（静的ホスティング）のためREST APIは存在しない。
本仕様はクライアント内部のサービスインターフェースを定義する。

## ImageService インターフェース

画像パスの生成を一元管理するサービス。
元WPFの OptionModel.ImagePath / CountryViewModel.ImagePath に対応する。

### joinPath（内部ユーティリティ）

```typescript
function joinPath(base: string, ...parts: string[]): string
```

- base 末尾のスラッシュを正規化し、parts をスラッシュで結合
- 全パス生成関数の内部で使用

### getFlagPath

```typescript
function getFlagPath(
  countryCode: CountryType,
  base: string
): string
```

- 国旗画像パスを返す
- 出力: `{base}/image/Flags/{countryCode}.png`
- 使用箇所: CountryFlag コンポーネント、RouteView コンポーネント
- 対応要求: FR-011-01, FR-011-03

### getCountryImagePath

```typescript
function getCountryImagePath(
  countryCode: CountryType | null | undefined,
  base: string
): string
```

- 国別メイン写真パスを返す
- countryCode が null/undefined の場合は `getWorldImagePath(base)` にフォールバック
- 出力: `{base}/image/Countries/{countryCode}/zero.jpg`
- 使用箇所: App コンポーネント（ヘッダー画像）
- 対応要求: FR-011-02

### getWorldImagePath

```typescript
function getWorldImagePath(base: string): string
```

- ワールドモード時のメイン画像パスを返す
- 出力: `{base}/image/world.JPEG`
- 使用箇所: App コンポーネント（ワールドモード時）、getCountryImagePath のフォールバック
- 対応要求: FR-011-02

### getFaviconPath

```typescript
function getFaviconPath(base: string): string
```

- favicon 画像パスを返す
- 出力: `{base}/image/Icon/icon.png`
- 使用箇所: App コンポーネント（favicon 設定）
- 対応要求: FR-011-04

## base パラメータ

全関数は `base` パラメータを受け取る。これは Vite の `import.meta.env.BASE_URL` を想定しており、GitHub Pages デプロイ時のサブディレクトリパスに対応する。

元WPFでは OptionModel.ImagePath で動的に設定していたが、WEB版では Vite のベースURL設定に統一する。

## エラーハンドリング方針

ImageService 自体はパス文字列の生成のみを担当し、画像の存在チェックは行わない。
画像の読み込みエラー時のフォールバックは UI コンポーネント側で処理する:

- **CountryFlag コンポーネント**: `<img>` の `onError` で非表示にする
- **App コンポーネント（ヘッダー画像）**: `<img>` の `onError` でワールド画像にフォールバック

## テスト仕様

#### UT-IMG-001-01: getFlagPath
- `getFlagPath('JPN', '/base')` → `'/base/image/Flags/JPN.png'`
- base 末尾スラッシュの正規化

#### UT-IMG-001-02: getCountryImagePath
- `getCountryImagePath('AUS', '/base')` → `'/base/image/Countries/AUS/zero.jpg'`
- `getCountryImagePath(null, '/base')` → `'/base/image/world.JPEG'`（フォールバック）
- `getCountryImagePath(undefined, '/base')` → `'/base/image/world.JPEG'`（フォールバック）

#### UT-IMG-001-03: getWorldImagePath
- `getWorldImagePath('/base')` → `'/base/image/world.JPEG'`

#### UT-IMG-001-04: getFaviconPath
- `getFaviconPath('/base')` → `'/base/image/Icon/icon.png'`

#### UT-IMG-001-05: joinPath 正規化
- base 末尾に `/` あり → 二重スラッシュにならないこと
- base 末尾に `/` なし → 正しくスラッシュが挿入されること
