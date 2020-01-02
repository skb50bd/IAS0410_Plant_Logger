using System.Threading.Tasks;
using System;
using System.Threading.Channels;

namespace IAS04110
{
    public class KeyboardReader : IInputReader
    {
        private readonly ChannelWriter<string> _inputWriter;
        public KeyboardReader(ChannelWriter<string> inputWriter) {
            _inputWriter = inputWriter;
        }

        public async Task Read()
        {
            while (true)
            {
                var line = Console.ReadLine();
                await _inputWriter.WriteAsync(line);

                if (line.ToLowerInvariant() == "exit")
                    return;}
        }
    }
}
