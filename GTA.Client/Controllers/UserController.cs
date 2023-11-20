using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.Controllers {
    public class UserController : Controller<User> {
        public UserController(Context context, User user) : base(context, user) { }
        public string Username { get => this.Model.Username; set => this.Model.Username = value; }
        public string Password { get => this.Model.Password; set => this.Model.Password = value; }
        public PersonController Person { get; set; } = default!;
    }
}
