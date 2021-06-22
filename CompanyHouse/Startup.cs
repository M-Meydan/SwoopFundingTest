using API.Attributes;
using API.Extensions;
using API.Filters;
using API.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers((options => { options.Filters.Add<ValidateModelAttribute>(); }))
               .ConfigureApiBehaviorOptions((options => { options.SuppressModelStateInvalidFilter = true; }))
               .AddJsonOptions(options => {
                   options.JsonSerializerOptions.WriteIndented = true;
                   options.JsonSerializerOptions.IgnoreNullValues = true;
                   options.JsonSerializerOptions.PropertyNamingPolicy = null; //sets pascal case
                });

            services.AddLogging();
            services.AddFluentValidationExtension();
            services.AddSwaggerExtension();
            //services.AddMvc(options =>
            //{
            //    options.CacheProfiles.Add("Default", new CacheProfile() { Location= ResponseCacheLocation.Client, Duration = Configuration.GetValue<int>("CacheDuration") });
            //});
            //services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddCompaniesHouseClient(Configuration.GetValue<string>("CompaniesHouseAPIKey"));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<CacheResourceFilter>();
            services.AddSingleton<ITestableCache, TestableCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseSwaggerExtension();

            app.UseMiddlewares();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
