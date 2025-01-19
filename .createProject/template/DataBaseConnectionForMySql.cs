using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using Dapper;

namespace PROJECT_NAME.Utils;

public class DataBaseConnectionForMySql : IDisposable
{
    /// <summary>
    /// コネクションインスタンス
    /// </summary>
    private MySqlConnection Connection { get; set; }

    /// <summary>
    /// トランザクションインスタンス
    /// </summary>
    public MySqlTransaction? Transaction { get; set; } = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DataBaseConnectionForMySql()
    {
        // インスタンス生成
        this.Connection = new MySqlConnection(connectionString: EnvConsts.ConnectionString);

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
    public IEnumerable<T> Query<T>(string sql, object? param = null, MySqlTransaction? tran = null)
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
    public IEnumerable<T> Query<T>(T columsType, string sql, object? param = null, MySqlTransaction? tran = null)
    {
        return Connection.Query<T>(sql: sql, param: param, transaction: tran ?? Transaction);
    }

    /// <summary>
    /// INSERT,UPDATE,DELETE用
    /// </summary>
    /// <param name="sql">生SQL文</param>
    /// <param name="paramList">パラメータリスト</param>
    /// <param name="tran">トランザクション</param>
    /// <returns></returns>
    public void Execute(string sql, List<MySqlParameter>? paramList = null, MySqlTransaction? tran = null)
    {
        if (paramList == null)
        {
            Connection.Execute(sql: sql, transaction: tran ?? Transaction);
        }
        else
        {
            using (var command = new MySqlCommand(cmdText: sql, connection: this.Connection))
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
    public void BulkInsert<T>(IEnumerable<T> entities, string tableName = "", MySqlTransaction? tran = null, bool isCamelToSnake = true)
    {
        //TODO: 一括Insertの実装
    }

    /// <summary>
    /// 自動採番列を持つテーブルへの一括Insert
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <param name="identityColumnName"></param>
    /// <param name="transaction"></param>
    public void BulkInsertForIdentify<T>(IEnumerable<T> entities, string tableName = "", string? identityColumnName = null, MySqlTransaction? tran = null, bool isCamelToSnake = true)
    {
        //TODO: 一括Insertの実装
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
