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
    }
}
