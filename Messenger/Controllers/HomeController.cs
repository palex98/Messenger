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
        public ActionResult Index(int id)
        {
            List<Chats> chats = new List<Chats>();
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
                    chats.Add(context.Chats.Where(i => i.Id == c.Value).FirstOrDefault());
                }

                user.LastSeen = DateTime.Now;
                context.SaveChanges();
            }

            ViewBag.MyId = id;

            return View(chats);
        }
    }
}