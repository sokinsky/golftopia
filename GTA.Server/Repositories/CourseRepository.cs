using GTA.Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GTA.Server.Repositories {
    public class CourseRepository : Repository<Course, Controller<Course>>{
        public CourseRepository(Context context) : base(context) { }

        [Web.Method]
        public async Task<Course> Extract() {
            var course = new Course();

            var html = "";
#if DEBUG
            var stream = typeof(GTA.Data.Model).Assembly.GetManifestResourceStream("GTA.honey.html");
            if (stream != null) {
                using var streamReader = new StreamReader(stream);
                html = streamReader.ReadToEnd();
            }
#endif
#if RELEASE
            var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage {
                RequestUri = new Uri("https://courses.swingu.com/courses/United-States-of-America/Virginia/Virginia-Beach/Honey-Bee-Golf-Club/31804"),
                Method = HttpMethod.Get
            };
            var httpResponse = await httpClient.SendAsync(httpRequest);
            if (httpResponse.IsSuccessStatusCode) {
                var html = await httpResponse.Content.ReadAsStringAsync();

            }
#endif


            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var htmlScorecard = htmlDocument.DocumentNode.SelectNodes($"//div[@class='vertical-scorecard visible-xs']");
            var htmlTees = htmlScorecard.First().SelectNodes($"//tbody");
            var htmlSummary = htmlTees.First();
            htmlSummary = htmlSummary.SelectSingleNode("//tr");

            foreach (var htmlTee in htmlTees.Skip(1)) {
                var htmlHoles = htmlTee.SelectNodes($"//tr");
                foreach (var htmlHole in htmlHoles.Skip(1)) {

                }
            }


            return course;

        }

        private Course.Tee extractTee(HtmlNode htmlNode) {
            if (htmlNode.Name != "tbody")
                throw new Exception("Invalid Course.Tee.Hole Node");
            var htmlTees = htmlNode.SelectNodes("//tr");
            var styles = htmlTees.First().Attributes["style"].Value;
            var backgroundColor = Regex.Match(styles, "background-color:\\s(\\.);*");

            var htmlTee = htmlTees.Last().SelectNodes("table");


            foreach (var htmlTee in htmlTees) {

            }

        }
        private Course.Tee.Hole extractHole(HtmlNode htmlNode) {
            if (htmlNode.Name != "tr")
                throw new Exception("Invalid Course.Tee.Hole Node");
            var htmlData = htmlNode.SelectNodes("//td");
            switch (htmlData.Count()) {
                case 4:
                    var result = new Course.Tee.Hole();
                    result.Number = int.Parse(Regex.Match(htmlData.First().InnerText, "(\\d+)").Groups[1].Value);
                    result.Par = int.Parse(htmlData.Skip(1).First().InnerText);
                    var distance = htmlData.Skip(2).First().InnerText;
                    distance = Regex.Match(distance, "(\\d+)yd").Groups[1].Value;
                    result.Distance = int.Parse(distance);
                    result.Index = int.Parse(htmlData.Skip(3).First().InnerText);
                    return result;
                default:
                    throw new Exception("");
            }
           
        }
    }
}
