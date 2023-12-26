using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Controllers {
    public class GroupController : Controller<Group> {
        public GroupController(Context context, Group group) : base(context, group) { }
    }
}
