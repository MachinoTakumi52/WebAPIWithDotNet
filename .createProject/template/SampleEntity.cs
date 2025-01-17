namespace PROJECT_NAME.Models.Entities
{



    public class SampleEntity
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <param name="adress"></param>
        public SampleEntity(int? id = null, string? password = null, string? name = null, int? age = null, int? gender = null, string? adress = null)
        {
            this.Id = id;
            this.Password = password;
            this.Name = name;
            this.Age = age;
            this.Gender = gender;
            this.Adress = adress;

        }

        /// <summary>
        /// ID
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// パスワード
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// // 名前
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 年齢
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// 住所
        /// </summary>
        public string? Adress { get; set; }
    }

    /// <summary>
    /// サンプルテーブルを主テーブルとして、部署テーブルをリレイションしたエンティティ
    /// </summary>
    public class SampleEntityWithDivison
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// // 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年齢
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 部門
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="division"></param>
        public SampleEntityWithDivison(int id, string name, int age, string division)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Division = division;
        }
    }

}