using System.ComponentModel.DataAnnotations;

/// コントローラーのアクションに対するリクエスト
/// コントローラ名毎にフォルダを切り、その中にリクエストモデルを作成する
namespace PROJECT_NAME.Models.Requests.Sample;
/// <summary>
/// サンプルリクエストモデル
/// モデルクラスは、リクエストボディのJSONを受け取るためのクラス
/// クラス名は、〇〇Requestのようにして作成する
/// 例えば、ユーザー情報を取得する場合は、ReadUserRequestのようにする
/// </summary>
public class ReadSamplesRequest
{
    //constructor
    public ReadSamplesRequest(string name)
    {
        Name = name;
    }

    // アノテーションを使用することで、プロパティに属性を付与できる
    // アノテーションを利用して、バリデーションを行うことができる
    // Required属性を使用することで、必須項目を指定できる
    [Required(ErrorMessage = "名前は必須です")]

    // 受け取りたいJSONのキー名を定義
    public string Name { get; set; }
}