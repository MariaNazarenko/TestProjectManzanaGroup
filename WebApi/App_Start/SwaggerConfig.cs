using System.Web.Http;
using System.Linq;
using WebActivatorEx;
using WebApi;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApi
{
    /// <summary>
    /// Конфигуратор сваггера
    /// </summary>
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "WebApi");
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("My Swagger UI");
                    });
        }
    }
}
