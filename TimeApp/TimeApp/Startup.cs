using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeApp.Foundation.Blobs;
using TimeApp.Foundation.TimeData;

namespace TimeApp
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureOptions>(_configuration.GetSection(AzureOptions.SectionName));

            services.AddSingleton<ITimeDataRepository, TimeDataRepository>();
            //services.AddSingleton<ITimeDataRepository, InMemoryTimeDataRepository>();
            services.AddSingleton<IBlobService, BlobService>();



            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*");
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "API",
                    "api/{ControllerBase=Home}/{action=Index}");
            });
        }
    }
}