using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Match : Event {
        public int CourseID { get; set; }
        public Course Course { get; set; } = default!;
        public virtual IEnumerable<MatchResult> Results { get; set; } = Enumerable.Empty<MatchResult>();
    }
    public class MatchResult : Model {
        public int MatchID { get; set; }
        public virtual Match Match { get; set; } = default!;
        public int TeamID { get; set; }
        public virtual Team Team { get; set; } = default!;

        public string Value { get; set; } = default!;
    }
}
