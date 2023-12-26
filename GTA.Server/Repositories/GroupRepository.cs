using GTA.Data.Models;
using GTA.Server.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Repositories {
    public class GroupRepository : Repository<Group, GroupController> {
        public GroupRepository(Context context) : base(context) { }


    }
}
