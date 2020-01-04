using System.Diagnostics;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
{
    public class ButtonReader : IInputReader
    {
        private ChannelWriter<string> _inputWriter;
        private string _line;
        private object _lineLock = new object();

        public void Initialize(ChannelWriter<string> inputWriter)
        {
            _inputWriter = inputWriter;
        }

        public void SetCommand(string command)
        {
            lock (_lineLock)
            {
                _line = command;
            }
        }

        public async Task Read()
        {
            lock (_lineLock)
            {
                while (true)
                {
                    if (!string.IsNullOrWhiteSpace(_line))
                    {
                        _inputWriter.WriteAsync(_line);
                        if (_line != "exit") break;
                        _line = "";
                    }

                    Thread.Sleep(300);
                }
            }
        }


    }
}