using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Enumeration
{
    public enum ErrorTypes
    {
        None,           // エラーなし
        FileNotFound,   // ファイルなし
        FileWrong,      // ファイル違い
        FileNotOpen,    // ファイル開けない
        FormatError,    // フォーマットエラー
        DataError,      // データエラー
    }
}
