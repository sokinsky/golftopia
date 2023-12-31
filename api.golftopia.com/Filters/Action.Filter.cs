﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.mlc.com.Filters {
    public class ActionFilter : IAsyncActionFilter {
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
			switch(context.Controller) {
				case IDataController dataController:
					var headers = new Dictionary<string, IEnumerable<string>>();
					foreach (var header in context.HttpContext.Request.Headers) {
						headers.Add(header.Key, context.HttpContext.Request.Headers[header.Key]);
						switch (header.Key.ToLower()) {
							case "login":
								dataController.Context.Login = await dataController.Context.Repositories.Logins.ByToken(header.Value.Single());
								break;

						}
					}
					dataController.Context.RequestHeaders = headers;					
					break;
			}
			await next();
		}
	}
}
