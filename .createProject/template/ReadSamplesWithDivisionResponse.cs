/// コントローラーのアクションに対するレスポンスモデル
/// コントローラ名毎にフォルダを切り、その中にレスポンスモデルを作成する
namespace PROJECT_NAME.Models.Responses.Sample;
/// <summary>
/// サンプルレスポンス
/// 以下のように返却される
/// {
///   "Users": [
///   {
///     "ID": 1,
///     "Name": "山田太郎",
///     "Age": 30,
///     "Division": "開発部"
///    }
///     ]
/// }
/// 
/// ファイル名とクラス名は、Action名 + Response
/// このレスポンスモデルは、sampleContorollerのReadSamplesアクションに対するレスポンスモデルなので、
/// ReadSamplesResponse とする
/// </summary>
public class ReadSamplesWithDivisionResponse
{
    /// <summary>
    /// ユーザー情報リスト
    /// </summary>
    public IEnumerable<ReadSamplesWithDivisionResponseDataUserModel> Users { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="users"></param>
    public ReadSamplesWithDivisionResponse(IEnumerable<ReadSamplesWithDivisionResponseDataUserModel> users)
    {
        this.Users = users;
    }
}

/// <summary>
/// レスポンスデータ
/// ユーザー情報
/// レスポンスに必要になってくるモデルクラスは、レスポンスクラス内にネストして作成する
/// </summary>
public class ReadSamplesWithDivisionResponseDataUserModel
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="age"></param>
    public ReadSamplesWithDivisionResponseDataUserModel(int id, string name, int age, string division)
    {
        this.ID = id;
        this.Name = name;
        this.Age = age;
        this.Division = division;
    }

    /// <summary>
    /// ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// 部署
    /// </summary>
    public string Division { get; set; }
}
