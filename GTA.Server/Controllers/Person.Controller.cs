using GTA.Data.Models;
using GTA.Server.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GTA.Server.Controllers {
    public class PersonController : Controller<Person>{
        public PersonController(Context context, Person person) : base(context, person) { }

        [Web.Method]
        public async Task<Controller<Login>> Register(string username, string password) {
            var userModel = await this.Context.Users.SingleOrDefaultAsync(x => x.Person == this.Model);
            if (userModel != null)
                throw Web.Exception.BadRequest("Person is already a user");
            if (await this.Context.Users.AnyAsync(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
                throw Web.Exception.BadRequest("Username is already in use");
            userModel = new User {
                Person = this.Model,
                Username = username,
                Password = password
            };
            await this.Context.SaveChangesAsync();
            var userRepository = new UserRepository(this.Context);
            return await userRepository.Login(username, password);
        }

    }
}
