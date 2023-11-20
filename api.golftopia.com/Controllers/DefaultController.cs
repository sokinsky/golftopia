using GTA.Data;
using GTA.Server;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace api.Controllers {
	public interface IDataController {
		public Context Context { get; set; }
	}
    public class DefaultController : ControllerBase {
		public DefaultController(Context context) {
			this.Context = context;
		}
		public Context Context { get; set; }
		[HttpGet("api/{repositoryName}")]
		public async Task<object> Get(string repositoryName) {
			return await this.Invoke(repositoryName, "search", new { });		
		}

		[HttpGet("api/{repositoryName}/{id}")]
		public async Task<IController<Model>> Get(string repositoryName, int id) {
			var result = await this.Invoke(repositoryName, "findasync", new { id });
			if (result != null)
				return (IController<Model>)result;
			return null;
		}
		[HttpPut("api/{repositoryName}")]
		public async Task<IController<Model>> Put(string repositoryName, [FromBody] object model) {
			var result = this.Context.Repository(repositoryName).Add(model);
			await this.Context.SaveChangesAsync();
			return result;
		}
		[HttpPatch("api/{repositoryName}/{id}")]
		public async Task<IController<Model>> Patch(string repositoryName, int id, [FromBody] object model) {
			var controller = await this.Get(repositoryName, id);
			controller.Import(model);
			await this.Context.SaveChangesAsync();
			return controller;
		}
		[HttpPost("api/{methodName}")]
		public async Task<object> Invoke(string methodName, [FromBody] object parameters) {
			return await GTA.Web.Method.Invoke(this.Context, methodName, parameters);
		}
		[HttpPost("api/{repositoryName}/{methodName}")]
		public async Task<object> Invoke(string repositoryName, string methodName, [FromBody] object parameters) {
			return await GTA.Web.Method.Invoke(this.Context.Repository(repositoryName), methodName, parameters);
		}
		[HttpPost("api/{repositoryName}/{id}/{methodName}")]
		public async Task<object> Invoke(string repositoryName, int id, string methodName, [FromBody] object parameters) {
			var controller = await this.Invoke(repositoryName, "findasync", new { id });
			return await GTA.Web.Method.Invoke(controller, methodName, parameters);
		}
		[HttpDelete("api/{repositoryName}/{id}")]
		public async Task<object> Delete(string repositoryName, int id) {
			var model = this.Context.Repository(repositoryName).Remove(id);
			await this.Context.SaveChangesAsync();
			return model;
		}
	}
}
