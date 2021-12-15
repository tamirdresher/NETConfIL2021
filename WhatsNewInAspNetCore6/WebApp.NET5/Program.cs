using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var startup = new WebApp.NET5.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();


//CreateHostBuilder(args).Build().Run();


//static IHostBuilder CreateHostBuilder(string[] args) =>
//   Host.CreateDefaultBuilder(args)
//       .ConfigureWebHostDefaults(webBuilder =>
//       {
//           webBuilder.UseStartup<WebApp.NET5.Startup>();
//       });


