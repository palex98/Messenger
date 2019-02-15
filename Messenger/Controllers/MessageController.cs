using Messenger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class MessageController : Controller
    {
        [HttpPost]
        public ActionResult GetMessages(int chatId)
        {
            List<Messages> listOfMessages = new List<Messages>();

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var chat = context.Chats.Where(c => c.Id == chatId).FirstOrDefault();

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
                    listOfMessages.Add(context.Messages.Where(i => i.Id == id).FirstOrDefault());
                }
            }

            return PartialView("Message", listOfMessages);
        }

        [HttpPost]
        public ActionResult PostMessage(string text, int sender, int receiver, int chatId, bool isForvarded = false, bool isReply = false, int? replyTo = 0)
        {
            Messages newMessage = new Messages
            {
                Text = text,
                Sender = sender,
                Receiver = receiver,
                ChatId = chatId,
                DateTime = DateTime.Now
            };

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var addedMessage = context.Messages.Add(newMessage);
                context.SaveChanges();

                var chat = context.Chats.Where(c => c.Id == chatId).FirstOrDefault();

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

            return PartialView("Message", new List<Messages> { newMessage });
        }

        [HttpPost]
        public ActionResult SearchMessages(int chatId, string searchRequest)
        {
            List<Messages> messages;

            using (var context = new MessengerDBEntities())
            {
                messages = context.Messages.Where(m => m.ChatId == chatId && m.Text.Contains(searchRequest)).ToList();
            }

            return PartialView("Message", messages);
        }
    }
}