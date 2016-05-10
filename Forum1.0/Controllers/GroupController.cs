using Forum1._0.Models;
using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum1._0.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null) {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Group> groupCollection = GroupRepository.getGroups(currentUser.Username);

            if (groupCollection ==null) {
                return View();
            }

            return View(GroupRepository.getGroups(currentUser.Username));
        }

        public ActionResult AllGroups() {
            User currentUser = (User)Session["USER"];

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Group> groupCollection = GroupRepository.getAllGroups(currentUser.Username);

            if (groupCollection == null)
            {
                return View();
            }

            return View(GroupRepository.getAllGroups(currentUser.Username));
        }

        public ActionResult RequestMembership(int groupID)
        {
            string username = (string)Session["username"];

            int status = GroupRepository.InsertRequest(groupID, username);

            return RedirectToAction("AllGroups", "Group");
        }
    }
}