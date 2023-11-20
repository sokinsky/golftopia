using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Identity.Client;
using site.golftopia.com;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();


var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(sp => http);

builder.Services.AddScoped<GTA.Razor.Components.ExampleJsInterop>();
builder.Services.AddScoped<GTA.Razor.Services.TestService>();
builder.Services.AddScoped<GTA.Razor.Services.FacebookService>();

using var contextResponse = await http.GetAsync("context.json");
var contextConfiguration = JsonSerializer.Deserialize<GTA.Client.Context.Configuration>(await contextResponse.Content.ReadAsStringAsync());
if (contextConfiguration == null)
    throw new Exception("");
var context = new GTA.Client.Context(contextConfiguration);
builder.Services.AddSingleton(x => context);

await builder.Build().RunAsync();
