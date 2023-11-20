using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace api.mlc.com.Filters {
    public class ExceptionFilter : IExceptionFilter {
        public void OnException(ExceptionContext context) {
            switch (context.Exception) {
                case GTA.Web.Exception apiException:
                    context.Result = new ContentResult() {
                        StatusCode = (int)apiException.StatusCode,
                        Content = JsonSerializer.Serialize(apiException.Message)
                    };
                    break;
                default:
                    context.Result = new ContentResult {
                        Content = context.Exception.ToString()
                    };
                    break;
            }
        }
    }

    public class ExceptionFilterAttribute  : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            switch (context.Exception) {
                case GTA.Web.Exception apiException:
                    break;
                default:
                    base.OnException(context);
                    break;
            }            
        }

        public class ExceptionResult : ActionResult {
            public ExceptionResult(GTA.Web.Exception exception) {
                
            }
        }
    }
}
