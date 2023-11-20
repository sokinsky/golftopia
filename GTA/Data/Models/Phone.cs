using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Phone : Model {
        public string Number { get; set; } = default!;
        public class Configuration : IEntityTypeConfiguration<Phone> {
            public void Configure(EntityTypeBuilder<Phone> builder) {
                builder.HasAlternateKey(nameof(Phone.Number));
                //builder.HasData(
                //    new Phone { ID = 1, Number = "7576926223" }
                //);
                
            }
        }
    }
}
