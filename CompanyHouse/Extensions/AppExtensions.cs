using API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyHouse API V1");
            });
        }
        public static void UseMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
