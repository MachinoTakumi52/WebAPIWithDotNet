using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webApiProJect;

/// <summary>
/// 例外処理コントローラー
/// </summary>
[Route($"api/{buildsettings.version}/[controller]")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    /// <summary>
    /// 例外処理API　例外が起きたときはここに来る
    /// </summary>
    /// <returns></returns>
    [Route("/error")]
    public JsonResult Error()
    {
        //コンテキスト取得
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        //例外取得
        var exception = context!.Error;
        //エラーメッセージ設定
        var response = new { error = exception.Message };
        return new JsonResult(response);
    }
}