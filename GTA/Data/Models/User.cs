using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTA.Data.Models {
    public class User : Model {
        [Key, ForeignKey(nameof(Person))]
        public override int ID { get; set; }

        public virtual Person Person { get; set; } = default!;

        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;

        public class Configuration : IEntityTypeConfiguration<User> {
            public void Configure(EntityTypeBuilder<User> builder) {
                builder.HasAlternateKey(nameof(User.Username));
                builder.HasData(new User { ID = 1, Username = "sokinsky", Password = "musk4rat!" });
            }
        }
    }
}
