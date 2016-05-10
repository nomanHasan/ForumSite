using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class StatusRepository : Repository
    {
        public static IEnumerable<Status> GetStatuses(string username) {

            string command = string.Format("select * from demouser.status where username = '{0}'",username);

            DataSet stDS = GetDataSet(command, "status");

            if (stDS.Tables["status"].Rows.Count <= 0) {
                yield return null;
            }

            foreach (DataRow row in stDS.Tables["status"].Rows) {
                yield return new Status
                {
                    Status_ID = Convert.ToInt32(row["status_ID"].ToString()),
                    Content = row["content"].ToString(),
                    Username = row["username"].ToString(),
                    User = UserRepository.getUser(row["username"].ToString()),
                    Status_Created = Convert.ToDateTime(row["status_created"].ToString()),
                    LikeCount = Convert.ToInt32(row["like_count"].ToString()),
                    DislikeCount = Convert.ToInt32(row["dislike_count"].ToString())
                };
            }

        }

        public static int StatusInsert(string content, string username) {

            OracleCommand command = new OracleCommand("insert into demouser.status(status_ID, content, username, status_created, like_count, dislike_count) values(demouser.status_seq.nextval,:1,:2,:3,0,0)");

            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = content;
            command.Parameters.Add(":2", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":3", OracleDbType.Date).Value = DateTime.Now;

            con.Open();

            int status = command.ExecuteNonQuery();

            con.Close();

            return status;

        }
    }
}