using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Address : Model {
        public string Street { get; set; } = default!;
        public string? Subpremise { get; set; }
        public string Zip { get; set; } = default!;
        
        [ForeignKey(nameof(City))] 
        public int CityID { get; set; } = default!;
        public virtual City City { get; set; } = default!;

        public class Configuration : IEntityTypeConfiguration<Address> {
            public void Configure(EntityTypeBuilder<Address> builder) {
                builder.HasData(
                    new Address {
                        ID = 1,
                        Street = "2500 S Independence Blvd",
                        Zip = "23456",
                        CityID = 1
                    }
                );
            }
        }

    }
    public class State : Model {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public class Configuration : IEntityTypeConfiguration<State> {
            public void Configure(EntityTypeBuilder<State> builder) {
                builder.HasData(
                    new State {
                        ID = 1,
                        Code = "VA",
                        Name = "Virginia"
                    }
                );
            }
        }

    }
    public class City : Model {
        public string Name { get; set; } = default!;        
        
        [ForeignKey(nameof(State))] 
        public int StateID { get; set; } = default!;
        public virtual State State { get; set; } = default!;
        public class Configuration : IEntityTypeConfiguration<City> {
            public void Configure(EntityTypeBuilder<City> builder) {
                builder.HasData(
                    new City {
                        ID = 1,
                        Name = "Virginia Beach",
                        StateID = 1
                    }
                );
            }
        }
    }
}
