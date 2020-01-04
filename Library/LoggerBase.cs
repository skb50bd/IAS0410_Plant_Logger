using Microsoft.Extensions.Configuration;

using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
{
    public class LoggerBase : ILogger
    {
        private ChannelReader<string> _logReader;

        public LoggerBase(IConfiguration config) {
            _fileName = config.GetValue<string>("FileName");
        }

        public virtual void Initialize(
            ChannelReader<string> logReader)
        {
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
