using _443eb9Laboratory;
using Microsoft.AspNetCore.HttpOverrides;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //services.AddCors(options =>
        //{
        //    options.AddPolicy(
        //        "AllowAll",
        //        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        //});
        services.AddControllersWithViews();
        services.AddSignalR();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // 配置中间件
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        //app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        //app.UseCors("AllowCors");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<WebHub>("/hub", options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
            });
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}