using Microsoft.Extensions.DependencyInjection;
using System;
using Worker.Data.Configs;
using Worker.Data.Contexts;
using Worker.Data.Repositories;
using Worker.Data.Repositories.Base;
using Worker.Domain.Entities.Autenticacao;
using Worker.Domain.Interfaces.Base;
using Worker.Domain.Interfaces.Configs;
using Worker.Domain.Interfaces.Repositories;
using Worker.Domain.Interfaces.Services;
using Worker.Domain.Interfaces.Services.Base;
using Worker.Domain.Interfaces.Services.Chrome;
using Worker.Service.Services;
using Worker.Service.Services.Autenticacao;
using Worker.Service.Services.Chrome;
using Worker.Service.Services.Jobs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using INavigator = Worker.Domain.Interfaces.Services.Chrome.INavigator;

namespace Worker.Service.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           
            // Carregar o ApplicationSettings da configuração (appsettings.json)
             services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));

            services.AddDatabaseContext<DapperContext>(opt =>
            {
                opt.ConnString = BuscarConnectionString(configuration);
            });

            // Registre o DapperContext
            services.AddSingleton<DapperContext>();
            services.AddHttpClient();

            services.AddHttpClient("google", client =>
            {
                client.BaseAddress = new Uri("https://google.com/");
            });

            //Repository
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IVivoCargaRepository, VivoCargaRepository>();

            // Services
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IVivoCargaService, VivoService>();

            services.AddTransient<IColetaDadosJob, ColetaDadosJob>();
            services.AddTransient<INavigator, Navigator>();
            services.AddTransient<IWorkerRoboCadastro, WorkerRoboCadastro>();
            services.AddSingleton<IDriverFactoryService>(_ =>
            {
                var driverFactory = new DriverFactoryService();
                AppDomain.CurrentDomain.ProcessExit += (_, _) => driverFactory?.Instance?.Quit();
                AppDomain.CurrentDomain.UnhandledException += (_, _) => driverFactory?.Instance?.Quit();
                return driverFactory;
            });


            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            
            return services;
        }

        private static string BuscarConnectionString(IConfiguration configuration)
        {
            var con = configuration.GetSection("DatabaseSettings:ConnectionStrings").Value;
            return con;
        }

        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, Action<IConnStringConfig<T>> options) where T : class
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            ConnStringConfig<T> config = new();
            options.Invoke(config);
            if (config.ConnString is null || string.IsNullOrWhiteSpace(config.ConnString))
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddSingleton<IConnStringConfig<T>>(config);
            services.AddSingleton<T>();

            return services;
        }
    }
}