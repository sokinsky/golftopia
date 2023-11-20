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
    public class Club : Venue {

    }
    public class Course : Venue {       
        [Column("Tees")]
        public string json_Tees { get; set; } = "[]";
        [NotMapped]
        public Tee[] Tees {
            get {
                var result = JsonSerializer.Deserialize<Tee[]>(this.json_Tees);
                if (result == null)
                    return new Tee[] { };
                return result;
            }
            set {
                this.json_Tees = JsonSerializer.Serialize(value);
            }
        }

        public class Tee {
            public string Name { get; set; } = default!;
            public string Color { get; set; } = default!;
            public int Slope { get; set; }
            public float Rating { get; set; }
            public Hole[] Holes { get; set; } = new Hole[] { };
            public class Hole {
                public int Number { get; set; }
                public int Par { get; set; }
                public int Index { get; set; }
                public int Distance { get; set; }
            }
        }

        public new class Configuration : IEntityTypeConfiguration<Course> {
            public void Configure(EntityTypeBuilder<Course> builder) { }
        }
    }
}
