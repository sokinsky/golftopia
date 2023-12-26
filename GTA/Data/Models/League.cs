using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class League : Model {
    }
    public class Season : Model {
        public string Name { get; set; } = default!;
        public int LeagueID { get; set; }
        public virtual League League { get; set; } = default!;
    }
}
