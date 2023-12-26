using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Services.Courses.RapidAPI {

    /// <summary>
    /// https://rapidapi.com/foshesco-65zCww9c1y0/api/golf-course-api
    /// </summary>
    public class Configuration {
        public string BaseUrl { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string Host { get; set; } = default!;

        public static Configuration Production => new Configuration {
            BaseUrl = "https://golf-course-api.p.rapidapi.com",
            Key = "f32f0a1199msh746a2893e709b4ep161d82jsn80d28b799686",
            Host = "golf-course-api.p.rapidapi.com"
        };

    }
}
