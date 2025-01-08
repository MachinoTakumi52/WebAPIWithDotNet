using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PROJECT_NAME.Controllers
{
    /// <summary>
    /// 例外処理コントローラー
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// 例外処理API　例外が起きたときはここに来る
        /// </summary>
        /// <returns></returns>
        [Route("/Error")]
        public ActionResult Error()
        {
            //コンテキスト取得
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return StatusCode(500, new { message = context!.Error.Message });
        }
    }
}