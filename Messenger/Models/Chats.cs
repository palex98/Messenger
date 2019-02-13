namespace Messenger.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Chats
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string ListOfMessages { get; set; }

        public static string GetTextOfLastMessageFromChat(int Id)
        {
            string msg;

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                var chat = context.Chats.Where(c => c.Id == Id).FirstOrDefault();

                int lastMsgId;

                try
                {
                    lastMsgId = JsonConvert.DeserializeObject<List<int>>(chat.ListOfMessages).Last();
                    msg = context.Messages.Where(m => m.Id == lastMsgId).FirstOrDefault().Text;
                }
                catch
                {
                    msg = "...";
                }
            }

            return msg;
        }
    }
}
