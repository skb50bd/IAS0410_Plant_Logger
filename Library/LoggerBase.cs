using Microsoft.Extensions.Configuration;

using System.IO;

namespace IAS04110
{
    public class LoggerBase : ILogger
    {
        public LoggerBase(IConfiguration config)
        {
            _fileName = config.GetValue<string>("FileName");
        }

        public event Log LogEvent;

        private string _fileName;

        public void SetFile(string name) => _fileName = name;
        public bool IsFileSet => !(_fileName is null);

        public virtual void Log(string message)
        {
            if (IsFileSet)
                File.AppendAllText(_fileName, message);

            LogEvent?.Invoke(message);
        }
    }
}
