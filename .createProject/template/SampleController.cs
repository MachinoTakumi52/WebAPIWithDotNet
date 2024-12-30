using Consts;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// サンプルコントローラー
/// </summary>
[ApiController] //APIコントローラーを示す属性
[Route($"apis/[controller]/[action]")] //ルーティングの設定
public class SampleController : ControllerBase //コントローラークラスはControllerBaseを継承する必要あり
{
    // 環境変数を使用したい場合は、IConfigurationをDIするので、フィールドを設定する
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
    /// 受け取った文字にHelloを付けて返却
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [HttpGet] //HTTPのGETメソッドを示す属性
    //「api/buildsettings.version/Sample/Get」 にアクセスされたときに呼び出される
    // [FromQuery]を使用することで、クエリパラメータを受け取ることができる
    public JsonResult Get([FromQuery] string text)
    {
        // 環境変数を取得
        // EnvConstsファイルでは、環境変数を取得するためのキーを定数として保持している
        string connectionString = _configuration[EnvConsts.ConnectionString]!;
        return new JsonResult("Hello " + text + connectionString);
    }

    /// <summary>
    /// 受け取った文字にHelloを付けて返却
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [HttpPost]//HTTPのPOSTメソッドを示す属性
    //「api/buildsettings.version/Sample/Post」 にアクセスされたときに呼び出される
    // [FromBody]を使用することで、リクエストボディを受け取ることができる
    public JsonResult Post([FromBody] SampleModel sample)
    {
        return new JsonResult("Hello " + sample.Name);
    }

    /// <summary>
    /// 受け取った文字にHelloを付けて返却
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [HttpPut] //HTTPのPUTメソッドを示す属性
    //「api/buildsettings.version/Sample/Put」 にアクセスされたときに呼び出される
    // [FromBody]を使用することで、クエリパラメータを受け取ることができる
    public JsonResult Put([FromBody] string text)
    {
        return new JsonResult("Hello " + text);
    }

    /// <summary>
    /// 受け取ったIDに対応するデータを削除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete] //HTTPのDELETEメソッドを示す属性
    //「api/buildsettings.version/Sample/Delete」 にアクセスされたときに呼び出される
    // [FromBody]を使用することで、クエリパラメータを受け取ることができる
    public JsonResult Delete([FromBody] int id)
    {
        return new JsonResult("Deleted data with ID: " + id);
    }

}