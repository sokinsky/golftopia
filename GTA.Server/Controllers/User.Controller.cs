using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Controllers {
    public class UserController : Controller<User> {
        public UserController(Context context, User user) : base(context, user) {

        }
    }
}
