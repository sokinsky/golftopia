using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Client.X {
    
    public class Context {

        protected GTA.Client.Context context = new GTA.Client.Context();
    }
    public class ChangeTracker {
        [Fact]
        public async Task Test() {
            var context = new Client.Context();
            var person = await context.People.FindAsync(1);
            Assert.NotNull(person);
        }
    }
}
