using IAS04110;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormsUI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var services =
                new ServiceCollection()
                   .Configure(args)
                   .BuildServiceProvider();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(services.GetService<MainForm>());
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
            services.AddSingleton<MainForm>();
            services.Configure<EmulatorSettings>(
                config.GetSection("EmulatorSettings"));
            services.AddSingleton<App>();
            services.AddSingleton<UnitResolver>();
            services.AddSingleton<Parser>();
            services.AddSingleton<ILogger, LoggerBase>();
            services.AddSingleton<IInputReader, ButtonReader>();
            services.AddSingleton<CommandSender>();
            services.AddSingleton<Emulator>();
            return services;
        }
    }
}
