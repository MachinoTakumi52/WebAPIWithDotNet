using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using webApi.Consts;

//builder����
var builder = WebApplication.CreateBuilder(args);

// �쐬�����R���g���[����ǉ����Ďg�p�ł���悤��
//AddController�Ȃ���map�ł��Ȃ��̂Œ���
//�����ݒ�������Ȃ��ꍇ
//builder.Services.AddControllers();

//���݂̔F�؃��[�U�[�C���X�^���X
var requireAuthenticatedUser = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//�R���g���[���ɋ��ʂ̐ݒ��������ꍇ
builder.Services.AddControllers(options =>
{
   //[Authorize]������S�ẴR���g���[���ɕt�^
   //�F�؍ς݃��[�U�����R���g���[���͎g�p�ł��Ȃ�
   //[allowAnonymous]�����t�^�R���g���[���́A�F�؂Ȃ��Ɏg�p�\
   //�ݒ��ǉ�
    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUser));
});

//swagger���g�����߂̐ݒ�
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

//  ���U�L���b�V���ݒ�
//https://noxi515.hateblo.jp/entry/2018/09/02/194901
//3�̎���������
//�E�C�����������U�L���b�V��
//������ɃX�P�[�����Ȃ����Ƃ��m���ȏꍇ�̂ݎg�p
//���U�L���b�V���Ȃ̂ɁA�A�A
//
//�ESQLServer���U�L���b�V��
//DB��SQLServer�̎��A�L���b�V���p�̃e�[�u�����쐬���ăL���b�V����o�^���邱�Ƃ��ł���
//
//�ERedis���U�L���b�V��
//Redis�̓I�[�v���\�[�X�ŊJ������Ă���Key-Value�^�̃C���������X�g�A
//NoSQL�̈��
//���ɕ��U����킯�ł͂Ȃ�������C�����������U�L���b�V�����g�p����
builder.Services.AddDistributedMemoryCache();

//�F�ؐݒ�
//cookie�F�؃X�L�[��(���`)��ǉ�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {

    //Cookie�L�����Ԑݒ�
    options.ExpireTimeSpan = TimeSpan.FromMinutes(400);

    //�F�؃`�������W�́A�F�؂���Ă��Ȃ����[�U�[���F�؂�K�v�Ƃ���G���h�|�C���g��v�������Ƃ��̃��_�C���N�g��
    options.LoginPath = "/AuthError";

    //�^�C���A�E�g�Ԋu�̔����ȏオ�o�߂����ꍇ�A�L���ȔF�� Cookie �̗L�����������Z�b�g���Ĕ��s�@
    options.SlidingExpiration = true;
});

//���F�ݒ�
//������̃��[�U�[�������g�p�ł���API�Ȃǂ��쐬���鎞�͂����Ő���(���[���̐���)
builder.Services.AddAuthorization(options => {
    //�t�H�[���o�b�N �|���V�[�́A�R���g���[���[�ő��̃|���V�[�܂��͑������w�肳��Ă��Ȃ��ꍇ
    //���F�~�h���E�F�A���ݒ肵���t�H�[���o�b�N�|���V�[���g�p����
    //���ݐݒ肳��Ă���̂��F�؃��[�U�[�ȊO�͂͂������
    options.FallbackPolicy = requireAuthenticatedUser;
});

//CORS�ݒ�
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins() //������I���W�������
            .WithMethods("PUT", "DELETE", "GET", "POST")//�����郁�\�b�h
            .AllowCredentials() //���i����������|���V�[��ݒ�
            .AllowAnyHeader(); //�����Ă����w�b�_��������|���V�[��ݒ�

        }
    );
});

//�Z�b�V�����̒ǉ�
builder.Services.AddSession();

// ���ϐ��擾�����Ăяo��
//applicationSetting.json�t�@�C������Ăяo��
//�J���p(applicationSetting_develop.json)�Ɩ{�ԗp(applicationSetting_product.json)������
//�{�ԓK�p�̎��́AapplicationSetting.json�ɖ{�ԗp�̃t�@�C�����R�s�y
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsetting.json")
    .AddEnvironmentVariables()
    .Build();
EnvConsts.getEnvConsts(config);

//�r���h��APP�쐬
var app = builder.Build();

//cors�L����
app.UseCors(MyAllowSpecificOrigins);

// �f�o�b�O��(develop)������swagger���N������悤��
if (app.Environment.IsDevelopment())
{
    //swagger�ǉ�
    app.UseSwagger();

    //swaggerUI�ǉ�  vscode�̏ꍇ
    //app.UseSwaggerUI(options => {
    //    //�f�t�H���g���[�g��swaggerUI���N���@
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //    options.RoutePrefix = string.Empty;
    //});
    app.UseSwaggerUI();
}

//HTTP �v����HTTPS�ɂ��_�C���N�g����
//HTTP�̃|�[�g�̒�`�����鎞�̂�
app.UseHttpsRedirection();

//UseDefaultFiles��UseStaticFiles�ɂ��ẮA���LURL�Q��
//https://www.ipentec.com/document/asp-net-core-set-default-document-in-static-file-folder
//�v���W�F�N�g����wwwroot�t�H���_�ɔz�u�����ÓI�t�@�C���̃f�t�H���g�h�L�������g�����삷���Ԃ�
//API�ł͎g�p���Ȃ�
//app.UseDefaultFiles();

//wwwroot�t�H���_�ɔz�u�����t�@�C����ÓI�t�@�C���Ƃ��ăA�N�Z�X�\�ȏ�ԂɁ@html,css,img...
//�ÓI�t�@�C����񋟂ł���悤�ɂȂ�
//API�ł͎g�p���Ȃ�
//app.UseStaticFiles();

//���[�e�B���O�̗L����
app.UseRouting();

//�G���[ �n���h�� �p�X��ݒ�
//�G���[���u/Error�v��path�Ƀ��_�C���N�g���A�G���[�A�N�V�������\�b�h�����s
app.UseExceptionHandler("/Error");

//�F�؂̗L����
app.UseAuthentication();

//���F�̗L����
app.UseAuthorization();

//Cookie �|���V�[�@�\���L��
//�f�t�H���g��MinimumSameSitePolicy�l��SameSiteMode.Lax
//https://laboradian.com/same-site-cookies/
app.UseCookiePolicy();

//�Z�b�V�����̗L����
app.UseSession();

//�������[�e�B���O�R���g���[�����}�b�v����
//�����routing�ŃG���h�|�C���g�Ƀ}�b�s���O�������̂ŃR�����g�A�E�g����
// app.MapControllers();

//�K�����[�e�B���O(ASP.NET Core MVC �� Razer Pages)�̍ۂɎg�p����
//����͑������[�e�B���O(WebAPI)�̂��߂���Ȃ�
//�K�����`���Ă�����G���h�|�C���g�ɒǉ�����
//MapDefaultControllerRoute()�͋K��̒�`��ǉ����郁�\�b�h
//app.MapDefaultControllerRoute();

// �R���g���[���[�Œ�`�����A�N�V���������G���h�|�C���g�Ƃ��Đݒ肷��
// https://blog.beachside.dev/entry/2020/12/23/144444
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

//���s
app.Run();