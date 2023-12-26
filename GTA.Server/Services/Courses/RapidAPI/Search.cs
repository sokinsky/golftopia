using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Services.Courses.RapidAPI {
    public class Search {
        public class Request {
            public async Task<Response> Execute(string name) {
                var client = new HttpClient();
                var request = new HttpRequestMessage {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{Configuration.Production.BaseUrl}/search?name={name}"),
                    Headers = {
                        { "X-RapidAPI-Key", Configuration.Production.Key },
                        { "X-RapidAPI-Host", Configuration.Production.Host }
                    }
                };

                using var response = await client.SendAsync(request);
                return new Response {
                    Status = response.StatusCode,
                    Body = await response.Content.ReadAsStringAsync()
                };
            }
        }
        public class Response {
            public System.Net.HttpStatusCode Status { get; set; } = default!;
            public string Body { get; set; } = default!;

            public class Payload {
                public string __id { get; set; } = default!;
                public string name { get; set; } = default!;
                public string phone { get; set; } = default!;
                public string address { get; set; } = default!;
                public string city { get; set; } = default!;
                public string state { get; set; } = default!;
                public string zip { get; set; } = default!;
                public string country { get; set; } = default!;
                public string cooridinates { get; set; } = default!;
                public string holes { get; set; } = default!;

                public Scorecard[] scorecard { get; set; } = default!;
                public Teebox[] teeBoxes { get; set; } = default!;

                public class Scorecard {
                    public int Hole { get; set; }
                    public int Par { get; set; }
                    public Dictionary<string, Teebox> tees { get; set; } = default!;
                    public int Handicap { get; set; }

                    public class Teebox {
                        public string color { get; set; } = default!;
                        public int yards { get; set; }

                    }

                }
                public class Teebox {
                    public string tee { get; set; } = default!;
                    public int slope { get; set; } = default!;
                    public double handicap { get; set; } = default!;
                }


            }
        }
    }
}
