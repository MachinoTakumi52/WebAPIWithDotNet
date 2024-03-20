namespace webApi.Const
{
    public class EnvConsts
    {
        /// <summary>
        /// バージョン　APIのRouteにversionを埋め込んでおくことで変更があったときの事故を防ぐ
        /// ・大きな修正があったときにバージョンを上げていくこと！ ex:v1,v2,v3
        /// </summary>
        public static string version { get; set; } = null!;

        /// <summary>
        /// DB接続文字列
        /// ・開発環境から本番環境に移るときに確認する！
        /// </summary>
        public static string connectionString { get; set; } = null!;

        /// <summary>
        /// 開発かどうか
        /// 認証はフロントから送られてくる認証キーを都度確認しているため、単体テストの時は以下の処理を行わないようにするためのフラグ
        /// 結合テスト時にはfalseにする
        /// </summary>
        /// <value></value>
        public static bool isDevelop { get; set; } = true;

        /// <summary>
        /// appsetting.jsonから変数を読み取る
        /// </summary>
        /// <param name="configuration">構成</param>
        public static void getEnvConsts(IConfiguration configuration)
        {
            // パスの取得
            version = configuration["Env:Version"];
            connectionString = configuration["Env:ConnectionString"];
        }
    }
}
