using System.ComponentModel.DataAnnotations;


/// <summary>
/// サンプルモデル
/// モデルクラスは、リクエストボディのJSONを受け取るためのクラス
/// </summary>
public class SampleModel
{
    // アノテーションを使用することで、プロパティに属性を付与できる
    // アノテーションを利用して、バリデーションを行うことができる
    // Required属性を使用することで、必須項目を指定できる
    [Required(ErrorMessage = "名前は必須です")]

    // 受け取りたいJSONのキー名を定義
    public string Name { get; set; }

    //constructor
    public SampleModel(string name)
    {
        Name = name;
    }
}