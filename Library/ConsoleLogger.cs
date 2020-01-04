using System.Threading.Channels;
using Microsoft.Extensions.Configuration;

using static System.Console;
using static System.Text.Encoding;

namespace IAS0410
{

    public class ConsoleLogger : LoggerBase, ILogger
    {
        public ConsoleLogger(IConfiguration config): base(config) {
            OutputEncoding = Unicode;
        }

        protected override void Log(string message)
        {
            base.Log(message);
            Write(message);
        }
    }
}
