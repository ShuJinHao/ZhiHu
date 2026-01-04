using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Zhihu.HttpApi.Common;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseHttpApiCommon(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        app.MapOpenApi();

        app.UseCors("AllowAny");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler(_ => { });

        return app;
    }
}