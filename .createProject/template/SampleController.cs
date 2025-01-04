using Consts;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// サンプルコントローラー
/// </summary>
public class SampleController : BaseController //コントローラーはBaseControllerを継承して作成する
{
    // 環境変数を使用したい場合は、IConfigurationをDIする
    private readonly IConfiguration _configuration;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="configuration"></param>
    public SampleController(IConfiguration configuration)
    {
        // DIされたIConfigurationをフィールドに設定
        _configuration = configuration;
    }

    /// <summary>
    /// GETメソッド例
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [HttpGet] //HTTPのGETメソッドを示す属性
    //「api/Sample/Get」 にアクセスされたときに呼び出される
    // [FromQuery]を使用することで、クエリパラメータを受け取ることができる
    // 戻り値をJsonResultにすることで、JSON形式で返却することができる
    public ActionResult<SampleModel> Get([FromQuery] string text)
    {
        // 環境変数を取得
        // EnvConstsファイルでは、環境変数を取得するためのキーを定数として保持している
        string connectionString = _configuration[EnvConsts.ConnectionString]!;
        return new SampleModel($"Hello {text} {connectionString}");
    }

    /// <summary>
    /// POSTメソッド例
    /// </summary>
    /// <param name="sample"></param>
    /// <returns></returns>
    [HttpPost]//HTTPのPOSTメソッドを示す属性
    //「api/Sample/Post」 にアクセスされたときに呼び出される
    // [FromBody]を使用することで、リクエストボディを受け取ることができる
    // 引数の型にモデルクラスを指定することで、リクエストボディのJSONをモデルクラスに変換して受け取ることができる
    public ActionResult<SampleModel> Post([FromBody] SampleModel sample)
    {
        return new SampleModel($"Hello {sample.Name}");
    }

    /// <summary>
    /// 例外処理例
    /// </summary>
    /// <param name="sample"></param>
    /// <returns></returns>
    [HttpPost]//HTTPのPOSTメソッドを示す属性
    //「api/Sample/Exception」 にアクセスされたときに呼び出される
    // 共通でエラーをハンドリングするため例外処理は、エラーをthrowするだけで良い
    public ActionResult<SampleModel> Exception([FromBody] SampleModel sample)
    {

        // 例外を発生させる
        throw new Exception("例外が発生しました");

    }
}