using Azure.Identity;
using GTA.Data.Models;
using GTA.Server.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Repositories {
    public class UserRepository : Repository<User, Controller<User>> {
        public UserRepository(Context context) : base(context) { }

        [Web.Method]
        public async Task<Controller<Login>> Login(string username, string password) {
            var userModel = await this.Context.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (userModel == null || userModel.Password != password)
                throw Web.Exception.NotFound("Username and password did not match any records");

           foreach (var login in this.Context.Logins.Where(x => x.UserID == userModel.ID && x.Expired == null)) {
                login.Expired = DateTime.Now;
            }
            var loginModel = new Login {
                User = userModel
            };
            this.Context.Logins.Add(loginModel);
            await this.Context.SaveChangesAsync();
            return new LoginController(this.Context, loginModel);
        }

        [Web.Method]
        public async Task<Controller<User>> ByLogin(string token) {
            var loginModel = await this.Context.Logins.SingleOrDefaultAsync(x => x.Token == token);
            if (loginModel == null)
                throw Web.Exception.NotFound("Login does not exist");
            var userModel = await this.Context.Users.SingleOrDefaultAsync(x => x.ID == loginModel.UserID);
            if (userModel == null)
                throw Web.Exception.NotFound("User was not found");
            return new Controller<User>(this.Context, userModel);

            
        }

    }
}
