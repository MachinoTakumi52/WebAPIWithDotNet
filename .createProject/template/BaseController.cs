using Microsoft.AspNetCore.Mvc;

namespace PROJECT_NAME.Controllers;

[ApiController] //APIコントローラーを示す属性
[Route($"api/[controller]/[action]")] //ルーティングの設定
public abstract class BaseController : ControllerBase
{

}