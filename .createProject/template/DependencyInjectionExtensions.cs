using PROJECT_NAME.Repositories;
using PROJECT_NAME.Services;

namespace PROJECT_NAME.Utils;
/// <summary>
/// DI拡張メソッド
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// DIコンテナにサービスを登録
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {

        // DIコンテナにサービスを登録
        // 以下のように、インターフェースと実装クラスを登録することで、リクエストごとにインスタンスが生成される
        services.AddScoped<ISampleService, SampleService>();
        return services;
    }

    /// <summary>
    /// DIコンテナにリポジトリを登録
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // DIコンテナにリポジトリを登録
        // 以下のように、インターフェースと実装クラスを登録することで、リクエストごとにインスタンスが生成される
        services.AddScoped<ISampleRepository, SampleRepository>();

        return services;
    }

    // /// <summary>
    // /// DIコンテナにデータベース接続を登録
    // /// </summary>
    // /// <param name="services"></param>
    // /// <returns></returns>
    // public static IServiceCollection AddDataBaseConnections(this IServiceCollection services)
    // {
    //     // DIコンテナにデータベース接続を登録
    //     // 以下のように、インターフェースと実装クラスを登録することで、リクエストごとにインスタンスが生成される
    //     services.AddScoped<DataBaseConnectionForPostgreSQL>();

    //     return services;
    // }
}
