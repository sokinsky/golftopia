using GTA.Client.Controllers;
using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.Repositories {
    public class LoginRepository : Repository<Login, LoginController> {
        public LoginRepository(Context context) : base(context) { }

        public async Task<LoginController> ByToken(string token) {
            var responseMessage = await this.post("bytoken", new { token });
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Unable to find login");
            var result = await this.Context.Logins.AddAsync(responseMessage);
            if (result == null)
                throw new Exception("Unable to create login");
            return result;
        }
    }
}
