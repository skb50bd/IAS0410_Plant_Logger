using System.Threading.Tasks;
using System;
using System.Threading.Channels;

namespace IAS0410
{
    public class KeyboardReader : IInputReader
    {
        private ChannelWriter<string> _inputWriter;
        public void Initialize(ChannelWriter<string> inputWriter) {
            _inputWriter = inputWriter;
        }

        public async Task Read()
        {
            while (true)
            {
                var line = Console.ReadLine();
                await _inputWriter.WriteAsync(line);

                if (line == "exit") break;
            }
            _inputWriter.Complete();
        }
    }
}
