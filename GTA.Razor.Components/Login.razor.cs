using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Razor.Components {
    public class LoginBase : Base{
        public Form form { get; set; } = new Form { Username = "sokinsky", Password = "musk4rat!"};
        public async Task login() {
            var login = await this.Context.Users.Login(form.Username, form.Password);
            if (login == null)
                throw new Exception("Invalid Login");

            await this.LocalStorage.SetItemAsync<string>("login", login.Token);
        }

        public class Form {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}
