using GTA.Data.Models;
using GTA.Server.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GTA.Server.Repositories {
    public class PersonRepository : Repository<Person, PersonController> {
        public PersonRepository(Context context) : base(context) { }

        [Web.Method]
        public async Task<IEnumerable<PersonController>> Search(string name) {
            var models = this.Context.People.Where(x => x.FirstName.Contains(name));
            return (await models.ToListAsync()).Select(x => this[x]);
        }
    }
}
