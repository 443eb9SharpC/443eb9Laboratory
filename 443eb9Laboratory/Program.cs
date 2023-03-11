namespace _443eb9Laboratory;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                //webBuilder.UseKestrel(options =>
                //{
                //    // 配置Kestrel以接受SSL连接
                //    options.ListenAnyIP(5001, listenOptions =>
                //    {
                //        listenOptions.UseHttps();
                //    });
                //});
            });
}