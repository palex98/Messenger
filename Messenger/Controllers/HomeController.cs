using Messenger.Models;
using Messenger.Models.Custom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class HomeController : Controller
    {
        public static List<string> colors = new List<string>
        {
            "ff4444",
            "ffbb33",
            "00C851",
            "33b5e5",
            "2BBBAD",
            "4285F4",
            "aa66cc",
            "ec407a",
            "bcaaa4"
        };

        public ActionResult Index(int id)
        {
            var userFromCookies = HttpContext.Request.Cookies["user"].Value;

            if (userFromCookies != id.ToString())
            {
                return Redirect("/");
            }

            List<ChatDataToView> data = new List<ChatDataToView>();

            Dictionary<int, int> contacts = new Dictionary<int, int>();

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == id);

                try
                {
                    contacts = JsonConvert.DeserializeObject<Dictionary<int, int>>(user.Contacts);
                }
                catch
                {

                }

                foreach (var c in contacts)
                {
                    var messages = context.Messages.Where(m => m.ChatId == c.Value && m.Sender != id && !m.IsReaded);

                    int unresded = messages.Count();

                    var contact = context.Users.FirstOrDefault(u => u.Id == c.Key);

                    ChatDataToView dt = new ChatDataToView
                    {
                        ChatId = c.Value,
                        Receiver = c.Key,
                        PartnerFullName = contact.UserName,
                        LastMessage = context.Chats.FirstOrDefault(ch => ch.Id == c.Value).LastMessage,
                        Unreaded = unresded, 
                        Color = contact.Color
                    };
                    data.Add(dt);
                }

                user.LastSeen = DateTime.Now.ToLocalTime().ToString("HH:mm dd.MM.yyyy");
                context.SaveChanges();

                ViewBag.Username = user.UserName;
            }

            ViewBag.MyId = id;

            data = data.OrderBy(d => d.LastMessage).ToList();

            data.Reverse();

            return View(data);
        }

        [HttpGet]
        public FileResult GetFile(string file)
        {
            string filePath = Server.MapPath("~/Files/" + file);

            return File(filePath, "application", file);
        }
    }
}