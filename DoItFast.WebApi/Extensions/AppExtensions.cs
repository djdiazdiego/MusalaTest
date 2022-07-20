using DoItFast.WebApi.Middlewares;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace DoItFast.WebApi.Extensions
{
    public static class AppExtensions
    {
        /// <summary>
        /// Add swagger middleware
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // For Debug in Kestrel
                c.SwaggerEndpoint("v1/swagger.json", "DoItFast.WebApi v1");
                c.RoutePrefix = "swagger";
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

            });
        }

        /// <summary>
        /// Middeware for catch flow errors
        /// </summary>
        /// <param name="app"></param>
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ErrorHandlerMiddleware>();

        //public static void UseJwtMiddleware(this IApplicationBuiSlder app)
        //    => app.UseMiddleware<JwtMiddleware>();

        /// <summary>
        /// Middeware for spa
        /// </summary>
        /// <param name="app"></param>
        public static void UseSpa(this WebApplication app)
        {
            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = app.Environment.IsDevelopment() ? "./" : "wwwroot";
                spa.Options.SourcePath = "ClientApp";

                if (app.Environment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

        /// <summary>
        /// Middeware for cors
        /// </summary>
        /// <param name="app"></param>
        public static void UseCors(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseCors(p =>
                {
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                    p.AllowCredentials();
                    p.WithOrigins("http://localhost:4200");
                });
            }
        }
    }
}
