using GTA.Data.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GTA.Server.Repositories {
    public class CourseRepository : Repository<Course, Controller<Course>>{
        public CourseRepository(Context context) : base(context) { }

        public async Task<Course> Create(string name) {
            var response = await Services.Courses.RapidAPI.Client.Search(name);
            var payload = JsonSerializer.Deserialize<Services.Courses.RapidAPI.Search.Response.Payload>(response.Body);
            if (payload == null)
                throw new Exception("");

            var result = new Course {
                Name = payload.name
            };
            foreach (var teebox in payload.teeBoxes) {
                var 
            }



        }
        private Task<Course> load(Services.Courses.RapidAPI.Search.Response.Payload payload) {

            var result = this.Context.Courses.
            var result = new Course {
                Name = payload.name
            };
            var teeBoxes = payload.teeBoxes.Select(x => create(x));
            foreach (var teeBox in teeBoxes) {
                this.Context.TeeBoxes.Add(teeBox);
            }
            var holes = 
        }
        private async Task<TeeBox> add(Course course, Services.Courses.RapidAPI.Search.Response.Payload.Teebox payload) {
            var result = await this.Context.TeeBoxes.FirstOrDefaultAsync(x => x.CourseID == course.ID && x.Name == payload.tee);
            if (result == null) {
                result = new TeeBox {
                    Course = course,
                    Name = payload.tee
                };
                this.Context.Add(result);
            }
            return result;
        }
        private Hole 
    }
}
