using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Messenger.Models;
using Microsoft.AspNet.SignalR;

namespace Messenger.Hubs
{
    public class MessengerHub : Hub
    {
        public static List<Users> Users = new List<Users>();

        public void Connect(int userId)
        {
            if (!Users.Any(x => x.ConnectionId == Context.ConnectionId))
            {

                using (var context = new MessengerDBEntities())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == userId);

                    user.ConnectionId = Context.ConnectionId;

                    Users.Add(user);
                }

                Clients.AllExcept(Context.ConnectionId).onUserConnected(userId);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            using (var context = new MessengerDBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == item.Id);

                user.LastSeen = "был в сети в " + DateTime.Now.ToLocalTime().ToString("HH:mm dd.MM.yyyy");

                context.SaveChanges();
            }

                if (item != null)
                {
                    Clients.All.onUserDisconnected(item.Id);
                    Users.Remove(item);
                }

            return base.OnDisconnected(stopCalled);
        }
    }
}