using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Zhihu.HttpApi.Common;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseHttpApiCommon(this WebApplication app)
    {
        app.MapOpenApi();
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                options.WithOpenApiRoutePattern("/openapi/v1.json");
                options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        app.UseCors("AllowAny");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler(_ => { });

        app.MapControllers();

        return app;
    }
}