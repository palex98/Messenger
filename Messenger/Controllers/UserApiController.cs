using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Messenger.Controllers
{
    public class UserApiController : ApiController
    {
        [HttpGet]
        [Route("api/user/GetName")]
        public string GetFullUserNameById(int id)
        {
            string name;

            using (MessengerDBEntities context = new MessengerDBEntities())
            {
                name = context.Users.Where(u => u.Id == id).FirstOrDefault().NameToString();
            }

            return name;
        }
    }
}
