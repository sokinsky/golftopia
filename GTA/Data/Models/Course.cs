using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Identity.Client;

namespace GTA.Data.Models {
    public class Course : Model {
        public string Name { get; set; } = default!;
        public int? PhoneID { get; set; }
        public virtual Phone? Phone { get; set; }
        public int? AddressID { get; set; }
        public virtual Address? Address { get; set; }

        public virtual ICollection<TeeBox> TeeBoxes { get; set; } = default!;
    }
    public class TeeBox : Model {
        public string Name { get; set; } = default!;
        public int CourseID { get; set; }
        public virtual Course Course { get; set; } = default!;

    }
    public class Hole : Model {
        public int Number { get; set; }
        public int Par { get; set; }
        public int Index { get; set; }
        public int Yards { get; set; }

        public int TeeBoxID { get; set; }
        public virtual TeeBox TeeBox { get; set; } = default!;
        
    }

    public class ScoreCard : Model {
        public int PersonID { get; set; }
        public virtual Person Person { get; set; } = default!;

        public int CourseID { get; set; }
        public virtual Course Course { get; set; } = default!;

        public virtual ICollection<ScoreCardHole> Holes { get; set; } = default!;


    }
    public class ScoreCardHole : Model {
        public int ScoreCordID { get; set; }
        public virtual ScoreCard ScoreCard { get; set; } = default!;

        public int HoleID { get; set; }
        public virtual Hole Hole { get; set; } = default!;

        public int Score { get; set; }
    }
}
