using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTA.Data.Models {
    public class PersonEmail : Model {
        [ForeignKey(nameof(Person))]
        public int PersonID { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;

        [ForeignKey(nameof(Email))]
        public int EmailID { get; set; } = default!;
        public virtual Email Email { get; set; } = default!;

        public string Code { get; set; } = Guid.NewGuid().ToString();
        public DateTime? Verified { get; set; }

        public class Configuration : IEntityTypeConfiguration<PersonEmail> {
            public void Configure(EntityTypeBuilder<PersonEmail> builder) {
                builder.HasAlternateKey(nameof(PersonEmail.PersonID), nameof(PersonEmail.EmailID));
                builder.HasData(new PersonEmail { ID = 1, PersonID = 1, EmailID = 1 });              
            }
        }
    }
}
