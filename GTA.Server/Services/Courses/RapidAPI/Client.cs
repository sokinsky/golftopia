using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Services.Courses.RapidAPI {
    public class Client {
        public static async Task<Search.Response> Search(string name) {
            return await new Search.Request().Execute(name);
        }
    }
}
