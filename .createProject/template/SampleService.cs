using PROJECT_NAME.Models.Requests.Sample;
using PROJECT_NAME.Models.Responses.Sample;
using PROJECT_NAME.Models.Entities;
using PROJECT_NAME.Repositories;
using PROJECT_NAME.Utils;

namespace PROJECT_NAME.Services
{
    /// <summary>
    /// サンプルサービスインターフェース
    /// サービスクラスは、インターフェースを必ず実装する
    /// インターフェース名は、サービスクラス名に「I」を付けた名前にする
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// 引数を検索条件にサンプルリストを取得する
        /// </summary>
        /// <returns></returns>
        ReadSamplesResponse ReadSamplesBy(ReadSamplesRequest requestData);

        /// <summary>
        /// 引数を検索条件に部署テーブルをリレーションしたサンプルリストを取得する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        ReadSamplesWithDivisionResponse ReadSamplesWithDivisionBy(ReadSamplesWithDivisionRequest requestData);

        /// <summary>
        /// サンプルデータを作成する
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        int CreateSample(CreateSampleRequest requestData);

        /// <summary>
        /// サンプルデータを更新する
        /// </summary>
        /// <param name="requestData"></param>
        void UpdateSample(UpdateSampleRequest requestData);

        /// <summary>
        /// サンプルデータを削除する
        /// </summary>
        /// <param name="sampleId"></param>
        void DeleteSample(int sampleId);

    }
    /// <summary>
    /// サンプルサービス
    /// サービスクラスでは、リポジトリを呼び出してデータベースにアクセスしたり、ビジネスロジックを実装する
    /// インターフェースを継承して実装していく
    /// </summary>
    public class SampleService : ISampleService
    {

        /// <summary>
        /// リポジトリを使用するためのフィールド
        /// </summary>
        private readonly ISampleRepository _sampleRepository;

        /// <summary>
        /// データベース接続を使用するためのフィールド
        /// </summary>
        private readonly DataBaseConnectionForPostgreSQL _dataBaseConnection;

        /// <summary>
        /// コンストラクタ
        /// 使用したいリポジトリは、DIした後コンストラクタの引数に指定することで使用できる
        /// </summary>
        /// <param name="sampleRepository"></param>
        public SampleService(ISampleRepository sampleRepository, DataBaseConnectionForPostgreSQL dataBaseConnection)
        {
            // DIされたISampleRepositoryをフィールドに設定
            _sampleRepository = sampleRepository;

            // DIされたDataBaseConnectionForPostgreSQLをフィールドに設定
            _dataBaseConnection = dataBaseConnection;
        }


        /// <summary>
        /// 引数を検索条件にサンプルリストを取得する
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReadSamplesResponse ReadSamplesBy(ReadSamplesRequest requestData)
        {
            // リポジトリのメソッドを呼び出してデータを取得
            IEnumerable<SampleEntity> resultData = _sampleRepository.ReadSamplesBy(name: requestData.Name);

            // サンプルリストをレスポンスのデータモデルに変換
            IEnumerable<ReadSamplesResponse.UserModel> users = resultData.Select(x => new ReadSamplesResponse.UserModel(id: x.Id.Value, name: x.Name, age: x.Age.Value));

            // 返却
            return new ReadSamplesResponse(users: users);
        }

        /// <summary>
        /// 引数を検索条件に部署テーブルをリレーションしたサンプルリストを取得する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        public ReadSamplesWithDivisionResponse ReadSamplesWithDivisionBy(ReadSamplesWithDivisionRequest requestData)
        {
            // リポジトリのメソッドを呼び出してデータを取得
            IEnumerable<SampleEntityWithDivison> resultData = _sampleRepository.ReadSamplesWithDivisionBy(name: requestData.Name, divisionId: requestData.DivisionId);

            // サンプルリストをレスポンスのデータモデルに変換
            IEnumerable<ReadSamplesWithDivisionResponse.UserModel> users = resultData.Select(x => new ReadSamplesWithDivisionResponse.UserModel(id: x.Id, name: x.Name, age: x.Age, division: x.Division));

            // 返却
            return new ReadSamplesWithDivisionResponse(users: users);
        }

        /// <summary>
        /// サンプルデータを作成する
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public int CreateSample(CreateSampleRequest requestData)
        {

            // リポジトリのメソッドを呼び出してデータを登録
            int createDataId = _sampleRepository.CreateSample(sample: new SampleEntity(id: null, password: requestData.Password, name: requestData.Name, age: requestData.Age, requestData.Gender, requestData.Adress));

            // 返却
            return createDataId;
        }

        /// <summary>
        /// サンプルデータを作成する
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public void UpdateSample(UpdateSampleRequest requestData)
        {

            // リポジトリのメソッドを呼び出してデータを更新
            _sampleRepository.UpdateSample(sample: new SampleEntity(id: null, password: requestData.Password, name: requestData.Name, age: null, gender: null, adress: null));
        }

        /// <summary>
        /// サンプルデータを削除する
        /// </summary>
        /// <param name="sampleId"></param>
        public void DeleteSample(int sampleId)
        {
            // リポジトリのメソッドを呼び出してデータを削除
            _sampleRepository.DeleteSample(id: sampleId);
        }
    }
}