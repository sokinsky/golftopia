using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.Controllers {
    public class LoginController : Controller<Login> {
        public LoginController(Context context, Login login) : base(context, login) { }        
        public DateTime Date { get => this.Model.Date; }
        public string Token { get => this.Model.Token; }
        public DateTime? Expired { get => this.Model.Expired; }
        public int UserID { get => this.Model.UserID; set => this.Model.UserID = value; }
        public UserController User { get; set; } = default!;


        public async Task<UserController> GetUser() {
            var result = await this.Context.Users.FindAsync(this.UserID);
            if (result == null)
                throw new Exception("Unable to retrieve Login.User");
            this.User = result;
            return this.User;
        }
            
           
    }
}
