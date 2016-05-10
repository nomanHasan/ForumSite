using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Helper
{
    public class OracleDBHelper
    {
        private static OracleDbType GetOracleDbType(object o)
        {
            if (o is string) return OracleDbType.Varchar2;
            if (o is DateTime) return OracleDbType.Date;
            if (o is Int64) return OracleDbType.Int64;
            if (o is Int32) return OracleDbType.Int32;
            if (o is Int16) return OracleDbType.Int16;
            if (o is sbyte) return OracleDbType.Byte;
            if (o is byte) return OracleDbType.Int16;
            if (o is decimal) return OracleDbType.Decimal;
            if (o is float) return OracleDbType.Single;
            if (o is double) return OracleDbType.Double;
            if (o is byte[]) return OracleDbType.Blob;

            return OracleDbType.Varchar2;
        }
    }
}