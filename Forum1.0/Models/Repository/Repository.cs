using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class Repository
    {
        public static string connStr = "User Id=system; password=demopass;" + "Data Source=:1521/XE";

        public static OracleConnection con = new OracleConnection(connStr);

        public static DataSet GetDataSet(string sqlString, string tableName)
        {
            OracleDataAdapter adapter;
            DataSet returnDataSet = new DataSet();
            adapter = new OracleDataAdapter(sqlString, connStr);
            adapter.Fill(returnDataSet, tableName);

            return returnDataSet;
        }

        public static int GetInt(string sqlString) {

            int returnNumber = 0;

            con.Open();

            OracleCommand cmd = new OracleCommand(sqlString, con);

            returnNumber = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            return returnNumber;
        }

        public static int ExecuteNonQuery(string sql)
        {
            OracleCommand command = new OracleCommand(sql);

            command.Connection = con;

            con.Open();
            int status = command.ExecuteNonQuery();
            con.Close();

            return status;
        }

    }
}