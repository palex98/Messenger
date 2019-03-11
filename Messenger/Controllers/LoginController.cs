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
            using (var context = new MessengerDBEntities())
            {
                Users user;

                if (context.Users.Any(u => u.UserName == login)) {
                    user = context.Users.FirstOrDefault(u => u.UserName == login);
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found!";
                    return View("Login");
                }

                if (user != null)
                {
                    var password = user.Password;

                    if (pass == password)
                    {
                        if (HttpContext != null) HttpContext.Response.Cookies["user"].Value = user.Id.ToString();
                        return RedirectToAction("Index", "Home", new { id = user.Id });
                    }
                }
            }

            ViewBag.ErrorMessage = "Password is incorrect!";
            return View("Login");
        }
    }
}