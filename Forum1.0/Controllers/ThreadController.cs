using Forum1._0.Helper;
using Forum1._0.Models;
using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum1._0.Controllers
{
    public class ThreadController : Controller
    {
        // GET: Thread
        public ActionResult Index(int groupID)
        {
            string username = (string)Session["USERNAME"];
            if (!Authenticate.AuthenticateMembership(username, groupID)) {
                return RedirectToAction("Index", "Group");
            }

            ViewBag.GroupID = groupID;

            IEnumerable<Thread> threadCollection = ThreadRepository.GetThreads(groupID);

            return View(threadCollection);
        }

        [HttpGet]
        public ActionResult Create(int groupID) {
            User currentUser = (User)Session["USER"];

            ViewBag.GroupID = groupID;
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection forms)
        {
            string thread_title = forms["thread_title"];
            string thread_desc = forms["thread_desc"];
            int groupID = Convert.ToInt32(forms["groupID"]);
            string username = (string)Session["USERNAME"];

            ThreadRepository.InsertThread(username,groupID,thread_title,thread_desc);

            return RedirectToAction("Index","Thread", new {groupID = groupID });


        }


    }
}