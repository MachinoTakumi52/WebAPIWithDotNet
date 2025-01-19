using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace PROJECT_NAME.Utils;
public class DataBaseConnectionForSqlServer : IDisposable
{
    /// <summary>
    /// コネクションインスタンス
    /// </summary>
    private SqlConnection Connection { get; set; }

    /// <summary>
    /// トランザクションインスタンス
    /// </summary>
    public SqlTransaction? Transaction { get; set; } = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DataBaseConnectionForSqlServer()
    {
        // インスタンス生成
        this.Connection = new SqlConnection(connectionString: EnvConsts.ConnectionString);

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
    public IEnumerable<T> Query<T>(string sql, object? param = null, SqlTransaction? tran = null)
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
    public IEnumerable<T> Query<T>(T columsType, string sql, object? param = null, SqlTransaction? tran = null)
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
    public void Execute(string sql, List<SqlParameter>? paramList = null, SqlTransaction? tran = null)
    {
        if (paramList == null)
        {
            Connection.Execute(sql: sql, transaction: tran ?? Transaction);
        }
        else
        {
            using (var command = new SqlCommand(cmdText: sql, connection: this.Connection))
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
    /// <param name="entitties">登録データ</param>
    /// <param name="tableName">登録テーブル名</param>
    /// <param name="tran">トランザクション</param>
    /// <param name="isCamelToSnake">スネーク型からキャメル型に変換するか</param>
    public void BulkInsert<T>(IEnumerable<T> entities, string tableName = "", SqlTransaction? tran = null, bool isCamelToSnake = true)
    {
        BulkInsertForIdentify<T>(entities: entities, tableName: tableName, identityColumnName: null, tran: tran, isCamelToSnake: isCamelToSnake);
    }

    /// <summary>
    /// 自動採番列を持つテーブルへの一括Insert
    /// </summary>
    /// <typeparam name="T"></typeparam>
    ///  /// <param name="entitties">登録データ</param>
    /// <param name="tableName">登録テーブル名</param>
    /// <param name="identityColumnName">自動採番列名</param>
    /// <param name="tran">トランザクション</param>
    /// <param name="isCamelToSnake">スネーク型からキャメル型に変換するか</param>
    public void BulkInsertForIdentify<T>(IEnumerable<T> entities, string tableName = "", string? identityColumnName = null, SqlTransaction? tran = null, bool isCamelToSnake = true)
    {
        // エンティティのプロパティ情報取得
        var properties = typeof(T).GetProperties();

        // データテーブルインスタンス生成
        var dataTable = new DataTable();

        // データテーブル作成
        foreach (var property in properties)
        {
            // 自動採番列は除外
            if (property.Name.Equals(value: identityColumnName, comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // カラム名をスネークケースに変換
            // モデルファイルのプロパティがキャメルケースの場合、登録時にDBが認識してくれないので
            // スネークケースに変換する
            var columName = isCamelToSnake ? CamelToSnakeCase(camelCase: property.Name) : property.Name;

            // データテーブルに値を格納
            if (property.PropertyType.IsGenericType
                && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // null許容型の時
            {
                dataTable.Columns.Add(columnName: columName, type: Nullable.GetUnderlyingType(property.PropertyType)!);
            }
            else // null非許容型の時
            {
                dataTable.Columns.Add(columnName: columName, type: property.PropertyType);
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

                var columName = isCamelToSnake ? CamelToSnakeCase(camelCase: property.Name) : property.Name;

                // 自動採番列は除外
                if (property.Name.Equals(value: identityColumnName, comparisonType: StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                row[columName] = property.GetValue(obj: entity) ?? DBNull.Value;
            }

            // 登録データ行追加
            dataTable.Rows.Add(row: row);
        }

        using (var bulkCopy = new SqlBulkCopy(connection: this.Connection, copyOptions: SqlBulkCopyOptions.Default, externalTransaction: tran ?? Transaction))
        {
            // タイムアウト指定
            bulkCopy.BulkCopyTimeout = 60;

            // テーブル名指定
            if (string.IsNullOrEmpty(value: tableName))
            {
                bulkCopy.DestinationTableName = typeof(T).Name;
            }
            else
            {
                bulkCopy.DestinationTableName = tableName;
            }


            // カラムマッピング設定
            foreach (DataColumn column in dataTable.Columns)
            {
                bulkCopy.ColumnMappings.Add(sourceColumn: column.ColumnName, destinationColumn: column.ColumnName);
            }

            // 一括Insert実行
            bulkCopy.WriteToServer(table: dataTable);
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
