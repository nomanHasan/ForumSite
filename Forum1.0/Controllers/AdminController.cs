using Forum1._0.Models;
using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum1._0.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult RequestList() {
            User currentUser = (User)Session["USER"];

            IEnumerable<Request> requestCollection = GroupRepository.getRequests(currentUser.Username);

            return View(requestCollection);
        }

        public ActionResult RequestAccept(string username, int groupID) {
            GroupRepository.MembershipInsert(groupID, username);

            return RedirectToAction("RequestList", "Admin");
        }

        public ActionResult RequestDeny(string username, int groupID) {
            GroupRepository.RequestDelete(groupID, username);

            return RedirectToAction("RequestList", "Admin");
        }

        [HttpGet]
        public ActionResult GroupCreate() {
            return View();
        }

        [HttpPost]
        public ActionResult GroupCreate(Group group) {

            if (!ModelState.IsValid) {
                return GroupCreate(group);
            }

            User user = (User)Session["USER"];

            int status = GroupRepository.GroupInsert(group.Group_Name, group.Group_Category, user.Username);

            return RedirectToAction("Index", "Group");
        }


        public ActionResult GroupManagement() {

            User user = (User)Session["USER"];

            IEnumerable<Group> allAdminGroups = GroupRepository.getAdminGroups(user.Username);
            
            return View(allAdminGroups);


        }

        public ActionResult Group(int groupID) {

            Group group = GroupRepository.getGroupByID(groupID);
            string username = (string)Session["USERNAME"];

            IEnumerable<User> Members = UserRepository.returnMembers(groupID);

            IEnumerable<User> Admins = UserRepository.returnAdmins(groupID, username);

            ViewBag.GroupID = groupID;

            ViewBag.Members = Members;
            ViewBag.Admins = Admins;

            return View(group);
        }



        public ActionResult CancelMembeship(int groupID, string username) {

            if (GroupRepository.MembershipExists(groupID, username)) {
                GroupRepository.MembershipDelete(groupID, username);
            }

            return RedirectToAction("Group", "Admin", new { groupID = groupID });
        }
        public ActionResult PromoteToAdmin(int groupID, string username) {
            if (!UserRepository.IsAdmin(groupID, username)) {
                GroupRepository.AdminInsert(username, groupID);
            }

            return RedirectToAction("Group", "Admin", new { groupID = groupID });
        }

        public ActionResult DemoteAdmin(int groupID, string username)
        {
            if (UserRepository.IsAdmin(groupID, username))
            {
                GroupRepository.AdminDelete(username, groupID);
            }

            return RedirectToAction("Group", "Admin", new { groupID = groupID });
        }

        public ActionResult ManageSystem() {
            return View();
        }

        public ActionResult CalibrateCommentLike() {
            Repository.ExecuteNonQuery("begin demouser.calibrate_comment_like; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }

        public ActionResult CalibrateCommentDislike()
        {
            Repository.ExecuteNonQuery("begin demouser.calibrate_comment_dislike; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }

        public ActionResult CalibrateThreadLike()
        {
            Repository.ExecuteNonQuery("begin demouser.calibrate_thread_like; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }

        public ActionResult CalibrateThreadDislike()
        {
            Repository.ExecuteNonQuery("begin demouser.calibrate_thread_dislike; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }

        public ActionResult CalibrateStatusLike()
        {
            Repository.ExecuteNonQuery("begin demouser.calibrate_status_like; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }

        public ActionResult CalibrateStatusDislike()
        {
            Repository.ExecuteNonQuery("begin demouser.calibrate_status_dislike; end; ");
            return RedirectToAction("ManageSystem", "Admin");
        }
    }
}