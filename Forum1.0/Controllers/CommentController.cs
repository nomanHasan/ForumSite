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
    public class CommentController : Controller
    {
        // GET: Comment

        [HttpGet]
        public ActionResult Index(int threadID)
        {

            if (! Authenticate.AuthenticateThreadAccess((string)Session["USERNAME"], threadID)) {
                return RedirectToAction("Index", "Group");
            }


            if (!ThreadRepository.CheckThreadAccess((string)Session["USERNAME"], threadID)) {
                return RedirectToAction("Index", "Group");
            }

            IEnumerable<Comment> commentCollection = CommentRepository.GetComments(threadID);

            Thread mainThread = ThreadRepository.GetThreadByID(threadID);

            ViewBag.Thread = mainThread;

            ViewBag.ThreadTitle = mainThread.Thread_Title;

            ViewBag.ThreadDesc = mainThread.Thread_Desc;

            ViewBag.ThreadCreated = mainThread.Thread_Created.ToString();

            ViewBag.ThreadAuthor = mainThread.User.Name;

            return View(CommentRepository.GetComments(threadID));
        }

        public ActionResult Like(int commentID, int threadID) {

            string username = (string)Session["USERNAME"];

            if (!Authenticate.AuthenticateThreadAccess(username, threadID))
            {
                return RedirectToAction("Index", "Group");
            }
            if (!CommentRepository.CheckCommentAccess(commentID, username)) {
                return RedirectToAction("Index", "Group");
            }

            int status = CommentRepository.LikeComment(commentID, username);

            return RedirectToAction("Index", "Comment", new { threadID = threadID });

        }

        public ActionResult Dislike(int commentID, int threadID) {
            string username = (string)Session["USERNAME"];

            if (!Authenticate.AuthenticateThreadAccess(username, threadID))
            {
                return RedirectToAction("Index", "Group");
            }
            if (!CommentRepository.CheckCommentAccess(commentID, username))
            {
                return RedirectToAction("Index", "Group");
            }

            int status = CommentRepository.DislikeComment(commentID, username);

            return RedirectToAction("Index", "Comment", new { threadID = threadID });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form) {

            string comment_content = form["comment_content"];
            //string threadID = form["threadID"];
            int threadID = Convert.ToInt32(form["threadID"]);
            User user = (User)Session["USER"];

            Thread mainThread = ThreadRepository.GetThreadByID(threadID);

            //return Content(threadID + " " + comment_content + " " + user.Username + " " + DateTime.Now);

            int status = CommentRepository.InsertComment(threadID, comment_content, user.Username, 0, DateTime.Now);

            return RedirectToAction("Index", "Comment",new {threadID = threadID});
        }
    }
}