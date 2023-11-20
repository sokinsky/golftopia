using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.X.Repositories {
    public class UserRepository {
        [Fact]
        public async Task Login() {
            var context = new GTA.Client.Context();
            var login = await context.Users.Login("sokinsky", "musk4rat!");
            Assert.NotNull(login);
            login.User = await login.GetUser();
            Assert.NotNull(login.User);
        }
    }
}
