using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Group : Model {
        public string Name { get; set; } = default!;

        public int? ParentID { get; set; }
        public virtual Group? Parent { get; set; }
        public virtual IEnumerable<Group> Children { get; set; } = Enumerable.Empty<Group>();

        public class Configuration : IEntityTypeConfiguration<Group> {           
            public void Configure(EntityTypeBuilder<Group> builder) {
                builder.HasAlternateKey(nameof(Group.ParentID), nameof(Group.Name));
            }
        }
    }

    public class GroupPerson : Model {
        public int PersonID { get; set; }
        public int GroupID { get; set; }

        public virtual Person Person { get; set; } = default!;        
        public virtual Group Group { get; set; } = default!;
    }

    

}
