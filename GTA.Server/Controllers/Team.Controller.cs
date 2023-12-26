using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Controllers {
    public class TeamController : Controller<Team> {
        public TeamController(Context context, Team team) : base(context, team) { }

    }
}
