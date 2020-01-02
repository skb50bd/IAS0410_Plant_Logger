using Microsoft.Extensions.Configuration;

using static System.Console;
using static System.Text.Encoding;

namespace IAS04110
{

    public class ConsoleLogger : LoggerBase, ILogger
    {
        public ConsoleLogger(IConfiguration config)
        : base(config)
        {
            OutputEncoding = Unicode;
        }

        public override void Log(string message)
        {
            base.Log(message);
            WriteLine(message);
        }
    }
}
