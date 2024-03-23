namespace webApiProJect
{
    /// <summary>
    /// buildするときの最後の最後で変数を決めるクラス
    /// build,発行の前に確認すること！！！
    ///
    /// [発行のやり方]
    /// 1.「dotnet publish -c Release -o ./bin/Publish」　コマンドを打つ
    /// 2. bin/Publishフォルダに新しいプロジェクトが発行される
    /// 3. コピーしてサーバー(IIS,Azureとか)に置く
    /// 
    /// </summary>
    public static class buildsettings
    {
        /// <summary>
        /// バージョン　APIのRouteにversionを埋め込んでおくことで変更があったときの事故を防ぐ
        /// ・大きな修正があったときにバージョンを上げていくこと！ ex:v1,v2,v3
        /// </summary>
        public const string version = "v1";
    }
}