using Messenger.Hubs;
using Messenger.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class MessageController : Controller
    {
        [HttpPost]
        public ActionResult GetMessages(int chatId, int myId)
        {
            List<Messages> listOfMessages = new List<Messages>();

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);

                List<int> messagesId = new List<int>();
                try
                {
                    messagesId = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages);
                }
                catch
                {
                    return PartialView("NoMessages");
                }

                foreach (var id in messagesId)
                {
                    listOfMessages.Add(context.Messages.FirstOrDefault(i => i.Id == id));
                }
            }

            if (listOfMessages.Count > 20) listOfMessages = listOfMessages.Skip(listOfMessages.Count - 20).ToList();

            ViewBag.myId = myId;

            return PartialView("Message", listOfMessages);
        }

        [HttpPost]
        public ActionResult GetLastMessage(int chatId, int myId)
        {
            List<Messages> message = new List<Messages>();

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var chat = context.Chats.Where(c => c.Id == chatId).FirstOrDefault();

                int messageId;

                try
                {
                    messageId = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages).LastOrDefault();
                }
                catch
                {
                    return PartialView("NoMessages");
                }

                message.Add(context.Messages.FirstOrDefault(i => i.Id == messageId));
            }

            ViewBag.myId = myId;

            return PartialView("Message", message);
        }

        [HttpPost]
        public ActionResult PostMessage(string text, int sender, int chatId)
        {
            Messages newMessage = new Messages
            {
                Text = text,
                Sender = sender,
                ChatId = chatId,
                DateTime = DateTime.Now,
                IsReaded = false
            };

            int receiver;

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var addedMessage = context.Messages.Add(newMessage);
                context.SaveChanges();

                var chat = context.Chats.Where(c => c.Id == chatId).FirstOrDefault();

                receiver = sender == chat.Sender ? chat.Receiver : chat.Sender;

                List<int> listOfMessages = new List<int>();

                try
                {
                    listOfMessages = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages);
                }
                catch
                {

                }

                listOfMessages.Add(addedMessage.Id);

                var json = JsonConvert.SerializeObject(listOfMessages);

                chat.ListOfMessages = json;

                context.SaveChanges();
            }

            if(MessengerHub.Users.Exists(u => u.Id == receiver))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();
                hubContext.Clients.Client(MessengerHub.Users.FirstOrDefault(c => c.Id == receiver).ConnectionId).newMessage(chatId);
            }
            
            return PartialView("Message", new List<Messages> { newMessage });
        }

        [HttpPost]
        public ActionResult PostFileMessage()
        {

            int sender = Convert.ToInt32(Request.Form.Get("myId"));
            int chatId = Convert.ToInt32(Request.Form.Get("chatId"));

            string text = "<a href=\"" + "../../Home/GetFile/?file=" + Request.Files[0].FileName + "\">" + Request.Files[0].FileName + "</a>";

            Messages newMessage = new Messages
            {
                Text = text,
                Sender = sender,
                ChatId = chatId,
                DateTime = DateTime.Now,
                IsReaded = false
            };

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                }
            }

            int receiver;

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var addedMessage = context.Messages.Add(newMessage);
                context.SaveChanges();

                var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);

                receiver = sender == chat.Sender ? chat.Receiver : chat.Sender;

                List<int> listOfMessages = new List<int>();

                try
                {
                    listOfMessages = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages);
                }
                catch
                {

                }

                listOfMessages.Add(addedMessage.Id);

                var json = JsonConvert.SerializeObject(listOfMessages);

                chat.ListOfMessages = json;

                context.SaveChanges();
            }

            if (MessengerHub.Users.Exists(u => u.Id == receiver))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();
                hubContext.Clients.Client(MessengerHub.Users.FirstOrDefault(c => c.Id == receiver).ConnectionId).newMessage(chatId);
            }

            return PartialView("Message", new List<Messages> { newMessage });
        }
    }
}


