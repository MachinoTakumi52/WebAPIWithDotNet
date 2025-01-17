namespace PROJECT_NAME.Models.Requests.Sample;

/// <summary>
/// サンプルリクエストモデル
/// モデルクラスは、リクエストボディのJSONを受け取るためのクラス
/// クラス名は、〇〇Requestのようにして作成する
/// </summary>
public class UpdateSampleRequest
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="password"></param>
    /// <param name="name"></param>
    public UpdateSampleRequest(string password, string name)
    {
        this.Password = password;
        this.Name = name;

    }
    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// // 名前
    /// </summary>
    public string Name { get; set; }
}
