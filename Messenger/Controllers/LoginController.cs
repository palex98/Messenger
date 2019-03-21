using System;
using Messenger.Models;
using System.Linq;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Response.Cookies["user"].Value = "#";

            return View("Login");
        }

        public ActionResult UserLogin(string login, string pass)
        {
            using (var context = new MessengerDBEntities())
            {
                Users user;

                try
                {
                    user = context.Users.First(u => u.UserName == login);
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = "Пользователь не найден!";
                    return View("Login");
                }

                if (user != null)
                {
                    var password = user.Password;

                    if (pass == password)
                    {
                        if (login == "admin")
                        {
                            return RedirectToAction("Index", "Admin", new { sdihwe89f43tb2sdbf3tb12dw = true });
                        }

                        if (HttpContext != null) HttpContext.Response.Cookies["user"].Value = user.Id.ToString();
                        return RedirectToAction("Index", "Home", new { id = user.Id });
                    }
                }
            }

            ViewBag.ErrorMessage = "Неправильный пароль!";
            return View("Login");
        }
    }
}