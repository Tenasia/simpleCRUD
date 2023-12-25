
﻿using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;

namespace OnePhp.HRIS.Core.Data
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection;
        private string m_sConnectionString = string.Empty;
        private MySqlConnection m_oConn = null;
        private MySqlTransaction m_oTrans = null;
        private int m_nCommandTimeout = 0;

        private Config _config = new Config();

        public AppDb()
        {
            Connection = new MySqlConnection(_config.ConnectionString);
        }

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Gets or sets the command timeout value
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return m_nCommandTimeout;
            }
            set
            {
                m_nCommandTimeout = value;
            }
        }

        /// <summary>
        /// Returns a valid MySql connectionstring
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return m_sConnectionString;
            }
        }

        /// <summary>
        /// Opens a connection to the MySql DB
        /// </summary>
        public void Open()
        {
            m_oConn = Connection;
            // m_oConn.ConnectionString = ConnectionString;
            m_oConn.Open();
        }

        /// <summary>
        /// Close the connection to the MySql db
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
        /// Executes a query against an MySql DB and returns a DataTable 
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
                    DataSet oDataSet = new DataSet();

                    using (MySqlDataAdapter oDataAdp = new MySqlDataAdapter(sQuery, m_oConn))
                    {
                        oDataAdp.Fill(oDataSet);
                        oDataTable = oDataSet.Tables[oDataSet.Tables.Count - 1];
                    }
                    return oDataTable;

                }
                else throw new DataException("Connection not open");
            }
            else throw new DataException("Connection not open");

        }


        /// <summary>
        /// Executes a query agains an MySql DB without expecting results
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>number of rows affected</returns>
        public int Execute(string sql)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    using (MySqlCommand oCmd = new MySqlCommand())
                    {
                        oCmd.CommandText = sql;
                        oCmd.Connection = m_oConn;
                        oCmd.CommandTimeout = m_nCommandTimeout;

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
        /// Executes a query agains an MySql DB without expecting results
        /// </summary>
        /// <param name="sql">Sql query</param>
        /// <returns>number of rows affected</returns>
        public int ExecuteScalar(string sql)
        {
            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {

                    using (MySqlCommand oCmd = new MySqlCommand())
                    {
                        oCmd.CommandText = sql;
                        oCmd.Connection = m_oConn;
                        oCmd.CommandTimeout = m_nCommandTimeout;

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

        public int ExecuteCommandNonQuery(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(0, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable);
        }

        public int ExecuteCommandNonQuery(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, CommandType commandType)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(0, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, commandType);
        }

        public int ExecuteCommandScalar(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(1, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable);
        }

        public int ExecuteCommandScalar(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, CommandType commandType)
        {
            DataTable oDataTable = new DataTable();
            return ExecuteCommand(1, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oDataTable, commandType);
        }

        public int ExecuteCommandReader(string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable)
        {
            return ExecuteCommand(2, sCommand, asParams, atParamTypes, aoValues, out nRowsAffected, ref oTable);
        }

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
        private int ExecuteCommand(int nMode, string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable)
        {

            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {
                    using (MySqlCommand oCmd = new MySqlCommand())
                    {
                        oCmd.CommandText = sCommand;
                        //oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandType = CommandType.Text;
                        oCmd.CommandTimeout = m_nCommandTimeout;

                        for (int i = 0; i < asParams.Length; i++)
                        {
                            MySqlParameter oParam = new MySqlParameter("@" + asParams[i], aoValues[i]);
                            oParam.DbType = atParamTypes[i];
                            //if (anParamSizes[i] != 0)
                            //{
                            //    oParam.Size = anParamSizes[i];
                            //}
                            oCmd.Parameters.Add(oParam);
                        }
                        //add return value parameter
                        MySqlParameter oRetParam = new MySqlParameter("RETURN_VALUE", 0);
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
                                using (MySqlDataAdapter oAdapter = new MySqlDataAdapter(oCmd))
                                {
                                    if (oTable != null)
                                    {
                                        oAdapter.Fill(oTable);
                                        nRowsAffected = 0;
                                    }
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

        private int ExecuteCommand(int nMode, string sCommand, string[] asParams, DbType[] atParamTypes, object[] aoValues, out int nRowsAffected, ref DataTable oTable, CommandType commandType)
        {

            if (m_oConn != null)
            {
                if (m_oConn.State == ConnectionState.Open)
                {
                    using (MySqlCommand oCmd = new MySqlCommand())
                    {
                        oCmd.CommandText = sCommand;
                        oCmd.CommandType = commandType;
                        oCmd.CommandTimeout = m_nCommandTimeout;

                        for (int i = 0; i < asParams.Length; i++)
                        {
                            MySqlParameter oParam = new MySqlParameter("@" + asParams[i], aoValues[i]);
                            oParam.DbType = atParamTypes[i];
                            //if (anParamSizes[i] != 0)
                            //{
                            //    oParam.Size = anParamSizes[i];
                            //}
                            oCmd.Parameters.Add(oParam);
                        }
                        //add return value parameter
                        MySqlParameter oRetParam = new MySqlParameter("RETURN_VALUE", 0);
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
                                MySqlDataAdapter oAdapter = new MySqlDataAdapter(oCmd);
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
                m_oConn.Close();
                m_oConn.Dispose();
            }
        }

        #endregion

    }
}