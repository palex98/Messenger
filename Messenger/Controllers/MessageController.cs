using Messenger.Hubs;
using Messenger.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);

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

            ViewBag.myId = sender;

            return PartialView("Message", new List<Messages> { newMessage });
        }

        [HttpPost]
        public ActionResult PostFileMessage()
        {

            int sender = Convert.ToInt32(Request.Form.Get("myId"));

            int chatId = Convert.ToInt32(Request.Form.Get("chatId"));

            string fileName;

            var upload = Request.Files[0];

            fileName = DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" +  System.IO.Path.GetFileName(upload.FileName);
            upload.SaveAs(Server.MapPath("~/Files/" + fileName));

            string text = "<a href=\"" + "../../Home/GetFile/?file=" + fileName + "\">" + Request.Files[0].FileName + "</a>";

            Messages newMessage = new Messages
            {
                Text = text,
                Sender = sender,
                ChatId = chatId,
                DateTime = DateTime.Now,
                IsReaded = false
            };

            int receiver;

            using (var context = new MessengerDBEntities())
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

            ViewBag.myId = sender;

            return PartialView("Message", new List<Messages> { newMessage });
        }

        public ActionResult ReadMessages(int userId, int chatId)
        {
            using (var context = new MessengerDBEntities())
            {
                var messages = context.Messages.Where(m => m.ChatId == chatId && m.Sender != userId);

                var unread = messages.Where(m => !m.IsReaded);

                foreach (var m in unread)
                {
                    m.IsReaded = true;
                }

                context.SaveChanges();

                var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);

                var receiver = userId == chat.Sender ? chat.Receiver : chat.Sender;

                if (MessengerHub.Users.Exists(u => u.Id == receiver))
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();
                    hubContext.Clients.Client(MessengerHub.Users.FirstOrDefault(c => c.Id == receiver).ConnectionId).readMessages(chatId);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}