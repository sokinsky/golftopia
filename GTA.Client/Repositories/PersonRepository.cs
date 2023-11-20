using GTA.Client.Controllers;
using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.Repositories {
    public class PersonRepository : Repository<Person, PersonController> {
        public PersonRepository(Context context) : base(context) { }
    }
}
