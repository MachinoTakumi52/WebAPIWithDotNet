using System.Data;
using Npgsql;
using Dapper;
using PROJECT_NAME.Consts;

namespace PROJECT_NAME.Utils
{
    public class DataBaseConnectionForPostgreSQL : IDisposable
    {
        /// <summary>
        /// コネクションインスタンス
        /// </summary>
        private NpgsqlConnection Connection { get; set; }

        /// <summary>
        /// トランザクションインスタンス
        /// </summary>
        public NpgsqlTransaction? Transaction { get; set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataBaseConnectionForPostgreSQL()
        {
            // インスタンス生成
            this.Connection = new NpgsqlConnection(connectionString: EnvConsts.ConnectionString);

            this.Connection.Open();

            //Dapperの設定(アンダーバーを無視する)
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            this.RollBack();
            this.Transaction?.Dispose();
            this.Connection.Close();
            this.Connection.Dispose();
        }

        /// <summary>
        /// SELECT文
        /// </summary>
        /// <param name="sql">生SQL文</param>
        /// <param name="param">パラメータ</param>
        /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object? param = null, NpgsqlTransaction? tran = null)
        {

            return Connection.Query<T>(sql: sql, param: param, transaction: tran ?? Transaction);
        }

        /// <summary>
        /// SELECT文　匿名型用
        /// </summary>
        /// <param name="columsType">匿名型のオブジェクトを渡す</param>
        /// <param name="sql">生SQL文</param>
        /// <param name="param">パラメータ</param>
        /// /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(T columsType, string sql, object? param = null, NpgsqlTransaction? tran = null)
        {
            return Connection.Query<T>(sql: sql, param: param, transaction: tran ?? Transaction);
        }

        /// <summary>
        /// INSERT,UPDATE,DELETE用
        /// </summary>
        /// <param name="sql">生SQL文</param>
        /// <param name="paramList">パラメータリスト</param>
        /// /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public void Execute(string sql, List<NpgsqlParameter>? paramList = null, NpgsqlTransaction? tran = null)
        {
            if (paramList == null)
            {
                Connection.Execute(sql: sql, transaction: tran ?? Transaction);
            }
            else
            {
                using (var command = new NpgsqlCommand(cmdText: sql, connection: this.Connection))
                {
                    // パラメータを追加
                    foreach (var parameter in paramList)
                    {
                        command.Parameters.Add(value: parameter);
                    }

                    command.Transaction = tran ?? Transaction;

                    // クエリを実行
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// bulkInsert文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="entities"></param>
        /// <param name="tableName">テーブル名</param>
        public void BulkInsert<T>(IEnumerable<T> entities, string tableName = "", NpgsqlTransaction? tran = null, bool isCamelToSnake = true)
        {
            BulkInsertForIdentify<T>(entities: entities, tableName: tableName, null, tran: tran, isCamelToSnake: isCamelToSnake);
        }

        /// <summary>
        /// 自動採番列を持つテーブルへの一括Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="tableName"></param>
        /// <param name="identityColumnName"></param>
        /// <param name="transaction"></param>
        public void BulkInsertForIdentify<T>(IEnumerable<T> entities, string tableName = "", string? identityColumnName = null, NpgsqlTransaction? tran = null, bool isCamelToSnake = true)
        {
            // エンティティのプロパティ情報取得
            var properties = typeof(T).GetProperties();

            // データテーブルインスタンス生成
            var dataTable = new DataTable();

            List<string> columnNames = new List<string>();

            // データテーブル作成
            foreach (var property in properties)
            {
                // 自動採番列は除外
                if (property.Name.Equals(value: identityColumnName, comparisonType: StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var columnName = isCamelToSnake ? CamelToSnakeCase(camelCase: property.Name) : property.Name;
                columnNames.Add(item: columnName);

                if (property.PropertyType.IsGenericType
                    && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // null許容型の時
                {
                    // カラム名をスネークケースに変換
                    // モデルファイルのプロパティがキャメルケースの場合、登録時にDBが認識してくれないので
                    // スネークケースに変換する
                    dataTable.Columns.Add(columnName: columnName, type: Nullable.GetUnderlyingType(nullableType: property.PropertyType)!);
                }
                else // null非許容型の時
                {
                    dataTable.Columns.Add(columnName: columnName, type: property.PropertyType);
                }
            }

            // 登録するデータをデータテーブルに追加
            foreach (var entity in entities)
            {
                // 一行ずつ作成
                var row = dataTable.NewRow();

                // プロパティの一つ一つに値を入れる
                foreach (var property in properties)
                {

                    var columnName = isCamelToSnake ? CamelToSnakeCase(camelCase: property.Name) : property.Name;

                    // 自動採番列は除外
                    if (property.Name.Equals(value: identityColumnName, comparisonType: StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    row[columnName] = property.GetValue(obj: entity) ?? DBNull.Value;
                }

                // 登録データ行追加
                dataTable.Rows.Add(row: row);
            }

            // カラム名をカンマ区切りに変換
            string columnNamesConvertString = string.Join(separator: ", ", values: columnNames);
            // バルクインサート
            using (var writer = Connection.BeginBinaryImport(copyFromCommand: $"COPY {tableName} ({columnNamesConvertString}) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    writer.StartRow();
                    foreach (var value in row.ItemArray)
                    {
                        writer.Write(value: value == DBNull.Value ? null : value);
                    }
                }
                writer.Complete();
            }
        }


        /// <summary>
        /// トランザクション 開始
        /// </summary>
        public void BeginTran()
        {
            try
            {
                Transaction = Connection.BeginTransaction();
            }
            catch
            {
                Transaction = null;
                throw;
            }
        }

        /// <summary>
        /// トランザクション コミット
        /// </summary>
        public void Commit()
        {
            try
            {

                Transaction?.Commit();
            }
            finally
            {
                Transaction = null;
            }
        }

        /// <summary>
        /// トランザクション ロールバック
        /// </summary>
        public void RollBack()
        {
            try
            {

                Transaction?.Rollback();
            }
            finally
            {
                Transaction = null;
            }

        }

        /// <summary>
        /// キャメルケースをスネークケースに変換する
        /// </summary>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        private static string CamelToSnakeCase(string camelCase)
        {
            if (string.IsNullOrEmpty(value: camelCase))
                return camelCase;

            var stringBuilder = new System.Text.StringBuilder();
            foreach (char c in camelCase)
            {
                if (char.IsUpper(c: c))
                {
                    if (stringBuilder.Length > 0)
                        stringBuilder.Append(value: '_');
                    stringBuilder.Append(value: char.ToLower(c: c));
                }
                else
                {
                    stringBuilder.Append(value: c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}