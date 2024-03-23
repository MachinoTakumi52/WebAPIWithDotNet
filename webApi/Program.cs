using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using webApi.Consts;

//builder生成
var builder = WebApplication.CreateBuilder(args);

// 作成したコントローラを追加して使用できるように
//AddControllerなしにmapできないので注意
//何も設定を加えない場合
//builder.Services.AddControllers();

//現在の認証ユーザーインスタンス
var requireAuthenticatedUser = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//コントローラに共通の設定を加える場合
builder.Services.AddControllers(options =>
{
   //[Authorize]属性を全てのコントローラに付与
   //認証済みユーザしかコントローラは使用できない
   //[allowAnonymous]属性付与コントローラは、認証なしに使用可能
   //設定を追加
    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUser));
});

//swaggerを使うための設定
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

//  分散キャッシュ設定
//https://noxi515.hateblo.jp/entry/2018/09/02/194901
//3つの実装がある
//・インメモリ分散キャッシュ
//複数台にスケールしないことが確実な場合のみ使用
//分散キャッシュなのに、、、
//
//・SQLServer分散キャッシュ
//DBがSQLServerの時、キャッシュ用のテーブルを作成してキャッシュを登録することができる
//
//・Redis分散キャッシュ
//Redisはオープンソースで開発されているKey-Value型のインメモリストア
//NoSQLの一種
//特に分散するわけではなかったらインメモリ分散キャッシュを使用する
builder.Services.AddDistributedMemoryCache();

//認証設定
//cookie認証スキーム(雛形)を追加
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {

    //Cookie有効時間設定
    options.ExpireTimeSpan = TimeSpan.FromMinutes(400);

    //認証チャレンジは、認証されていないユーザーが認証を必要とするエンドポイントを要求したときのリダイレクト先
    options.LoginPath = "/AuthError";

    //タイムアウト間隔の半分以上が経過した場合、有効な認証 Cookie の有効期限をリセットし再発行　
    options.SlidingExpiration = true;
});

//承認設定
//ある一定のユーザーだけが使用できるAPIなどを作成する時はここで制御(ロールの制御)
builder.Services.AddAuthorization(options => {
    //フォールバック ポリシーは、コントローラーで他のポリシーまたは属性が指定されていない場合
    //承認ミドルウェアが設定したフォールバックポリシーを使用する
    //現在設定されているのが認証ユーザー以外ははじかれる
    options.FallbackPolicy = requireAuthenticatedUser;
});

//CORS設定
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins() //許可するオリジン入れる
            .WithMethods("PUT", "DELETE", "GET", "POST")//許可するメソッド
            .AllowCredentials() //資格情報を許可するポリシーを設定
            .AllowAnyHeader(); //送られてきたヘッダを許可するポリシーを設定

        }
    );
});

//セッションの追加
builder.Services.AddSession();

// 環境変数取得処理呼び出し
//applicationSetting.jsonファイルから呼び出し
//開発用(applicationSetting_develop.json)と本番用(applicationSetting_product.json)が存在
//本番適用の時は、applicationSetting.jsonに本番用のファイルをコピペ
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsetting.json")
    .AddEnvironmentVariables()
    .Build();
EnvConsts.getEnvConsts(config);

//ビルドしAPP作成
var app = builder.Build();

//cors有効化
app.UseCors(MyAllowSpecificOrigins);

// デバッグ環境(develop)時だけswaggerが起動するように
if (app.Environment.IsDevelopment())
{
    //swagger追加
    app.UseSwagger();

    //swaggerUI追加  vscodeの場合
    //app.UseSwaggerUI(options => {
    //    //デフォルトルートでswaggerUIを起動　
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //    options.RoutePrefix = string.Empty;
    //});
    app.UseSwaggerUI();
}

//HTTP 要求をHTTPSにいダイレクトする
//HTTPのポートの定義がある時のみ
app.UseHttpsRedirection();

//UseDefaultFilesとUseStaticFilesについては、下記URL参照
//https://www.ipentec.com/document/asp-net-core-set-default-document-in-static-file-folder
//プロジェクト内のwwwrootフォルダに配置した静的ファイルのデフォルトドキュメントが動作する状態に
//APIでは使用しない
//app.UseDefaultFiles();

//wwwrootフォルダに配置したファイルを静的ファイルとしてアクセス可能な状態に　html,css,img...
//静的ファイルを提供できるようになる
//APIでは使用しない
//app.UseStaticFiles();

//ルーティングの有効化
app.UseRouting();

//エラー ハンドラ パスを設定
//エラー時「/Error」のpathにリダイレクトし、エラーアクションメソッドを実行
app.UseExceptionHandler("/Error");

//認証の有効化
app.UseAuthentication();

//承認の有効化
app.UseAuthorization();

//Cookie ポリシー機能が有効
//デフォルトのMinimumSameSitePolicy値はSameSiteMode.Lax
//https://laboradian.com/same-site-cookies/
app.UseCookiePolicy();

//セッションの有効化
app.UseSession();

//属性ルーティングコントローラをマップする
//今回はroutingでエンドポイントにマッピングさせたのでコメントアウトした
// app.MapControllers();

//規則ルーティング(ASP.NET Core MVC や Razer Pages)の際に使用する
//今回は属性ルーティング(WebAPI)のためいらない
//規則を定義してそれをエンドポイントに追加する
//MapDefaultControllerRoute()は規約の定義を追加するメソッド
//app.MapDefaultControllerRoute();

// コントローラーで定義したアクション名をエンドポイントとして設定する
// https://blog.beachside.dev/entry/2020/12/23/144444
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

//実行
app.Run();