using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Match : Event {
        public int CourseID { get; set; }
        public Course Course { get; set; } = default!;
    }

    public class Player : Person {

    }


}
