using Microsoft.AspNetCore.Mvc;
using webApiProJect;


/// <summary>
/// サンプルコントローラー
/// </summary>
[ApiController] //APIコントローラーを示す属性
[Route($"api/{buildsettings.version}/[controller]/[action]")] //ルーティングの設定
public class SampleController : Controller
{
    /// <summary>
    /// Hello Worldを返却
    /// </summary>
    /// <returns></returns>
    [HttpGet] //HTTPのGETメソッドを示す属性
    //「api/v1/Sample/Search」 にアクセスされたときに呼び出される
    public JsonResult Search()
    {
        //返却
        return new JsonResult("Hello World");
    }
}