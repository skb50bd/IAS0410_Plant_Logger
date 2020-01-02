using Microsoft.Extensions.Configuration;

using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS04110
{
    public class LoggerBase : ILogger
    {
        private readonly ChannelReader<string> _logReader;
        public LoggerBase(IConfiguration config, ChannelReader<string> logReader)
        {
            _fileName = config.GetValue<string>("FileName");
            _logReader = logReader;
        }

        public event Log LogEvent;

        private string _fileName;

        public void SetFile(string name) => _fileName = name;
        public bool IsFileSet => !(_fileName is null);

        protected virtual void Log(string message)
        {
            if (IsFileSet)
                File.AppendAllText(_fileName, message);

            LogEvent?.Invoke(message);
        }

        public async Task Listen() {
            while(await _logReader.WaitToReadAsync()) {
                if (_logReader.TryRead(out var message))
                {
                    Log(message + "\n");
                }
            }
        }
    }
}
