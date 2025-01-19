namespace PROJECT_NAME.Models.Requests.Sample;

/// <summary>
/// サンプルリクエストモデル
/// モデルクラスは、リクエストボディのJSONを受け取るためのクラス
/// クラス名は、〇〇Requestのようにして作成する
/// </summary>
public class CreateSampleRequest
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="age"></param>
    /// <param name="gender"></param>
    /// <param name="adress"></param>
    public CreateSampleRequest(string password, string name, int age, int gender, string adress)
    {
        this.Password = password;
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
        this.Adress = adress;

    }
    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// // 名前
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// 性別
    /// </summary>
    public int Gender { get; set; }
    /// <summary>
    /// 住所
    /// </summary>
    public string Adress { get; set; }
}
