using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class UserRepository : Repository
    {
       

        public static User getUser(string username) {

            User returnUser = new User();

            string command = string.Format("select * from demouser.forum_user where username = '{0}'", username);

            DataSet usrDataSet = GetDataSet(command, "users");

            foreach (DataRow row in usrDataSet.Tables["users"].Rows)
            {

                returnUser =  new User
                {
                    Username = row["username"].ToString(),
                    Password = row["password"].ToString(),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Phone = row["phone"].ToString(),
                    Address = row["address"].ToString(),
                    Activity_Rating = (float)Convert.ToDouble(row["activity_rating"]),
                    Like_Rating = (float)Convert.ToDouble(row["like_rating"]),
                    Dislike_Rating = (float)Convert.ToDouble(row["dislike_rating"])
                };
            }

            return returnUser;
        }

        public static IEnumerable<User> getUsers() {

            DataSet usrDataSet = GetDataSet("select * from demouser.forum_user ", "users");


            foreach (DataRow row in usrDataSet.Tables["users"].Rows) {

                yield return new User
                {
                    Username = row["username"].ToString(),
                    Password = row["password"].ToString(),
                    Name = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Phone = row["phone"].ToString(),
                    Address = row["address"].ToString(),
                    Activity_Rating = (float)Convert.ToDouble(row["activity_rating"]),
                    Like_Rating = (float)Convert.ToDouble(row["like_rating"]),
                    Dislike_Rating = (float)Convert.ToDouble(row["dislike_rating"])
                };
            }
        }
        



        public static User CheckLogin(string usrname, string passwd)
        {
            User newUser = new User();
            string command = string.Format("select * from demouser.forum_user where username = '{0}' and password = '{1}'", usrname, passwd);

            DataSet usrDataSet = GetDataSet(command, "user");

            if (usrDataSet.Tables["user"].Rows.Count > 0)
            {
                foreach (DataRow row in usrDataSet.Tables["user"].Rows)
                {
                    newUser =  new User
                    {
                        Username = row["username"].ToString(),
                        Password = row["password"].ToString(),
                        Name = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Phone = row["phone"].ToString(),
                        Address = row["address"].ToString(),
                        Activity_Rating = (float)Convert.ToDouble(row["activity_rating"]),
                        Like_Rating = (float)Convert.ToDouble(row["like_rating"]),
                        Dislike_Rating = (float)Convert.ToDouble(row["dislike_rating"])
                    };
                }
                return newUser;
            }
            else {
                return null;
            }

        }


        public static bool UsernameExist(string username) {
            string sql1 = string.Format("select * from demouser.forum_user where username='{0}'", username);

            DataSet usDS = GetDataSet(sql1, "username");
            if (usDS.Tables["username"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static int InsertUser(string username, string password, string name, string email, string phone, string address) {

            OracleCommand command = new OracleCommand("insert into demouser.forum_user(username,password,name,email,phone,address,activity_rating,like_rating,dislike_rating) values(:1,:2,:3,:4,:5,:6,0,0,0)");
            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = username;
            command.Parameters.Add(":2", OracleDbType.Varchar2).Value = password;
            command.Parameters.Add(":3", OracleDbType.Varchar2).Value = name;
            command.Parameters.Add(":4", OracleDbType.Varchar2).Value = email;
            command.Parameters.Add(":5", OracleDbType.Varchar2).Value = phone;
            command.Parameters.Add(":6", OracleDbType.Varchar2).Value = address;

            con.Open();
            int status = command.ExecuteNonQuery();
            con.Close();

            return status;

        }

        public static int IsAdmin(string username) {
            string command = string.Format("select demouser.IsAdmin(username) from demouser.forum_user where username = '{0}'", username);

            int status = GetInt(command);

            return status;
        }

        public static IEnumerable<User> returnMembers(int groupID) {
            string command = string.Format("select * from demouser.forum_user where username in (select username from demouser.membership where group_ID = {0} and username not in (select username from demouser.admin where group_ID = {1}))", groupID, groupID);

            DataSet grDS = GetDataSet(command, "group");

            if (grDS.Tables["group"].Rows.Count <= 0) {
                yield return null;
            }

            foreach(DataRow row in grDS.Tables["group"].Rows) {

                string usrname = row["username"].ToString();

                User user = UserRepository.getUser(usrname);

                yield return user;
            }
        }

        public static IEnumerable<User> returnAdmins(int groupID, string username)
        {
            string command = string.Format("select * from demouser.forum_user where username in (select username from demouser.admin where group_ID = {0} and username !='{1}')", groupID, username);

            DataSet grDS = GetDataSet(command, "group");

            if (grDS.Tables["group"].Rows.Count <= 0)
            {
                yield return null;
            }

            foreach (DataRow row in grDS.Tables["group"].Rows)
            {

                string usrname = row["username"].ToString();

                User user = UserRepository.getUser(usrname);

                yield return user;
            }
        }



        public static bool IsMember(int groupID, string username) {
            string command = string.Format("select username from demouser.membership where groupID = {0} and username = '{1}'", groupID, username);

            DataSet mDS = GetDataSet(command, "member");

            if (mDS.Tables["member"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool IsAdmin(int groupID, string username)
        {
            string command = string.Format("select username from demouser.admin where group_ID = {0} and username = '{1}'", groupID, username);

            DataSet mDS = GetDataSet(command, "admin");

            if (mDS.Tables["admin"].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

    }
}