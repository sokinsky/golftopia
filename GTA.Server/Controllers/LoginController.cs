using GTA.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Controllers {
    public class LoginController : Controller<Login> {
        public LoginController(Context context, Login login) : base(context, login){ }


    }
}
