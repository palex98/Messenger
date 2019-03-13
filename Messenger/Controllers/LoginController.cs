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

        public ActionResult UserLogin(string login, string pass)
        {
            using (var context = new MessengerDBEntities())
            {
                Users user;

                try
                {
                    user = context.Users.First(u => u.UserName == login);
                }
                catch
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