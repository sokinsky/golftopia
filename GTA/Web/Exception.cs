using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Web {
    public class Exception : System.Exception {
        public HttpStatusCode StatusCode { get; set; }
        public Exception(string message) : base(message){ }

        public static Exception BadRequest(string message) {
            return new Exception(message) { StatusCode = HttpStatusCode.BadRequest };
        }
        public static Exception Unauthorized(string message) {
            return new Exception(message) { StatusCode = HttpStatusCode.Unauthorized };
        }
        public static Exception NotFound(string message) {
            return new Exception(message) { StatusCode = HttpStatusCode.NotFound };
        }
    }
}
