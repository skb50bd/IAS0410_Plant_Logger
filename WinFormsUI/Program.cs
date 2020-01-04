using IAS0410;

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

            return services.AddSingleton(config)
                           .AddOptions()
                           .AddSingleton<MainForm>()
                           .Configure<EmulatorSettings>(
                                config.GetSection("EmulatorSettings"))
                           .AddSingleton<App>()
                           .AddSingleton<ILogger, LoggerBase>()
                           .AddSingleton<IInputReader, ButtonReader>()
                           .AddSingleton<CommandSender>()
                           .AddSingleton<Emulator>();
        }
    }
}
