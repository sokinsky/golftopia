using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Team : Model {
        public string Name { get; set; } = default!;
    }
    public class TeamPerson : Model  {
        public int PersonID { get; set; }
        public virtual Person Person { get; set; } = default!;
        public int TeamID { get; set; }
        public virtual Team Team { get; set; } = default!;
    }
}
