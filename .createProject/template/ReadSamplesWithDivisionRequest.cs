using System.ComponentModel.DataAnnotations;

namespace PROJECT_NAME.Models.Requests.Sample
{
    /// <summary>
    /// サンプルリクエストモデル
    /// モデルクラスは、リクエストボディのJSONを受け取るためのクラス
    /// クラス名は、〇〇Requestのようにして作成する
    /// </summary>
    public class ReadSamplesWithDivisionRequest
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="divisionId"></param>
        public ReadSamplesWithDivisionRequest(string name, int divisionId)
        {
            Name = name;
            DivisionId = divisionId;
        }

        // アノテーションを使用することで、プロパティに属性を付与できる
        // アノテーションを利用して、バリデーションを行うことができる
        // Required属性を使用することで、必須項目を指定できる
        [Required(ErrorMessage = "名前は必須です")]

        // 受け取りたいJSONのキー名を定義
        public string Name { get; set; }

        [Required(ErrorMessage = "部署IDは必須です")]

        // 受け取りたいJSONのキー名を定義
        public int DivisionId { get; set; }

    }
}