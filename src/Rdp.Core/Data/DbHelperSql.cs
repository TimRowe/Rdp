using Rdp.Core.Security;
using Rdp.Core.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rdp.Core.Data
{

    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    /// <remarks>数据访问抽象基础类</remarks>
    public abstract class DbHelperSql
    {
        #region "执行简单的SQL语句"
        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExists(string connectionString, string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(connectionString, sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }
        /// <summary>
        /// 取得某表的最大值加1
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="FieldName">列名称</param>
        /// <param name="TableName">表名称</param>
        /// <returns>最大值加1</returns>
        public static int GetMaxID(string connectionString, string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(connectionString, strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// Existses the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="strSql">The string SQL.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Exists(string connectionString, string strSql)
        {
            object obj = GetSingle(connectionString, strSql);
            int cmdresult = 0;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TabExists(string connectionString, string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object obj = GetSingle(connectionString, strsql);
            int cmdresult = 0;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Existses the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdParms">The command parms.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Exists(string connectionString, string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(connectionString, strSql, cmdParms);
            int cmdresult = 0;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region "执行简单SQL语句"
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string connectionString, string sqlString)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        connection.Close();
                        connection.Dispose();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        //Throw e
                        throw new SysDbException(e.Message, sqlString, null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 同一个数据库连接中，执行多条sql
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static bool ExecuteSql(string connectionString, string[] sqlStrings)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var sqlString in sqlStrings)
                    {
                        SqlCommand cmd = new SqlCommand(sqlString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    connection.Dispose();
                }

                catch (SqlException e)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new SysDbException(e.Message, sqlStrings.ToString(), null);
                }
                finally
                {
                    connection.Close();
                }

                return true;
            }
        }

        /// <summary>
        /// Executes the SQL by time.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="SQLString">The SQL string.</param>
        /// <param name="Times">The times.</param>
        /// <returns>System.Int32.</returns>
        public static int ExecuteSqlByTime(string connectionString, string SQLString, int Times)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        connection.Close();
                        connection.Dispose();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        //Throw e
                        throw new SysDbException(e.Message, SQLString, null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行Sql和Oracle滴混合事务
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="list">SQL命令行列表</param>
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        /// <exception cref="System.Exception">
        /// 违背要求 + myDE.CommandText + 必须符合select count(..的格式
        /// or
        /// SQL:违背要求 + myDE.CommandText + 必须符合select count(..的格式
        /// or
        /// SQL:违背要求 + myDE.CommandText + 返回值必须大于0
        /// or
        /// SQL:违背要求 + myDE.CommandText + 返回值必须等于0
        /// or
        /// SQL:违背要求 + myDE.CommandText + 必须有影响行
        /// </exception>
        public static int ExecuteSqlTran(string connectionString, List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (CommandInfo myDE in list)
                    {
                        string cmdText = myDE.CommandText;
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                        PrepareCommand(cmd, connection, tx, cmdText, cmdParms);
                        if (myDE.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (isHave)
                            {
                                myDE.OnSolicitationEvent();
                            }
                        }
                        if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须大于0");
                            }
                            if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须等于0");
                            }
                            continue;
                        }
                        int val = cmd.ExecuteNonQuery();
                        if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                        {
                            tx.Rollback();
                            throw new Exception("SQL:违背要求" + myDE.CommandText + "必须有影响行");
                        }
                        cmd.Parameters.Clear();
                    }
                    //Dim oraConnectionString As String = PubConstant.GetConnectionString("ConnectionStringPPC")
                    //Dim res As Boolean = OracleHelper.ExecuteSqlTran(oraConnectionString, oracleCmdSqlList)
                    //If Not res Then
                    //    tx.Rollback()
                    //    Throw New Exception("Oracle执行失败")
                    //End If
                    tx.Commit();
                    connection.Close();
                    connection.Dispose();
                    return 1;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    tx.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw e;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="SQLStringList">多条SQL语句</param>
        /// <returns>System.Int32.</returns>
        public static int ExecuteSqlTran(string connectionString, List<String> SQLStringList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    int n = 0;
                    while (n < SQLStringList.Count)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                        System.Math.Max(System.Threading.Interlocked.Increment(ref n), n - 1);
                    }
                    tx.Commit();
                    connection.Close();
                    connection.Dispose();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    connection.Close();
                    connection.Dispose();
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();

                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string connectionString, string sqlString, string content)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return rows;
                }
                catch (SqlException e)
                {
                    connection.Close();
                    connection.Dispose();
                    //Throw e
                    throw new SysDbException(e.Message, sqlString, null);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string connectionString, string SQLString, string content)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    cmd.Dispose();
                    connection.Close();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (SqlException e)
                {
                    cmd.Dispose();
                    connection.Close();
                    //Throw e
                    throw new SysDbException(e.Message, SQLString, null);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string connectionString, string strSQL, byte[] fs)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                    return rows;
                }
                catch (SqlException e)
                {
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                    throw new SysDbException(e.Message, strSQL, null);
                    //Throw e
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string connectionString, string sqlString)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw ex
                        throw new SysDbException(ex.Message, sqlString, null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">The SQL string.</param>
        /// <param name="Times">The times.</param>
        /// <returns>System.Object.</returns>
        public static object GetSingle(string connectionString, string sqlString, int Times)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (SqlException e)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw e
                        throw new SysDbException(e.Message, sqlString, null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string connectionString, string strSQL)
        {
            SqlConnection connection = CreateDbConnection(connectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                //Throw e
                throw new SysDbException(e.Message, strSQL, null);
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string connectionString, string sqlString)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter cmd = new SqlDataAdapter(sqlString, connection);
                    cmd.Fill(ds, "ds");
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                catch (SqlException ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Throw ex
                    throw new SysDbException(ex.Message, sqlString, null);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                return ds;
            }
        }
        /// <summary>
        /// Queries the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">The SQL string.</param>
        /// <param name="times">The times.</param>
        /// <returns>DataSet.</returns>
        public static DataSet Query(string connectionString, string sqlString, int times)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter cmd = new SqlDataAdapter(sqlString, connection);
                    cmd.SelectCommand.CommandTimeout = times;
                    cmd.Fill(ds, "ds");
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                catch (SqlException ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Throw ex
                    throw new SysDbException(ex.Message, sqlString, null);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                return ds;
            }
        }
        #endregion
        #region "执行带参数的SQL语句"

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="cmdParms">The command parms.</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string connectionString, string sqlString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        throw new SysDbException(e.Message, sqlString, cmdParms);
                        //Throw e
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(string connectionString, Dictionary<string, object> sqlStringList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (KeyValuePair<string, object> myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDe.Value;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException e)
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        throw new SysDbException(e.Message, sqlStringList.ToString(), null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(string connectionString, Hashtable sqlStringList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDe.Value;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException e)
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw
                        throw new SysDbException(e.Message, sqlStringList.ToString(), null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="cmdList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <returns>System.Int32.</returns>
        public static int ExecuteSqlTran(string connectionString, System.Collections.Generic.List<CommandInfo> cmdList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                object obj = cmd.ExecuteScalar();
                                bool isHave = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHave = false;
                                }
                                isHave = Convert.ToInt32(obj) > 0;
                                if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        return count;
                    }
                    catch (SqlException e)
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw
                        throw new SysDbException(e.Message, cmdList.ToString(), null);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(string connectionString, System.Collections.Generic.List<CommandInfo> SQLStringList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (SqlException e)
                    {
                        trans.Rollback();
                        //Throw
                        throw new SysDbException(e.Message, SQLStringList.ToString(), null);
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(string connectionString, Hashtable sqlStringList)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (SqlException e)
                    {
                        trans.Rollback();
                        //Throw
                        throw new SysDbException(e.Message, sqlStringList.ToString(), null);
                    }
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <param name="cmdParms">The command parms.</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string connectionString, string sqlString, params SqlParameter[] cmdParms)
        {

            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception e)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw e
                        throw new SysDbException(e.Message, sqlString, cmdParms);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms">The command parms.</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string connectionString, string sqlString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = CreateDbConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
            {
                //Throw e
                throw new SysDbException(e.Message, sqlString, cmdParms);
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms">The command parms.</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string connectionString, string sqlString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException ex)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        //Throw ex
                        throw new SysDbException(ex.Message, sqlString, cmdParms);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// Prepares the command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="cmdParms">The command parms.</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection connection, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            cmd.Connection = connection;
            cmd.CommandText = cmdText;
            if ((trans != null))
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if ((cmdParms != null))
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && parameter.Value == null)
                    {
                        if (parameter.DbType == DbType.String)
                        {
                            parameter.Value = "";
                        }
                        else
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion
        #region "存储过程操作"

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDa = new SqlDataAdapter();
                try
                {
                    connection.Open();

                    sqlDa.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDa.Fill(dataSet, tableName);
                    connection.Close();
                    connection.Dispose();
                    return dataSet;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Throw
                    throw new SysDbException(ex.Message, storedProcName, (SqlParameter[])parameters);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="Times">连接超时</param>
        /// <returns>DataSet.</returns>
        public static DataSet RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter();
                    sqlDa.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDa.SelectCommand.CommandTimeout = Times;
                    sqlDa.Fill(dataSet, tableName);
                    connection.Close();
                    connection.Dispose();
                    return dataSet;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Throw ex
                    throw new SysDbException(ex.Message, storedProcName, (SqlParameter[])parameters);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if ((parameter != null))
                {
                    try
                    {
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                    catch (Exception ex)
                    {
                        throw new SysDbException(ex.Message, storedProcName, (SqlParameter[])parameters);
                    }
                }
            }
            return command;
        }
        /// <summary>
        /// 执行存储过程，返回影响的行数
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns>System.Int32.</returns>
        public static int RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, ref int rowsAffected)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                try
                {
                    int result = 0;
                    connection.Open();
                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    return result;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Return ex.ToString()
                    throw new SysDbException(ex.Message, storedProcName, (SqlParameter[])parameters);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        public static int RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = CreateDbConnection(connectionString))
            {
                try
                {
                    int result = 0;
                    connection.Open();
                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    return result;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    //Return ex.ToString()
                    //Throw
                    throw new SysDbException(ex.Message, storedProcName, (SqlParameter[])parameters);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            var sqlparam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, string.Empty);
            sqlparam.Direction = ParameterDirection.ReturnValue;
            sqlparam.IsNullable = false;
            command.Parameters.Add(sqlparam);
            return command;
        }
        #endregion

        #region "高级数据库操作(分页)"
        /// <summary>
        /// 根据查询语句返回DataTable
        /// </summary>
        /// <param name="str">查询语句</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>
        /// 2017-03-28   Tim   Add
        /// </history>
        public static DataTable QueryByPage(ref PageParam pageParam)
        {
            var execSql = string.Format(
                "{0} SELECT {1} FROM {2} {3} {4} {5};",
                pageParam.PreSql,
                pageParam.FieldList,
                pageParam.TableName,
                string.IsNullOrEmpty(pageParam.Where)? "" : " WHERE " + pageParam.Where,
                string.IsNullOrEmpty(pageParam.Order.Trim()) ? "" : " ORDER BY " + pageParam.Order,
                string.IsNullOrEmpty(pageParam.Order.Trim()) ? "" : " OFFSET " + (pageParam.pageSize * (pageParam.pageIndex - 1)).ToString() + " ROWS  FETCH NEXT " + pageParam.pageSize.ToString() + " ROWS ONLY "
                );

            var dt = Query(DefaultQueryConn, execSql, pageParam.SqlParamList!=null? pageParam.SqlParamList.ToArray():null).Tables[0];

            if (pageParam.pageIndex == 1 && dt.Rows.Count < pageParam.pageSize)
            {
                pageParam.TotalCount = dt.Rows.Count;
            }
            else if(pageParam.TotalCount == 0 || pageParam.pageIndex == 1)
            {
                var countSql = string.Format(
                "{0} SELECT COUNT(1) C   FROM {1}  {2} ",
                string.IsNullOrEmpty(pageParam.PreSql) ? "" : pageParam.PreSql,
                pageParam.TableName,
                string.IsNullOrEmpty(pageParam.Where) ? "" : " WHERE " + pageParam.Where);

                pageParam.TotalCount = (int)Query(DefaultQueryConn, countSql, pageParam.SqlParamList.ToArray()).Tables[0].Rows[0]["C"];
            }

            return dt;
        }
        #endregion

        public static SqlConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// 当连接建立以后触发此函数
        /// </summary>
        //public static Func<SqlConnection, SqlConnection> OnConnectionCreated;


        #region 默认的连接字符串
        //todo
        public static string DefaultQueryConn = string.Empty;
        public static string DefaultUpdateConn = string.Empty;
        public static string CouUpdate = string.Empty;
        public static string CouQuery = string.Empty;
        #endregion

    }
}

