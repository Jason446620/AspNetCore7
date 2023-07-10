using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ISqlHelperService
    {
        DataTable GetDataTableBySP(string SpName);
        DataTable GetDataTableBySPWithParam(string SpName, params object[] Params);
        DataTable GetDataTableBySPWithParam(string SpName, params SqlParameter[] Params);
        DataTable GetDataTableBySql(string Sql);
        object GetScalarValueBySP(string SPName);
        dynamic ExecuteSQL(string SQL);
        bool IsExist(string TableName, string Col, string value);
        bool IsExist(string TableName, string Col, int value);
        bool IsExist(string TableName, string WhereClause);
    }
}