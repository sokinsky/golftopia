using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Round : Event {
        public int PersonID { get; set; }
        public virtual Person Person { get; set; } = default!;
        public int CourseID { get; set; }
        public virtual Course Course { get; set; } = default!;
    }
}
