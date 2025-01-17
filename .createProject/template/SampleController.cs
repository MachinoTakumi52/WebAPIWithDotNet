using Microsoft.AspNetCore.Mvc;
using PROJECT_NAME.Services;
using PROJECT_NAME.Utils;
using jndf.Models.Responses.Sample;
using jndf.Models.Requests.Sample;

namespace PROJECT_NAME.Controllers
{
    /// <summary>
    /// サンプルコントローラー
    /// </summary>
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

        /// <summary>
        /// GETメソッドについて
        /// [HttpGet]属性は、HTTPのGETメソッドを示す属性
        /// 「api/Sample/ReadCombineString」 にアクセスされたときに呼び出される
        /// api/Sample/ReadCombineString?text=Hello にアクセスされたときに、textパラメータにHelloが渡される
        /// [FromQuery]を使用することで、クエリパラメータを受け取ることができる
        /// ActionResult<T>を使用することで、戻り値の型を指定できる
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> ReadCombineString([FromQuery] string text)
        {
            // 環境変数を取得
            // EnvConstsファイルでは、環境変数を取得するためのキーを定数として保持している
            string connectionString = _configuration[EnvConsts.ConnectionString]!;
            return $"Hello {text} {connectionString}";
        }

        /// <summary>
        /// POSTメソッドについて
        /// [HttpPost]属性は、HTTPのPOSTメソッドを示す属性
        ///「api/Sample/ReadSamples にアクセスされたときに呼び出される
        /// [FromBody]を使用することで、リクエストボディを受け取ることができる
        /// 引数の型にモデルクラスを指定することで、リクエストボディのJSONをモデルクラスに変換して受け取ることができる
        /// ActionResult<T>を使用することで、戻り値の型を指定できる
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ReadSamplesResponse> ReadSamples([FromBody] ReadSamplesRequest requestData)
        {
            // Json形式で返却される
            return _sampleService.ReadSamplesBy(requestData: requestData);
        }

        /// <summary>
        /// 別テーブルをリレーションして取得する場合
        /// [HttpPost]属性は、HTTPのPOSTメソッドを示す属性
        ///「api/Sample/ReadSamplesWithDivision にアクセスされたときに呼び出される
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ReadSamplesWithDivisionResponse> ReadSamplesWithDivision([FromBody] ReadSamplesWithDivisionRequest requestData)
        {
            // Json形式で返却される
            return _sampleService.ReadSamplesWithDivisionBy(requestData: requestData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSample([FromBody] CreateSampleRequest requestData)
        {
            // サンプルデータを作成する
            int createDataId = _sampleService.CreateSample(requestData: requestData);

            // 作成したデータのIDを返却する
            // 返却のモデルを作るほどでもない場合は、匿名型で返却する
            return Ok(new { id = createDataId });
        }

        /// <summary>
        /// サンプルデータ更新
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSample([FromBody] UpdateSampleRequest sample)
        {
            // サンプルデータを更新する
            _sampleService.UpdateSample(requestData: sample);

            // 何も返却しない場合は、Ok()を返却する
            return Ok();
        }

        /// <summary>
        ///  サンプルデータ削除
        ///  リクエストモデルを作成する必要がない場合は、引数に直接指定
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSample([FromBody] int sampleId)
        {
            // サンプルデータを削除する
            _sampleService.DeleteSample(sampleId);

            // 何も返却しない場合は、Ok()を返却する
            return Ok();
        }

        /// <summary>
        /// 例外処理について
        /// 「api/Sample/Exception」 にアクセスされたときに呼び出される
        /// 共通でエラーをハンドリングするため例外処理は、エラーをthrowするだけで良い
        /// エラー時、特別catch内処理を書く必要がなければ書く必要はない
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Exception()
        {
            // 例外を発生させる
            throw new Exception("例外が発生しました");
        }
    }
}