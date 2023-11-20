using GTA.Client.Controllers;
using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.Repositories {
    public class UserRepository : Repository<User, UserController> {
        public UserRepository(Context context) : base(context) { }
        public async Task<LoginController?> Login(string username, string password) {
            var responseMessage = await this.post("login", new { username, password });
            if (responseMessage.IsSuccessStatusCode) 
                return await this.Context.Logins.AddAsync(responseMessage);            
            return null;
        }
        public async Task<UserController?> ByLogin(string token) {
            var responseMessage = await this.post("bylogin", new { token });
            if (responseMessage.IsSuccessStatusCode)
                return await this.Context.Users.AddAsync(responseMessage);
            return null;
        }
    }
}
