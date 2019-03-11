using Messenger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class AdminController : Controller
    {
        [Route("admin")]
        public ActionResult Index()
        {
            using (var context = new MessengerDBEntities())
            {
                var users = context.Users.ToList();
                return View("Admin", users);
            }
        }

        public RedirectResult CreateUser(string username, string password)
        {
            using (var context = new MessengerDBEntities())
            {
                if (!context.Users.Any(u => u.UserName == username))
                {
                    var newUser = new Users
                    {
                        UserName = username,
                        Password = password,
                        LastSeen = " "
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                }

                var users = context.Users.ToList();

                return Redirect("/admin");
            }
        }

        [HttpGet]
        public ActionResult GetContacts(int userId)
        {
            using (var context = new MessengerDBEntities())
            {
                var usersContactsJSON = context.Users.FirstOrDefault(u => u.Id == userId).Contacts;

                Dictionary<int, int> usersContacts = new Dictionary<int, int>();

                try
                {
                    usersContacts = JsonConvert.DeserializeObject<Dictionary<int, int>>(usersContactsJSON);
                }
                catch
                {

                }

                List<Users> checkedUsers = new List<Users>();

                foreach (var u in usersContacts)
                {
                    var user = context.Users.FirstOrDefault(us => us.Id == u.Key);

                    checkedUsers.Add(user);
                }

                List<Users> uncheckedUsers = new List<Users>();

                foreach (var user in context.Users)
                {
                    if(!checkedUsers.Exists(u => u == user) && user.Id != userId)
                    {
                        uncheckedUsers.Add(user);
                    }
                }

                ViewBag.User = context.Users.FirstOrDefault(u => u.Id == userId).UserName;
                ViewBag.CheckedUsers = checkedUsers;
                ViewBag.UncheckedUsers = uncheckedUsers;

                return PartialView("Contacts");
            }
        }
    }
}