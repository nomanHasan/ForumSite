using Forum1._0.Models;
using Forum1._0.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum1._0.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User loginUser) {

            User loggedUser = UserRepository.CheckLogin(loginUser.Username, loginUser.Password);

            if (loggedUser != null)
            {
                Session["USER"] = loggedUser;
                Session["USERNAME"] = loggedUser.Username;
                Session["NAME"] = loggedUser.Name;

                if (UserRepository.IsAdmin(loggedUser.Username) > 0)
                {
                    Session["ISADMIN"] = true;
                }
                else {
                    Session["ISADMIN"] = false;
                }

                return View("LoginSuccessful");
            }
            else {
                ViewBag.ErrorMessage = "Your provided Password and username doesn't match";
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user) {
            if (!ModelState.IsValid) {
                return View(user);
                //return Content("Modestate is not Valid");
            }

            if (UserRepository.UsernameExist(user.Username)) {
                ViewBag.UsernameExist = "Username already Exists. Enter a different username";
                return View(user);
            }

            int status = UserRepository.InsertUser(user.Username, user.Password, user.Name, user.Email, user.Phone, user.Address);

            ViewBag.Name = user.Name;
            return View("RegisterSuccess");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult Home() {
            return RedirectToAction("Index", "Group");
        }

        public ActionResult LogoutConfirm() {

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}