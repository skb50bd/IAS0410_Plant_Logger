using IAS0410;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.IO;
using System.Threading.Tasks;

namespace ConsoleUI
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var services =
                new ServiceCollection()
                   .Configure(args)
                   .BuildServiceProvider();

            var app = services.GetService<App>();
            await app.Run();
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
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<IInputReader, KeyboardReader>();
            services.AddSingleton<CommandSender>();
            services.AddSingleton<Emulator>();
            return services;
        }
    }
}
