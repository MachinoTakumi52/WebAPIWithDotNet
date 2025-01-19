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
    public UpdateSampleRequest(int id, string password, string name)
    {
        this.Id = id;
        this.Password = password;
        this.Name = name;

    }

    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// // 名前
    /// </summary>
    public string Name { get; set; }
}

