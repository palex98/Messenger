using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult Registration()
        {

            return View();
        }

        [HttpPost]
        public RedirectResult RegistrateNewUser(string firstName, string lastName, string userName, string email, string password)
        {
            Users newUser = new Users
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Password = password
            };

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }
    }
}