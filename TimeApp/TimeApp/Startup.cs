using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeApp.Foundation.Blobs;
using TimeApp.Foundation.TimeZone;
using TimeApp.Options;
using TimeApp.Repositories.TimeZone;

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
            services.Configure<AzureCosmosOptions>(_configuration.GetSection(AzureCosmosOptions.SectionName));
            services.Configure<AzureBlobOptions>(_configuration.GetSection(AzureBlobOptions.SectionName));

            services.AddScoped<ITimeZoneService, TimeZoneService>();
            services.AddSingleton<ITimeZoneRepository, TimeZoneRepository>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddSingleton<IBlobCleanupService, BlobCleanupService>();

            services.AddHostedService<TimeAppHostedService>();

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