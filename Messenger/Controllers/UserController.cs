using Messenger.Hubs;
using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

                    return user.LastSeen;
                }
            }

        }
    }
}
