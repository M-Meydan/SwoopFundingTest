using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CompanyHouse API", Version = "v1" });
                c.EnableAnnotations();
            });
        }

        public static void AddFluentValidationExtension(this IServiceCollection services)
        {
            services.AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
                config.DisableDataAnnotationsValidation = false;
            });
        }
    }
}
