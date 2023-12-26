using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTA.Data.Models {
    public class Person : Model {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;


        public class Configuration : IEntityTypeConfiguration<Person> {
            public void Configure(EntityTypeBuilder<Person> builder) {
                builder.HasData(
                    new Person { ID = 1, FirstName = "Steve", LastName = "Okinsky" },
                    new Person { ID = 2, FirstName = "David", LastName = "Okinsky" },
                    new Person { ID = 3, FirstName = "Nathan", LastName = "Pannell" },
                    new Person { ID = 4, FirstName = "Bob", LastName = "Souls" },
                    new Person { ID = 5, FirstName = "Ryan", LastName = "Wheeler" },
                    new Person { ID = 6, FirstName = "Mathew", LastName = "Hess" },
                    new Person { ID = 7, FirstName = "Brian", LastName = "Corr" },
                    new Person { ID = 8, FirstName = "Danny", LastName = "Martel" }
                );
                
            }
        }
    }

    public class Player : Model {
        [Key, ForeignKey(nameof(Person))]
        public override int ID { get; set; }
        public virtual Person Person { get; set; } = default!;

    }


}
