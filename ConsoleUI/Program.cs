using IAS04110;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.IO;

namespace ConsoleUI
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var services =
                new ServiceCollection()
                   .Configure(args)
                   .BuildServiceProvider();

            var app = services.GetService<App>();
            app.Run();
        }

        static IServiceCollection Configure(
            this IServiceCollection services,
            string[] args)
        {
            var configBuilder =
                new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .AddCommandLine(args);
            IConfiguration config = configBuilder.Build();

            services.AddSingleton(config);
            services.AddOptions();
            services.Configure<EmulatorSettings>(
                config.GetSection("EmulatorSettings"));
            services.AddSingleton<App>();
            services.AddSingleton<UnitResolver>();
            services.AddSingleton<Parser>();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<IInputReader, KeybaordReader>();
            services.AddSingleton<CommandSender>();
            services.AddSingleton<Emulator>();
            return services;
        }
    }
}
