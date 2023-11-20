using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.X {
    public class Repository {
        [Fact]
        public async Task FindAsync() {
            var context = new GTA.Client.Context();
            var person = await context.People.FindAsync(1);
            Assert.NotNull(person);
            Assert.Single(context.People);
            Assert.Equal(Microsoft.EntityFrameworkCore.EntityState.Unchanged, person.ModelState);
            Assert.Equal("Steve", person.FirstName);
            person.FirstName = "Stephen";
            Assert.Equal(Microsoft.EntityFrameworkCore.EntityState.Modified, person.ModelState);
            Assert.Equal("Stephen", person.FirstName);
            person = await context.People.FindAsync(1);
            Assert.NotNull(person);
            Assert.Single(context.People);
            Assert.Equal(Microsoft.EntityFrameworkCore.EntityState.Modified, person.ModelState);
            Assert.Equal("Stephen", person.FirstName);
            
        }
    }
}
