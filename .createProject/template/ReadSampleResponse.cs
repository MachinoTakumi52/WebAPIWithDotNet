/// コントローラーのアクションに対するレスポンスモデル
/// コントローラ名毎にフォルダを切り、その中にレスポンスモデルを作成する
namespace PROJECT_NAME.Models.Responses.Sample
{
    /// <summary>
    /// サンプルレスポンス
    /// 以下のように返却される
    /// {
    ///   "Users": [
    ///   {
    ///     "ID": 1,
    ///     "Name": "山田太郎",
    ///     "Age": 30,
    ///    }
    ///     ]
    /// }
    /// 
    /// ファイル名とクラス名は、Action名 + Response
    /// このレスポンスモデルは、sampleContorollerのReadSamplesアクションに対するレスポンスモデルなので、
    /// ReadSamplesResponse とする
    /// </summary>
    public class ReadSamplesResponse
    {
        /// <summary>
        /// ユーザー情報リスト
        /// </summary>
        public IEnumerable<UserModel> Users { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="users"></param>
        public ReadSamplesResponse(IEnumerable<UserModel> users)
        {
            this.Users = users;
        }

        /// <summary>
        /// レスポンスデータ
        /// ユーザー情報
        /// レスポンスに必要になってくるモデルクラスは、レスポンスクラス内にネストして作成する
        /// </summary>
        public class UserModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="age"></param>
            public UserModel(int id, string name, int age)
            {
                this.ID = id;
                this.Name = name;
                this.Age = age;
            }

            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 名前
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 年齢
            /// </summary>
            public int Age { get; set; }
        }
    }
}