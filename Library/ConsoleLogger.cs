﻿using System.Threading.Channels;
using Microsoft.Extensions.Configuration;

using static System.Console;
using static System.Text.Encoding;

namespace IAS04110
{

    public class ConsoleLogger : LoggerBase, ILogger
    {
        public ConsoleLogger(IConfiguration config, ChannelReader<string> logReader)
        : base(config, logReader)
        {
            OutputEncoding = Unicode;
        }

        protected override void Log(string message)
        {
            base.Log(message);
            Write(message);
        }
    }
}
