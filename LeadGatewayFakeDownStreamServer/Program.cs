using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LeadGatewayFakeDownStreamServer
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel((context, options) =>
                {
                    options.ListenAnyIP(9001);
                });
    }

    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger) => _logger = logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) => app.Use(next => SimpleMiddleware);

        private async Task SimpleMiddleware(HttpContext context)
        {
            await Task.Delay(500);

            _logger.LogWarning("Logging request!");
            context.Response.StatusCode = StatusCodes.Status200OK;

            context.Response.ContentType = MediaTypeNames.Application.Json;

            var jsonString = JsonConvert.SerializeObject("All good");

            await context.Response.WriteAsync(jsonString, Encoding.UTF8);
        }
    }
}