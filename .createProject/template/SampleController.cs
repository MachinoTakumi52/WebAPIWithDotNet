using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using PROJECT_NAME.Services;
using PROJECT_NAME.Utils;
using PROJECT_NAME.Models.Responses.Sample;
using PROJECT_NAME.Models.Requests.Sample;

namespace PROJECT_NAME.Controllers;
public class SampleController : BaseController //コントローラーはBaseControllerを継承して作成する
{
    // 環境変数を使用したい場合は、IConfigurationをDIする
    private readonly IConfiguration _configuration;

    // サービスを使用するためのフィールド
    private readonly ISampleService _sampleService;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="configuration"></param>
    public SampleController(IConfiguration configuration, ISampleService sampleService)
    {
        // DIされたIConfigurationをフィールドに設定
        _configuration = configuration;
        // DIされたISampleServiceをフィールドに設定
        _sampleService = sampleService;
    }

    /// GETメソッドについて
    /// 「api/Sample/ReadCombineString」 にアクセスされたときに呼び出される
    /// api/Sample/ReadCombineString?text=Hello にアクセスされたときに、textパラメータにHelloが渡される
    /// [FromQuery]を使用することで、クエリパラメータを受け取ることができる
    /// ActionResult<T>を使用することで、戻り値の型を指定できる
    [HttpGet]
    [SwaggerOperation(
        Summary = "文字列返却",
        Description = "クエリパラメータで受け取った文字列を加工して返却する"
    )]
    public ActionResult<string> ReadCombineString([FromQuery] string text)
    {
        // 環境変数を取得
        // EnvConstsファイルでは、環境変数を取得するためのキーを定数として保持している
        string connectionString = _configuration[EnvConsts.ConnectionString]!;
        return $"Hello {text} {connectionString}";
    }

    /// POSTメソッドについて
    /// [HttpPost]属性は、HTTPのPOSTメソッドを示す属性
    ///「api/Sample/ReadSamples にアクセスされたときに呼び出される
    /// [FromBody]を使用することで、リクエストボディを受け取ることができる
    /// 引数の型にモデルクラスを指定することで、リクエストボディのJSONをモデルクラスに変換して受け取ることができる
    /// ActionResult<T>を使用することで、戻り値の型を指定できる
    [HttpPost]
    [SwaggerOperation(
        Summary = "サンプルデータ取得",
        Description = "リクエストボディで受け取った条件に合致するサンプルデータを取得する"
    )]
    public ActionResult<ReadSamplesResponse> ReadSamples([FromBody] ReadSamplesRequest requestData)
    {
        // Json形式で返却される
        return _sampleService.ReadSamplesBy(requestData: requestData);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "部署テーブルをリレーションしたサンプルデータ取得",
        Description = "引数を検索条件に部署テーブルをリレーションしたサンプルリストを取得する"
    )]
    public ActionResult<ReadSamplesWithDivisionResponse> ReadSamplesWithDivision([FromBody] ReadSamplesWithDivisionRequest requestData)
    {
        // Json形式で返却される
        return _sampleService.ReadSamplesWithDivisionBy(requestData: requestData);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "サンプルデータ作成",
        Description = "リクエストボディで受け取ったデータを元にサンプルデータを作成する"
    )]
    public ActionResult CreateSample([FromBody] CreateSampleRequest requestData)
    {
        // サンプルデータを作成する
        int createDataId = _sampleService.CreateSample(requestData: requestData);

        // 作成したデータのIDを返却する
        // 返却のモデルを作るほどでもない場合は、匿名型で返却する
        return Ok(new { id = createDataId });
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "サンプルデータ更新",
        Description = "リクエストボディで受け取ったデータを元にサンプルデータを更新する"
    )]
    public ActionResult UpdateSample([FromBody] UpdateSampleRequest sample)
    {
        // サンプルデータを更新する
        _sampleService.UpdateSample(requestData: sample);

        // 何も返却しない場合は、Ok()を返却する
        return Ok();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "サンプルデータ削除",
        Description = "リクエストボディで受け取ったデータを元にサンプルデータを削除する"
    )]
    public ActionResult DeleteSample([FromBody] int sampleId)
    {
        // サンプルデータを削除する
        _sampleService.DeleteSample(sampleId);

        // 何も返却しない場合は、Ok()を返却する
        return Ok();
    }

    /// 「api/Sample/Exception」 にアクセスされたときに呼び出される
    /// 共通でエラーをハンドリングするため例外処理は、エラーをthrowするだけで良い
    /// エラー時、特別catch内処理を書く必要がなければ書く必要はない
    [HttpPost]
    [SwaggerOperation(
        Summary = "例外処理",
        Description = "例外を発生させる"
    )]
    public ActionResult Exception()
    {
        // 例外を発生させる
        throw new Exception("例外が発生しました");
    }
}