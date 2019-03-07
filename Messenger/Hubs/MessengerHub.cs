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

        // Подключение нового пользователя
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

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(Context.ConnectionId).onUserConnected();
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (item != null)
            {
                Users.Remove(item);
                Clients.All.onUserDisconnected();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}