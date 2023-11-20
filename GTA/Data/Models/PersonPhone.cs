using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class PersonPhone : Model {
        [ForeignKey(nameof(Person))]
        public int PersonID { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;

        [ForeignKey(nameof(Phone))]
        public int PhoneID { get; set; } = default!;
        public virtual Phone Phone { get; set; } = default!;

        public string Code { get; set; } = Guid.NewGuid().ToString();
        public DateTime? Verified { get; set; }

        public class Configuration : IEntityTypeConfiguration<PersonPhone> {
            public void Configure(EntityTypeBuilder<PersonPhone> builder) {
                builder.HasAlternateKey(nameof(PersonPhone.PersonID), nameof(PersonPhone.PhoneID));
                //builder.HasData(new PersonPhone { ID = 1, PersonID = 1, PhoneID = 1 });
            }
        }
    }
}
