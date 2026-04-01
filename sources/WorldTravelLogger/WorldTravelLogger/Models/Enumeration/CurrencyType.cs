using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Enumeration
{
    // ISO4217
    // https://ja.wikipedia.org/wiki/ISO_4217
    public enum CurrencyType
    {
        [Display(Name = "¥")]
        JPY,    // 日本円
        [Display(Name = "€")]
        EUR,    // ユーロ
        // ドル
        [Display(Name = "$")]
        USD,    // アメリカドル
        [Display(Name = "A$")]
        AUD,    // オーストラリアドル
        [Display(Name = "C$")]
        CAD,    // カナダドル
        [Display(Name = "B$")]
        BZD,    // ベリーズドル
        [Display(Name = "NT$")]
        TWD,    // 新台湾ドル
        [Display(Name = "HK$")]
        HKD,    // 香港ドル
        // ペソ
        [Display(Name = "M$")]
        MXP,    // メキシコペソ
        [Display(Name = "C$")]
        CUP,    // キューバペソ
        [Display(Name = "Ar$")]
        ARS,    // アルゼンチンペソ
        [Display(Name = "U$")]
        UYU,    // ウルグアイペソ
        [Display(Name = "CO$")]
        COP,    // コロンビアペソ
        [Display(Name = "CL$")]
        CLP,    // チリペソ
        [Display(Name = "₱")]
        PHP,    // フィリピンペソ

        // ポンド
        [Display(Name = "£")]
        GBP,    // イギリスポンド
        [Display(Name = "E£")]
        EGP,    // エジプトポンド

        // ディルハム
        [Display(Name = "UAED")]
        AED,    // UAEディルハム
        [Display(Name = "MAD")]
        MAD,    // モロッコディルハム

        // クローナ
        [Display(Name = "Dkr")]
        DKK,    // デンマーククローネ
        [Display(Name = "Ikr")]
        ISK,    // アイスランドクローナ
        [Display(Name = "Skr")]
        SEK,    // スウェーデンクローナ
        [Display(Name = "Nkr")]
        NOK,    // ノルウェークローネ

        // ディナール
        [Display(Name = "Sдин.")]
        RSD,    // セルビアディナール
        [Display(Name = "TD")]
        TND,    // チュニジアディナール
        [Display(Name = "Mден")]
        MKD,    // マケドニア・デナール

        // レイ
        [Display(Name = "L")]
        MDL,    // モルドバレイ
        [Display(Name = "lei")]
        RON,    // ルーマニアレイ

        [Display(Name = "L")]
        ALL,    // アルバニアレク
        [Display(Name = "֏")]
        AMD,    // アルメニアドラム
        [Display(Name = "₹")]
        INR,    // インドルピー
        [Display(Name = "лв")]
        UZS,    // ウズベキスタンスム
        [Display(Name = "Q")]
        GTQ,    // グアテマラケツァル
        [Display(Name = "₡")]
        CRC,    // コスタリカコロン
        [Display(Name = "₾")]
        GEL,    // ジョージアラリ
        [Display(Name = "CHF")]
        CHF,    // スイスフラン
        [Display(Name = "Kč")]
        CZK,    // チェココルナ
        [Display(Name = "₺")]
        TRY,    // トルコリラ
        [Display(Name = "C$")]
        NIO,    // ニカラグアコルドバオロ
        [Display(Name = "Ft")]
        HUF,    // ハンガリーフォリント
        [Display(Name = "₲")]
        PYG,    // パラグアイグアラニ
        [Display(Name = "R$")]
        BRL,    // ブラジルレアル
        [Display(Name = "лв")]
        BGN,    // ブルガリアレフ
        [Display(Name = "₫")]
        VND,    // ベトナムドン
        [Display(Name = "S/")]
        PEN,    // ペルーソル
        [Display(Name = "KM")]
        BAM,    // ボスニア・ヘルツェゴビナマルク
        [Display(Name = "Bs.")]
        BOB,    // ボリビアボリビアノ
        [Display(Name = "zł")]
        PLN,    // ポーランドズロチ
        [Display(Name = "₩")]
        KRW,    // 大韓民国ウォン
    }

    public enum MajorCurrencytype
    {
        [Display(Name = "¥")]
        JPN,    // 日本円
        [Display(Name = "$")]
        USD,    // USドル
        [Display(Name = "€")]
        EUR,    // ユーロ
    }
}
