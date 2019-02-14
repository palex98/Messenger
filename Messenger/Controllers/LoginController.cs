using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models;

namespace Messenger.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult UserLogin(string login, string pass)
        {
            Users user;
            using (var context = new MessengerDBEntities())
            {
                if (context.Users.Any(u => u.UserName == login)) {
                    user = context.Users.FirstOrDefault(u => u.UserName == login);
                }else if (context.Users.Any(u => u.Email == login))
                {
                    user = context.Users.FirstOrDefault(u => u.Email == login);
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found!";
                    return View("Login");
                }

                var password = user.Password;

                if (pass == password) return RedirectToAction("Index", "Home", new { id = user.Id });
            }

            ViewBag.ErrorMessage = "Password is incorrect!";
            return View("Login");
        }
    }
}