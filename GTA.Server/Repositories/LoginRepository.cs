using GTA.Data.Models;
using GTA.Server.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Repositories {
    public class LoginRepository : Repository<Login, LoginController>{
        public LoginRepository(Context context) : base(context) { }
        [Web.Method]
        public async Task<LoginController> ByToken(string token) {
            var login = await this.Context.Logins.SingleOrDefaultAsync(x => x.Token == token);
            if (login == null)
                throw Web.Exception.NotFound("Unable to find login with provided token");
            return this[login];
        }
    }
}
