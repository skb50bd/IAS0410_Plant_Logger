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

        public void Initialize(ChannelWriter<string> inputWriter)
        {
            _inputWriter = inputWriter;
        }

        public void SetCommand(string command)
        {
            _inputWriter.WriteAsync(command);
            if (command == "exit")
            {
                _inputWriter.Complete();
            }
        }

        public Task Read()
        {
            return Task.CompletedTask;
        }


    }
}