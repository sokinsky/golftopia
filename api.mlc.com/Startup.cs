using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api.mlc.com {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllers(config => {
				config.Filters.Add(new Filters.ActionFilter());
				config.Filters.Add(new Filters.ExceptionFilter());

            });
			services.AddDbContext<GTA.Server.Context>(options => options.UseSqlServer(Configuration["ConnectionStrings:GTA"]));
			services.AddCors(options =>
				options.AddDefaultPolicy(builder =>
					builder
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						//.AllowCredentials()
				)
			);

			//	options.AddDefaultPolicy(builder => {
			//		builder.WithOrigins("http://localhost:8081/").AllowAnyOrigin();
			//	}
			//));
			services.AddMvc().
				AddJsonOptions(options => {
					options.JsonSerializerOptions.PropertyNamingPolicy = null;
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
					//options.JsonSerializerOptions.Converters.Add(new MLC.ValidationSerializer());
					//options.JsonSerializerOptions.Converters.Add(new MLC.Data.ModelSerializer());
					options.JsonSerializerOptions.Converters.Add(new GTA.Server.ControllerSerializer());
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
				builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
			);

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
