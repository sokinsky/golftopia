using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LMS.Data.Server {
    public class Exception : System.Exception {
        public Exception(HttpStatusCode status, object message) { this.StatusCode = status; this.Message = message; }
        public Exception(System.Exception systemException) {
            switch (systemException) {
                case DbUpdateException dbUpdateException:
                    switch (dbUpdateException.InnerException) {
                        case SqlException sqlException:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    this.StatusCode = HttpStatusCode.InternalServerError;
                    this.Message = systemException.Message;
                    break;
            }

        }
        public Exception(string message) : this(new System.Exception(message)) { }
        public HttpStatusCode StatusCode { get; set; }
        public new object Message { get; set; } = default!;

        public static Exception BadRequest(object message) {
            return new Exception(HttpStatusCode.BadRequest, message);
        }
        public static Exception Unauthorized(object message) {
            return new Exception(HttpStatusCode.Unauthorized, message);
        }
        public static Exception NotFound(object message) {
            return new Exception(HttpStatusCode.NotFound, message);
        }
    }
}
