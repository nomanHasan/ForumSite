using Forum1._0.Models;
using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum1._0.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View(UserRepository.getUsers());
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Timeline(string username) {
            User currentUser = (User)Session["USER"];

            IEnumerable<Status> Statuses = StatusRepository.GetStatuses(username);

            User user = UserRepository.getUser(username);

            ViewBag.CurrentUser = currentUser;
            ViewBag.User = user;
            
            return View(Statuses);
        }

        public ActionResult UserProfile() {
            return RedirectToAction("Index", "Group");
        }

        [HttpPost]
        public ActionResult PostStatus(FormCollection form) {
            string content = form["status_content"];

            string username = (string)Session["USERNAME"];

            int status = StatusRepository.StatusInsert(content, username);

            return RedirectToAction("Timeline", "User", new { username = username});

        }

        public ActionResult MyProfile() {

            User user = (User)Session["USER"];

            return View(user);

        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
