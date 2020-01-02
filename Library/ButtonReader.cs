using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS04110
{
    public class ButtonReader : IInputReader
    {
        private readonly ChannelWriter<string> _inputWriter;
        public ButtonReader(ChannelWriter<string> inputWriter) {
            _inputWriter = inputWriter;
        }

        public async Task Read()
        {
            await _inputWriter.WriteAsync("");
            throw new NotImplementedException();
        }
    }
}