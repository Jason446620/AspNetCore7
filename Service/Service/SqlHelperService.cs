using Service.IService;
using Service.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class SqlHelperService : ISqlHelperService
    {
        private readonly string _connectionString;
        public SqlHelperService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public dynamic ExecuteSQL(string SQL)
        {
            dynamic objValue;
            try
            {
                objValue = SqlHelper.ExecuteNonQuery(_connectionString, CommandType.Text, SQL);
            }
            catch (Exception ex)
            {
#pragma warning disable CA2200 // 再次引发以保留堆栈详细信息
                throw ex;
#pragma warning restore CA2200 // 再次引发以保留堆栈详细信息
            }
            return objValue;
        }

        public DataTable GetDataTableBySP(string SpName)
        {
            return SqlHelper.ExecuteDataTable(_connectionString, CommandType.StoredProcedure, SpName);
        }

        public DataTable GetDataTableBySPWithParam(string SpName, params object[] Params)
        {
            return SqlHelper.ExecuteDataTable(_connectionString, SpName, Params);
        }

        public DataTable GetDataTableBySPWithParam(string SpName, params SqlParameter[] Params)
        {
            return SqlHelper.ExecuteDataTable(_connectionString, SpName, Params);
        }

        public DataTable GetDataTableBySql(string Sql)
        {
            return SqlHelper.ExecuteDataTable(_connectionString, CommandType.Text, Sql);
        }

        public object GetScalarValueBySP(string SPName)
        {
            object objValue;
            try
            {
                objValue = SqlHelper.ExecuteScalar(_connectionString, CommandType.StoredProcedure, SPName);
            }
            catch (Exception ex)
            {
#pragma warning disable CA2200 // 再次引发以保留堆栈详细信息
                throw ex;
#pragma warning restore CA2200 // 再次引发以保留堆栈详细信息
            }
            return objValue;
        }

        public bool IsExist(string TableName, string Col, string value)
        {
            DataTable dtResults;
            bool isExist;
            try
            {
                string strSQL = $"SELECT TOP 1 {Col} FROM {TableName} WHERE {Col} = '{value}'";
                dtResults = SqlHelper.ExecuteDataTable(_connectionString, CommandType.Text, strSQL);
                isExist = dtResults.Rows.Count > 0 ? true : false;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }

        public bool IsExist(string TableName, string Col, int value)
        {
            DataTable dtResults;
            bool isExist;
            try
            {
                string strSQL = $"SELECT TOP 1 {Col} FROM {TableName} WHERE {Col} = {value}";
                dtResults = SqlHelper.ExecuteDataTable(_connectionString, CommandType.Text, strSQL);
                isExist = dtResults.Rows.Count > 0 ? true : false;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }

        public bool IsExist(string TableName, string WhereClause)
        {
            DataTable dtResults;
            bool isExist;
            try
            {
                string strSQL = $"SELECT TOP 1 * FROM {TableName} WHERE  {WhereClause}";
                dtResults = SqlHelper.ExecuteDataTable(_connectionString, CommandType.Text, strSQL);
                isExist = dtResults.Rows.Count > 0 ? true : false;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }
    }
}
