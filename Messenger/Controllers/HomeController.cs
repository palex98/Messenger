using Messenger.Models;
using Messenger.Models.Custom;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int id)
        {
            List<ChatDataToView> data = new List<ChatDataToView>();

            Dictionary<int, int> contacts = new Dictionary<int, int>();

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

                try
                {
                    contacts = JsonConvert.DeserializeObject<Dictionary<int, int>>(user.Contacts);
                }
                catch
                {

                }

                foreach (var c in contacts)
                {
                    ChatDataToView dt = new ChatDataToView
                    {
                        chatId = c.Value,
                        Receiver = c.Key,
                        partnerFullName = context.Users.Where(u => u.Id == c.Key).FirstOrDefault().NameToString(),
                        textOfLastMsg = Chats.GetTextOfLastMessageFromChat(c.Value)
                    };
                    data.Add(dt);
                }

                user.LastSeen = DateTime.Now;
                context.SaveChanges();
            }

            ViewBag.MyId = id;

            return View(data);
        }
    }
}