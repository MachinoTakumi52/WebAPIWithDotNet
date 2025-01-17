// 種類毎に定数を定義
namespace PROJECT_NAME.Utils
{
    /// <summary>
    /// 環境変数取得用のキーを定義する定数クラス
    /// 　appsettings.jsonに記載された環境変数を取得するためのキーを定義する
    /// </summary>
    public static class EnvConsts
    {
        /// <summary>
        /// DB接続文字列
        /// </summary>
        public const string ConnectionString = "Env:CONNECTION_STRING";
    }
}
