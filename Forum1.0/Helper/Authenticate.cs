using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Helper
{
    public class Authenticate
    {
        public static bool AuthenticateThreadAccess(string username, int threadID) {

            int status = ThreadRepository.CheckThreadAccess2(threadID, username);

            if (status > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool AuthenticateCommentAccess(string username, int commentID) {
            if (CommentRepository.CheckCommentAccess(commentID, username))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool AuthenticateMembership(string username, int groupID) {

            return GroupRepository.CheckMembership(groupID,username);
        }

        public static bool AuthenticateAdmin(string username, int groupID) {
            return GroupRepository.CheckAdmin(groupID,username);
        }

        public static bool AuthenticateCommentAuthor(string username, int commentID) {

            return true;
        }
    }
}