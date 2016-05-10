using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class ThreadRepository: Repository
    {

        

        public static IEnumerable<Thread> GetThreads(int groupID) {

            //IEnumerable<Thread> threadCollection = new List<Thread>();

            string command = string.Format("select * from demouser.forum_thread where group_ID = {0}",groupID);

            DataSet thDataSet = GetDataSet(command, "thread");

            if (thDataSet.Tables["thread"].Rows.Count <= 0)
            {
                yield return null;
            }

            
            foreach (DataRow row in thDataSet.Tables["thread"].Rows)
            {
                yield return new Thread
                {
                    Thread_ID = Convert.ToInt32(row["thread_ID"]),
                    Thread_Title = row["thread_title"].ToString(),
                    Thread_Desc = row["thread_desc"].ToString(),
                    Thread_Created = Convert.ToDateTime(row["thread_created"].ToString()),
                    Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                    Username = row["username"].ToString(),
                    User = UserRepository.getUser(row["username"].ToString())
                };
            }
            
        }

        public static Thread GetThreadByID(int threadID)
        {
            Thread returnThread = new Thread();

            string command = string.Format("select * from demouser.forum_thread where thread_ID = {0}", threadID);

            DataSet thDataSet = GetDataSet(command, "thread");

            if (thDataSet != null)
            {
                foreach (DataRow row in thDataSet.Tables["thread"].Rows)
                {
                    returnThread= new Thread
                    {
                        Thread_ID = Convert.ToInt32(row["thread_ID"]),
                        Thread_Title = row["thread_title"].ToString(),
                        Thread_Desc = row["thread_desc"].ToString(),
                        Thread_Created = Convert.ToDateTime(row["thread_created"].ToString()),
                        Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                        Username = row["username"].ToString(),
                        User = UserRepository.getUser(row["username"].ToString())
                    };
                }
                return returnThread;
            }

            return null;
        }

        public static int CheckThreadAccess2(int threadID, string username) {

            string command1 = string.Format("select group_ID from demouser.forum_thread where thread_ID = {0}", threadID);

            int groupID = GetInt(command1);

            if (groupID <= 0) return 0;

            string command2 = string.Format("select group_ID from demouser.membership where group_ID = {0} and username = '{1}'", groupID, username);

            int rez = GetInt(command2);

            if (rez > 0)
            {
                return 1;
            }
            else {
                return 0;
            }
        }

        public static bool CheckThreadAccess(string username, int threadID) {
            Thread returnThread = new Thread();

            string command = string.Format("select * from demouser.user_thread_access_view where thread_ID = {0} and username = '{1}'", threadID, username);

            DataSet thDataSet = GetDataSet(command, "thread");

            if (thDataSet.Tables["thread"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }

        }

        public static int InsertThread(string username, int groupID, string thread_title, string thread_desc) {

            OracleCommand command = new OracleCommand("insert into demouser.forum_thread(thread_ID, thread_title, thread_desc, group_ID, username, lock_status, thread_created) values(demouser.thread_seq.nextval,:1,:2,:3,:4,0,:5)");
            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = thread_title;
            command.Parameters.Add(":2", OracleDbType.Varchar2).Value = thread_desc;
            command.Parameters.Add(":3", OracleDbType.Int32).Value = groupID;
            command.Parameters.Add(":4", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":5", OracleDbType.Date).Value = DateTime.Now;


            con.Open();
            int status = command.ExecuteNonQuery();
            con.Close();

            return status;
        }

    }
}