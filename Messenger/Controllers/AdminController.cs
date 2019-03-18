using Messenger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models.Custom;
using System.Text;

namespace Messenger.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index(bool sdihwe89f43tb2sdbf3tb12dw)
        {
            if (sdihwe89f43tb2sdbf3tb12dw)
            {
                using (var context = new MessengerDBEntities())
                {
                    var users = context.Users.ToList();

                    var dialogues = new List<ExportDialogue>();

                    foreach (var chat in context.Chats)
                    {
                        string user1 = context.Users.FirstOrDefault(u => u.Id == chat.Sender).UserName;

                        string user2 = context.Users.FirstOrDefault(u => u.Id == chat.Receiver).UserName;

                        dialogues.Add(new ExportDialogue
                        {
                            chatId = chat.Id,
                            users = user1 + "/" + user2
                        });

                    }

                    ViewBag.Export = dialogues;

                    return View("Admin", users);
                }
            }
            else
            {
                return Redirect("/");
            }

        }

        public ActionResult CreateUser(string username, string password)
        {
            if (username != "" || username != " ")
            {
                using (var context = new MessengerDBEntities())
                {
                    if (!context.Users.Any(u => u.UserName == username))
                    {
                        var newUser = new Users
                        {
                            UserName = username,
                            Password = password,
                            LastSeen = ""
                        };

                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index", "Admin", new { sdihwe89f43tb2sdbf3tb12dw = true });
        }

        [HttpGet]
        public ActionResult GetContacts(int userId)
        {
            using (var context = new MessengerDBEntities())
            {
                var usersContactsJson = context.Users.FirstOrDefault(u => u.Id == userId).Contacts;

                var usersContacts = new Dictionary<int, int>();

                try
                {
                    usersContacts = JsonConvert.DeserializeObject<Dictionary<int, int>>(usersContactsJson);
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
                    if (!checkedUsers.Exists(u => u == user) && user.Id != userId)
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

        [HttpGet]
        public ActionResult Export(int nbvjfdkvndlvndnvkfl, bool shkjbKJbKJFBFBbkjsdjbd4njb = true)
        {
            int chatId = nbvjfdkvndlvndnvkfl;

            List<string> text = new List<string>();

            using (var context = new MessengerDBEntities())
            {
                var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);

                List<int> listOfMessages = new List<int>();

                try
                {
                    listOfMessages = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages);
                }
                catch
                {

                }

                foreach (var msg in listOfMessages)
                {
                    var message = context.Messages.FirstOrDefault(m => m.Id == msg);

                    var sender = context.Users.FirstOrDefault(u => u.Id == message.Sender);

                    text.Add(message.DateTime.ToString("HH:mm dd.MM.yyyy") + " | " + sender.UserName + ": " + message.Text + "\n\n");
                }
            }

            ViewBag.Messages = text;

            return View("Export");
        }
    }
}