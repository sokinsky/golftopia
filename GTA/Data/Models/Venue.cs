using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTA.Data.Models {
    public class Venue : Model {
        public string Name { get; set; } = default!;
        public int? ParentID { get; set; }
        public virtual Venue? Parent { get; set; }

        [ForeignKey(nameof(Address))]
        public int? AddressID { get; set; } = default!;
        public virtual Address? Address { get; set; } = default!;

        [ForeignKey(nameof(Phone))]
        public int? PhoneID { get; set; } = default!;
        public virtual Phone? Phone { get; set; } = default!;

        public class Configuration : IEntityTypeConfiguration<Venue> {
            public void Configure(EntityTypeBuilder<Venue> builder) {
                builder.HasData(
                    new Venue { 
                        ID = 1, 
                        Name = "Honey Bee Golf Club", 
                        AddressID = 1
                    }
                );
            }
        }
    }

}
