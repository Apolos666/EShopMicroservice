namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        //// Add Carter
        //services.AddCarter();
        //// Add HealthChecks
        //services.AddHealthChecks();
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        //app.UseHealthChecks("/health");
        //app.UseRouting();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapCarter();
        //});
        return app;
    }
}