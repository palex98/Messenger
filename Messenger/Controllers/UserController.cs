using Messenger.Hubs;
using Messenger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Messenger.Controllers
{
    public class UserController : ApiController
    {
        public string GetUserStatus(int userId)
        {
            if (MessengerHub.Users.Exists(u => u.Id == userId))
            {
                return "Online";
            }
            else
            {
                using (var context = new MessengerDBEntities())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == userId);

                    return "был в сети в " + user.LastSeen;
                }
            }
        }

        public void PostContact(PostParams prms)
        {
            using (var context = new MessengerDBEntities())
            {
                var user1 = context.Users.FirstOrDefault(u => u.Id == prms.userId);

                Dictionary<int, int> usersContacts1 = new Dictionary<int, int>();

                try
                {
                    usersContacts1 = JsonConvert.DeserializeObject<Dictionary<int, int>>(user1.Contacts);
                }
                catch
                {

                }

                if(!usersContacts1.Any(u => u.Key == prms.contactId))
                {
                    var newChat = new Chats()
                    {
                        Sender = prms.userId,
                        Receiver = prms.contactId
                    };

                    var chat = context.Chats.Add(newChat);
                    context.SaveChanges();

                    usersContacts1.Add(prms.contactId, chat.Id);

                    var newJSON1 = JsonConvert.SerializeObject(usersContacts1);

                    user1.Contacts = newJSON1;

                    var user2 = context.Users.FirstOrDefault(u => u.Id == prms.contactId);

                    Dictionary<int, int> usersContacts2 = new Dictionary<int, int>();

                    try
                    {
                        usersContacts2 = JsonConvert.DeserializeObject<Dictionary<int, int>>(user2.Contacts);
                    }
                    catch
                    {

                    }

                    usersContacts2.Add(prms.userId, chat.Id);

                    var newJSON2 = JsonConvert.SerializeObject(usersContacts2);

                    user2.Contacts = newJSON2;

                    context.SaveChanges();
                }
                else
                {
                    usersContacts1.Remove(prms.contactId);

                    var newJSON1 = JsonConvert.SerializeObject(usersContacts1);

                    user1.Contacts = newJSON1;

                    var user2 = context.Users.FirstOrDefault(u => u.Id == prms.contactId);

                    Dictionary<int, int> usersContacts2 = new Dictionary<int, int>();

                    try
                    {
                        usersContacts2 = JsonConvert.DeserializeObject<Dictionary<int, int>>(user2.Contacts);
                    }
                    catch
                    {

                    }

                    usersContacts2.Remove(prms.userId);

                    var newJSON2 = JsonConvert.SerializeObject(usersContacts2);

                    user2.Contacts = newJSON2;

                    context.SaveChanges();
                }
            }
        }

        public void DeleteUser(DelParams prms)
        {
            using (var context = new MessengerDBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == prms.userId);

                if (user != null) context.Users.Remove(user);

                context.SaveChanges();

                foreach (var u in context.Users)
                {
                    var usersContacts = new Dictionary<int, int>();

                    try
                    {
                        usersContacts = JsonConvert.DeserializeObject<Dictionary<int, int>>(u.Contacts);
                    }
                    catch
                    {

                    }

                    if (usersContacts.ContainsKey(prms.userId))
                    {
                        usersContacts.Remove(prms.userId);
                    }

                    var newJson = JsonConvert.SerializeObject(usersContacts);

                    u.Contacts = newJson;
                }

                context.SaveChanges();
            }
        }

        public void PutPassword(PutParams prms)
        {
            using (var context = new MessengerDBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == prms.userId);

                user.Password = prms.password;

                context.SaveChanges();
            }
        }
    }

    public class DelParams
    {
        public int userId { get; set; }
    }

    public class PutParams
    {
        public int userId { get; set; }
        public string password { get; set; }
    }

    public class PostParams
    {
        public int userId { get; set; }
        public int contactId { get; set; }
    }
}
