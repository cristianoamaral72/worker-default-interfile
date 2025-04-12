using Serilog;
using Worker.Service.Extensions;
using WorkerRoboCadastros;

namespace WorkerRoboSeo;

public class Program
{
    public static void Main(string[] args)
    {
        // Validar se a paste de Logs existe, se não existir, criar
        string logDirectory = "C:\\Logs";

        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // Defina o nível mínimo de log
            .WriteTo.File($"{logDirectory}\\log_WorkerRoboDeCadastro.txt", rollingInterval: RollingInterval.Day) // Define o caminho do arquivo de log
            .CreateLogger();

        Log.Information("Iniciando o worker...");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureAppConfiguration((host, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
                config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}", optional: true);
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(dispose: true);
                });
                services.AddOptions();
                services.AddLogging();

                services.AddHostedService<WorkerRoboCadastro>();
                services.AddServices(hostContext.Configuration);

            });


}