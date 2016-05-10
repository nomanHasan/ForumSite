using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Forum1._0.Models.Repository
{
    public class GroupRepository : Repository
    {



        public static IEnumerable<Models.Group> getGroups(string username)
        {

            string command = string.Format("select * from demouser.forum_group where group_ID in (select group_ID from demouser.membership where username = '{0}')", username);

            DataSet grpDataSet = GetDataSet(command, "groups");

            if (grpDataSet.Tables["groups"].Rows.Count <= 0) {
                yield return null;
            }

            foreach (DataRow row in grpDataSet.Tables["groups"].Rows)
            {

                yield return new Group
                {
                    Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                    Group_Name = row["group_name"].ToString(),
                    Group_Category = row["group_category"].ToString(),
                    Group_Created = Convert.ToDateTime(row["group_created"].ToString())
                };
            }
        }

        public static Group getGroupByID(int groupID)
        {
            Group newGroup = new Group();

            string command = string.Format("select * from demouser.forum_group where group_ID = {0}", groupID);

            DataSet grpDataSet = GetDataSet(command, "groups");

            if (grpDataSet.Tables["groups"].Rows.Count <= 0)
            {
                return null;
            }

            foreach (DataRow row in grpDataSet.Tables["groups"].Rows)
            {

                newGroup = new Group
                {
                    Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                    Group_Name = row["group_name"].ToString(),
                    Group_Category = row["group_category"].ToString(),
                    Group_Created = Convert.ToDateTime(row["group_created"].ToString()),
                    Creator_ID = row["creator_ID"].ToString(),
                    Creator = UserRepository.getUser(row["creator_ID"].ToString())
                };
            }

            return newGroup;
        }


        public static IEnumerable<Models.Group> getAdminGroups(string username) {
            string command = string.Format("select * from demouser.forum_group where group_ID in (select group_ID from demouser.admin where username = '{0}')", username);

            DataSet grpDataSet = GetDataSet(command, "groups");

            if (grpDataSet.Tables["groups"].Rows.Count <= 0)
            {
                yield return null;
            }

            foreach (DataRow row in grpDataSet.Tables["groups"].Rows)
            {

                yield return new Group
                {
                    Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                    Group_Name = row["group_name"].ToString(),
                    Group_Category = row["group_category"].ToString(),
                    Group_Created = Convert.ToDateTime(row["group_created"].ToString())
                };
            }
        }

        public static IEnumerable<Models.Group> getAllGroups(string username)
        {

            string command = string.Format("select * from demouser.forum_group where group_ID not in (select group_ID from demouser.membership where username = '{0}')",username);

            DataSet grpDataSet = GetDataSet(command, "groups");

            if (grpDataSet.Tables["groups"].Rows.Count <= 0)
            {
                yield return null;
            }

            foreach (DataRow row in grpDataSet.Tables["groups"].Rows)
            {

                yield return new Group
                {
                    Group_ID = Convert.ToInt32(row["group_ID"].ToString()),
                    Group_Name = row["group_name"].ToString(),
                    Group_Category = row["group_category"].ToString(),
                    Group_Created = Convert.ToDateTime(row["group_created"].ToString())
                };
            }
        }

        public static IEnumerable<Models.Request> getRequests(string username)
        {

            string command = string.Format("select * from demouser.request_membership where group_ID in (select group_ID from demouser.admin where username = '{0}')", username);

            DataSet grpDataSet = GetDataSet(command, "request");

            if (grpDataSet.Tables["request"].Rows.Count <= 0)
            {
                yield return null;
            }

            foreach (DataRow row in grpDataSet.Tables["request"].Rows)
            {

                yield return new Request
                {
                    GroupID = Convert.ToInt32(row["group_ID"].ToString()),
                    Group = getGroupByID(Convert.ToInt32(row["group_ID"].ToString())),
                    Username = row["username"].ToString(),
                    User = UserRepository.getUser(row["username"].ToString())
                };
            }
        }

        public static int InsertRequest(int groupID, string username) {
            if (RequestExists(groupID, username))
            {
                return 0;
            }
            string command = string.Format("insert into demouser.request_membership(username, group_ID) values('{0}',{1})", username, groupID);
            int status = ExecuteNonQuery(command);

            return status;
        }

        public static bool RequestExists(int groupID, string username) {
            DataSet reqDS = GetDataSet(string.Format("select * from demouser.request_membership where group_ID = {0} and username = '{1}'", groupID, username), "request");

            if (reqDS.Tables["request"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static int RequestDelete(int groupID, string username)
        {
            string command = string.Format("delete from demouser.request_membership where group_ID = {0} and username = '{1}'", groupID, username);

            int status = ExecuteNonQuery(command);

            return status;

        }


        public static bool MembershipExists(int groupID, string username)
        {
            DataSet reqDS = GetDataSet(string.Format("select * from demouser.membership where group_ID = {0} and username = '{1}'", groupID, username), "request");

            if (reqDS.Tables["request"].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        


        public static int MembershipInsert(int groupID, string username) {
            if (MembershipExists(groupID, username)) {
                return 0;
            }

            string command = string.Format("insert into demouser.membership(username, group_ID) values('{0}',{1})", username, groupID);
            int status = ExecuteNonQuery(command);

            status = RequestDelete(groupID, username);

            return status;

        }

        public static int MembershipDelete(int groupID, string username)
        {
            if (!MembershipExists(groupID, username))
            {
                return 0;
            }

            string command = string.Format("delete from demouser.membership where group_ID = {0} and username='{1}'", groupID, username);
            int status = ExecuteNonQuery(command);
            
            return status;

        }



        public static int AdminInsert(string username, int groupID) {
            string command = string.Format("insert into demouser.admin(username, group_ID) values('{0}',{1})", username, groupID);

            int status = ExecuteNonQuery(command);

            return status;
        }

        public static int AdminDelete(string username, int groupID)
        {
            string command = string.Format("delete from demouser.admin where username = '{0}' and group_ID = {1}", username, groupID);

            int status = ExecuteNonQuery(command);

            return status;
        }

        public static int GroupInsert(string groupName, string groupCategory, string creatorUsername) {

            OracleCommand command = new OracleCommand("insert into demouser.forum_group(group_ID, group_name, group_created, group_category, creator_ID) values(demouser.group_seq.nextval, :1,:2,:3,:4)");
            command.Connection = con;

            command.Parameters.Add(":1", OracleDbType.Varchar2).Value = groupName;
            command.Parameters.Add(":2", OracleDbType.Date).Value= DateTime.Now;
            command.Parameters.Add(":3", OracleDbType.Varchar2).Value = groupCategory;
            command.Parameters.Add(":4", OracleDbType.Varchar2).Value = creatorUsername;

            con.Open();
            int status = command.ExecuteNonQuery();
            con.Close();
            

            return status;
        }

        public static bool GroupExists(int groupID)
        {
            Group newGroup = new Group();

            string command = string.Format("select * from demouser.forum_group where group_ID = {0}", groupID);

            DataSet grpDataSet = GetDataSet(command, "groups");

            if (grpDataSet.Tables["groups"].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool CheckMembership(int groupID, string username)
        {
            string command2 = string.Format("select group_ID from demouser.membership where group_ID = {0} and username = '{1}'", groupID, username);

            int rez = GetInt(command2);

            if (rez > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckAdmin(int groupID, string username)
        {
            string command2 = string.Format("select group_ID from demouser.admin where group_ID = {0} and username = '{1}'", groupID, username);

            int rez = GetInt(command2);

            if (rez > 0)
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