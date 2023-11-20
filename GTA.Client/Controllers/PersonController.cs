using GTA.Data.Models;

namespace GTA.Client.Controllers {
    public class PersonController : Controller<Person> {
        public PersonController(Context context, Person person) : base(context, person) { }
        public string FirstName { get => this.Model.FirstName; set => this.Model.FirstName = value; }
        public string LastName { get => this.Model.LastName; set => this.Model.LastName = value; }
    }
}
