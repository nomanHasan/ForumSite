using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class CommentRepository : Repository
    {

        public static IEnumerable<Comment> GetComments(int threadID)
        {

            //IEnumerable<Thread> threadCollection = new List<Thread>();

            string command = string.Format("select * from demouser.forum_comment where thread_ID = {0} order by comment_ID asc", threadID);

            DataSet thDataSet = GetDataSet(command, "comments");

            if(thDataSet.Tables["comments"].Rows.Count <= 0)
            {
                yield return null;
            }

            
            foreach (DataRow row in thDataSet.Tables["comments"].Rows)
            {
                yield return new Comment
                {
                    Comment_ID = Convert.ToInt32(row["comment_ID"]),
                    Comment_Content = row["comment_content"].ToString(),
                    Thread_ID = Convert.ToInt32(row["thread_ID"].ToString()),
                    Username = row["username"].ToString(),
                    Comment_Created = Convert.ToDateTime(row["comment_created"].ToString()),
                    Reply_Comment_ID = Convert.ToInt32(row["reply_comment_ID"].ToString()),
                    User = UserRepository.getUser(row["username"].ToString()),
                    Like_Count = Convert.ToInt32(row["like_count"].ToString()),
                    Dislike_Count = Convert.ToInt32(row["dislike_count"].ToString()),
                };
            }
            

            yield return null;
        }

        public static int NextCommentID() {
            string command = "select max(comment_ID) from demouser.forum_comment";

            int number = GetInt(command);

            return number +1;
        }

        public static int InsertComment(int threadID, string comment_content, string username, int reply_comment_ID, DateTime comment_created) {

            OracleCommand command = new OracleCommand("insert into demouser.forum_comment(comment_ID, comment_content, thread_ID, username, reply_comment_ID, comment_created, like_count, dislike_count) values(:1, :2, :3, :4, :5, :6, 0, 0)");

            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Int32).Value = NextCommentID();
            command.Parameters.Add(":2", OracleDbType.Varchar2).Value = comment_content;
            command.Parameters.Add(":3", OracleDbType.Int32).Value = threadID;
            command.Parameters.Add(":4", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":5", OracleDbType.Int32).Value = reply_comment_ID;
            command.Parameters.Add(":6", OracleDbType.Date).Value = comment_created;

            con.Open();

            int status = command.ExecuteNonQuery();

            con.Close();

            return status;
        }


        public static bool CheckCommentAuthor(int commentID, string username)
        {

            string command1 = string.Format("select comment_ID from demouser.forum_comment where username = '{0}' and comment_ID = {1}", username,commentID);

            int rez = GetInt(command1);

            if (rez > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }


       


        public static int LikeComment(int commentID, string username)
        {

            if (DislikeExists(commentID, username)) {
                DeleteDislike(commentID, username);
            }

            string sql = string.Format("select * from demouser.comment_like where username = '{0}' and comment_ID = {1}", username, commentID);

            DataSet cmDataSet = GetDataSet(sql, "comment");

            if (cmDataSet.Tables["comment"].Rows.Count > 0) {
                string del = string.Format("delete from demouser.comment_like where username = '{0}' and comment_ID = {1}", username, commentID);

                int s = ExecuteNonQuery(del);

                return s;
            }

            OracleCommand command = new OracleCommand("insert into demouser.comment_like(username, comment_ID) values(:1,:2)");

            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":2", OracleDbType.Int32).Value = commentID;

            con.Open();

            int status = command.ExecuteNonQuery();

            con.Close();

            return status;
        }

        public static int DislikeComment(int commentID, string username)
        {
            if (LikeExists(commentID, username)) {
                DeleteLike(commentID, username);
            }

            string sql = string.Format("select * from demouser.comment_dislike where username = '{0}' and comment_ID = {1}", username, commentID);

            DataSet cmDataSet = GetDataSet(sql, "comment");

            if (cmDataSet.Tables["comment"].Rows.Count > 0)
            {
                string del = string.Format("delete from demouser.comment_dislike where username = '{0}' and comment_ID = {1}", username, commentID);

                int s = ExecuteNonQuery(del);

                return s;
            }


            OracleCommand command = new OracleCommand("insert into demouser.comment_dislike(username, comment_ID) values(:1,:2)");

            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":2", OracleDbType.Int32).Value = commentID;

            con.Open();

            int status = command.ExecuteNonQuery();

            con.Close();

            return status;
        }


        public static bool DislikeExists(int commentID, string username)
        {
            string sql = string.Format("select * from demouser.comment_dislike where username = '{0}' and comment_ID = {1}", username, commentID);

            DataSet cmDataSet = GetDataSet(sql, "comment");

            if (cmDataSet.Tables["comment"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool LikeExists(int commentID, string username)
        {
            string sql = string.Format("select * from demouser.comment_like where username = '{0}' and comment_ID = {1}", username, commentID);

            DataSet cmDataSet = GetDataSet(sql, "comment");

            if (cmDataSet.Tables["comment"].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckCommentAccess(int commentID, string username) {
            string sql = string.Format("select * from demouser.user_comment_access_view where comment_ID = {0} and username = '{1}'", commentID, username);

            DataSet cmDS = GetDataSet(sql,"comment");

            if (cmDS.Tables["comment"].Rows.Count > 0) {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int DeleteDislike(int commentID, string username) {
            string del = string.Format("delete from demouser.comment_dislike where username = '{0}' and comment_ID = {1}", username, commentID);

            int s = ExecuteNonQuery(del);

            return s;
        }

        public static int DeleteLike(int commentID, string username)
        {
            string del = string.Format("delete from demouser.comment_like where username = '{0}' and comment_ID = {1}", username, commentID);

            int s = ExecuteNonQuery(del);

            return s;
        }

    }
}