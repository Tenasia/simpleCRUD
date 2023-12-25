using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace OnePhp.HRIS.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLServerDB : IDisposable
    {
        private static string m_sConnectionString = String.Empty;
        private SqlConnection m_oConn = null;
        private SqlTransaction m_oTrans = null;

        private Config _config = new Config();

        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerDB()
        {
            m_sConnectionString = _config.SentryConnectionString;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerDB(string sConnectionString)
        {
            m_sConnectionString = sConnectionString;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerDB(string sServer, string sDatabase, bool bWinAuthentication, string sUsername, string sPassword)
        {
            SqlConnectionStringBuilder oConnBuilder = new SqlConnectionStringBuilder();
            oConnBuilder.DataSource = sServer;
            oConnBuilder.InitialCatalog = sDatabase;
            if (bWinAuthentication)
            {
                oConnBuilder.IntegratedSecurity = true;
            }
            else
            {
                oConnBuilder.UserID = sUsername;
                oConnBuilder.Password = sPassword;
            }
            m_sConnectionString = oConnBuilder.ConnectionString;


        }

        /// <summary>
        /// Returns a valid Sql connectionstring
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return m_sConnectionString;
            }
        }

        /// <summary>
        /// Opens a connection to the Sql DB
        /// </summary>
        public void Open()
        {
            m_oConn = new SqlConnection();
            m_oConn.ConnectionString = ConnectionString;
            m_oConn.Open();
        }

        /// <summary>
        /// Close the connection to the Sql db
        /// </summary>
        public void Close()
        {
            if (m_oConn != null)
            {
                m_oConn.Close();
                m_oConn = null;
            }
        }


        /// <summary>
        /// Executes a query against an Sql DB and returns a DataTable 
        /// </summary>
        /// <param name="sQuery">SQL Query</param>
        /// <returns>DataTable containing the results of the query</returns>
        public DataTable Fetch(string sQuery)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {
                    DataTable oDataTable = new DataTable();

                    using (SqlDataAdapter oDataAdp = new SqlDataAdapter(sQuery, m_oConn))
                    {
                        oDataAdp.Fill(oDataTable);
                    }
                    return oDataTable;
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");
        }

        /// <summary>
        /// Fetches the int32.
        /// </summary>
        /// <param name="sQuery">The s query.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public int FetchInt32(string sQuery, string column)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    DataTable oDataTable = new DataTable();

                    using (SqlDataAdapter oDataAdp = new SqlDataAdapter(sQuery, m_oConn))
                    {
                        oDataAdp.Fill(oDataTable);
                    }

                    int x = default(Int32);
                    if (oDataTable.Rows.Count > 0)
                    {
                        x = Convert.ToInt32(oDataTable.Rows[0][column]);
                    }
                    return x;
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");

        }

        public string FetchString(string sQuery, string column)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    DataTable oDataTable = new DataTable();

                    using (SqlDataAdapter oDataAdp = new SqlDataAdapter(sQuery, m_oConn))
                    {
                        oDataAdp.Fill(oDataTable);
                    }

                    return oDataTable.Rows[0][column].ToString();
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");

        }

        /// <summary>
        /// Executes a query agains an Sql DB without expecting results
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>number of rows affected</returns>
        public int Execute(string sql)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        oCmd.CommandText = sql;
                        oCmd.Connection = m_oConn;
                        if (m_oTrans != null)
                        {
                            oCmd.Transaction = m_oTrans;
                        }
                        return oCmd.ExecuteNonQuery();
                    }
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");

        }

        /// <summary>
        /// Executes a query agains an Sql DB without expecting results
        /// </summary>
        /// <param name="sql">Sql query</param>
        /// <returns>number of rows affected</returns>
        public int ExecuteScalar(string sql)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        oCmd.CommandText = sql;
                        oCmd.Connection = m_oConn;
                        if (m_oTrans != null)
                        {
                            oCmd.Transaction = m_oTrans;
                        }
                        return Convert.ToInt32(oCmd.ExecuteScalar());
                    }
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");

        }


        /// <summary>
        /// Executes the command non query.
        /// </summary>
        /// <param name="sCommand">The s command.</param>
        /// <param name="asParams">As params.</param>
        /// <param name="atParamTypes">At param types.</param>
        /// <param name="aoValues">The ao values.</param>
        /// <param name="nRowsAffected">The n rows affected.</param>
        /// <returns></returns>
        public int ExecuteCommandNonQuery(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(0, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Executes the command non query.
        /// </summary>
        /// <param name="sCommand">The s command.</param>
        /// <param name="asParams">As params.</param>
        /// <param name="atParamTypes">At param types.</param>
        /// <param name="aoValues">The ao values.</param>
        /// <param name="nRowsAffected">The n rows affected.</param>
        /// <param name="eType">Type of the e.</param>
        /// <returns></returns>
        public int ExecuteCommandNonQuery(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, CommandType commandType)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(0, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, commandType);
        }

        /// <summary>
        /// Executes the command scalar.
        /// </summary>
        /// <param name="sCommand">The s command.</param>
        /// <param name="asParams">As params.</param>
        /// <param name="atParamTypes">At param types.</param>
        /// <param name="aoValues">The ao values.</param>
        /// <param name="nRowsAffected">The n rows affected.</param>
        /// <returns></returns>
        public int ExecuteCommandScalar(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(1, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, CommandType.StoredProcedure);
        }

        public int ExecuteCommandScalar(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, CommandType commandType)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(1, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, commandType);
        }

        /// <summary>
        /// Executes the command reader.
        /// </summary>
        /// <param name="sCommand">The s command.</param>
        /// <param name="asParams">As params.</param>
        /// <param name="atParamTypes">At param types.</param>
        /// <param name="aoValues">The ao values.</param>
        /// <param name="nRowsAffected">The n rows affected.</param>
        /// <param name="oTable">The o table.</param>
        /// <returns></returns>
        public int ExecuteCommandReader(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable)
        {
            return ExecuteCommand(2, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oTable, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Executes the command reader.
        /// </summary>
        /// <param name="sCommand">The s command.</param>
        /// <param name="asParams">As params.</param>
        /// <param name="atParamTypes">At param types.</param>
        /// <param name="aoValues">The ao values.</param>
        /// <param name="nRowsAffected">The n rows affected.</param>
        /// <param name="oTable">The o table.</param>
        /// <param name="eType">Type of the e.</param>
        /// <returns></returns>
        public int ExecuteCommandReader(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable, CommandType commandType)
        {
            return ExecuteCommand(2, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oTable, commandType);
        }

        /// <summary>
        /// Executes a parameterized command 
        /// </summary>
        /// <param name="sCommand">parameterized query or stored procedure name</param>
        /// <param name="asParams">parameter names</param>
        /// <param name="atParamTypes">parameter types</param>
        /// <param name="anParamSizes"></param>
        /// <param name="aoValues">values</param>
        /// <returns>integer result of the query</returns>
        private int ExecuteCommand(int nMode, string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable, CommandType commandType)
        {

            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        oCmd.CommandText = sCommand;
                        oCmd.CommandType = commandType;

                        for (int i = 0; i < asParams.Length; i++)
                        {
                            SqlParameter oParam = new SqlParameter(asParams[i], aoValues[i]);
                            oParam.DbType = atParamTypes[i];
                            if (atParamTypes[i] == DbType.Binary)
                            {
                                oParam.SqlDbType = SqlDbType.Image;
                            }
                            //if (anParamSizes[i] != 0)
                            //{
                            //    oParam.Size = anParamSizes[i];
                            //}
                            oCmd.Parameters.Add(oParam);
                        }
                        //add return value parameter
                        SqlParameter oRetParam = new SqlParameter("RETURN_VALUE", DBNull.Value);
                        oRetParam.Direction = ParameterDirection.ReturnValue;

                        oCmd.Parameters.Add(oRetParam);

                        oCmd.Connection = m_oConn;
                        if (m_oTrans != null)
                        {
                            oCmd.Transaction = m_oTrans;
                        }

                        nRowsAffected = 0;
                        switch (nMode)
                        {
                            case 0: //NonQuery
                                nRowsAffected = oCmd.ExecuteNonQuery();
                                break;
                            case 1: //Scalar
                                nRowsAffected = oCmd.ExecuteNonQuery();
                                break;
                            case 2: //Reader
                                SqlDataAdapter oAdapter = new SqlDataAdapter(oCmd);
                                if (oTable != null)
                                {
                                    oAdapter.Fill(oTable);
                                    nRowsAffected = 0;
                                }
                                break;
                        }

                        return Convert.ToInt32(oCmd.Parameters["RETURN_VALUE"].Value);
                    }
                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");
        }

        // <summary>
        /// Begins a Transaction
        /// </summary>
        public void BeginTransaction()
        {
            if (m_oConn != null)
            {
                m_oTrans = m_oConn.BeginTransaction();
            }
            else throw new DataException("Connection not open");

        }

        /// <summary>
        /// Commits an active transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (m_oTrans != null)
            {
                m_oTrans.Commit();
                m_oTrans = null;
            }
            else throw new DataException("Not in transcation");
        }

        /// <summary>
        /// Rollbacks an active transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (m_oTrans != null)
            {
                m_oTrans.Rollback();
                m_oTrans = null;
            }
            else throw new DataException("Not in transcation");
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_oConn != null)
            {
                m_oConn.Dispose();
            }
        }

        #endregion
    }
}
