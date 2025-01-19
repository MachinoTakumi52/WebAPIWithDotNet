
using PROJECT_NAME.Models.Entities;

namespace PROJECT_NAME.Repositories;
/// <summary>
/// サンプルリポジトリインターフェース
/// リポジトリクラスは、インターフェースを必ず実装する
/// インターフェース名は、リポジトリクラス名に「I」を付けた名前にする
/// テーブルをリレイションしたデータを取得したい場合は、
/// リレイションする主テーブルの Repository ファイルに取得ソースを書く
/// </summary>
public interface ISampleRepository
{
    /// <summary>
    /// サンプルリストを取得する(全件取得)
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SampleEntity> ReadSamples();
    /// <summary>
    /// 引数の検索条件を元にサンプルリストを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<SampleEntity> ReadSamplesBy(string name);

    /// <summary>
    /// 引数の検索条件を元に部署をリレイションしたサンプルリストを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<SampleEntityWithDivison> ReadSamplesWithDivisionBy(string name, int divisionId);

    /// <summary>
    /// サンプルを作成する
    /// </summary>
    /// <param name="sample"></param>
    public int CreateSample(SampleEntityForCreate sample);

    /// <summary>
    /// サンプルを更新する
    /// </summary>
    /// <param name="sample"></param>
    public void UpdateSample(SampleEntityForUpdate sample);

    /// <summary>
    /// サンプルを削除する
    /// </summary>
    /// <param name="id"></param>
    public void DeleteSample(int id);
}

/// <summary>
/// サンプルリポジトリ
/// リポジトリクラスでは、データベースにアクセスするためのメソッドを実装する
/// インターフェースを継承して実装していく
/// </summary>
public class SampleRepository : ISampleRepository
{
    /// <summary>
    /// サンプルリストを取得する(全件取得)
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SampleEntity> ReadSamples()
    {
        // DB操作
        return [];
    }

    /// <summary>
    /// 引数の検索条件を元にサンプルリストを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<SampleEntity> ReadSamplesBy(string name)
    {
        // DB操作
        return [];
    }

    /// <summary>
    /// 引数の検索条件を元にサンプルリストを取得する
    /// テーブルをリレイションしたデータを取得したい場合は、リレイションする主テーブルの Repository ファイルに取得ソースを書く
    /// </summary>
    /// <param name="name"></param>
    /// <param name="divisionId"></param>
    /// <returns></returns>
    public IEnumerable<SampleEntityWithDivison> ReadSamplesWithDivisionBy(string name, int divisionId)
    {
        // DB操作
        return [];
    }

    /// <summary>
    /// サンプルを作成する
    /// </summary>
    /// <param name="sample"></param>
    public int CreateSample(SampleEntityForCreate sample)
    {
        // DB操作 新規登録ID返却
        return 0;
    }

    /// <summary>
    /// サンプルを更新する
    /// </summary>
    /// <param name="sample"></param>
    public void UpdateSample(SampleEntityForUpdate sample)
    {
        // DB操作
    }

    /// <summary>
    /// サンプルを削除する
    /// </summary>
    /// <param name="sample"></param>
    public void DeleteSample(int id)
    {
        // DB操作
    }
}
