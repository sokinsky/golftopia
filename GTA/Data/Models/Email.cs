using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Email : Model {
        public string Address { get; set; } = default!;
        public class Configuration : IEntityTypeConfiguration<Email> {
            public void Configure(EntityTypeBuilder<Email> builder) {
                builder.HasAlternateKey(nameof(Email.Address));
                builder.HasData(new Email { ID = 1, Address = "sokinsky@gmail.com" });
            }
        }
    }
}
