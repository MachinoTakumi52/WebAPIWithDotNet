using System.ComponentModel.DataAnnotations;


/// <summary>
/// サンプルモデル
/// </summary>
public class SampleModel
{    //Required属性を使用することで、必須項目を指定できる
    //他属性が記載されているサイトは以下
    //https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.dataannotations?view=net-6.0

    [Required(ErrorMessage = "名前は必須です")]
    public string Name { get; set; }
    //constructor
    public SampleModel(string name)
    {
        Name = name;
    }
}