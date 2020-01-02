using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS04110
{
    public class CommandSender
    {
        private readonly ChannelReader<string> _inputReader;
        private readonly ChannelWriter<string> _commandWriter;
        private readonly ChannelWriter<string> _logWriter;
        public CommandSender(
            ChannelReader<string> inputReader,
            ChannelWriter<string> commandWriter,
            ChannelWriter<string> logWriter)
        {
            _inputReader = inputReader;
            _commandWriter = commandWriter;
            _logWriter = logWriter;
        }

        public async Task Start() {
            await _logWriter.WriteAsync("Command Sender is Listening to Inputs...");
            while(await _inputReader.WaitToReadAsync()) {
                if (_inputReader.TryRead(out var command))
                {
                    if(!string.IsNullOrWhiteSpace(command)) 
                        await _commandWriter.WriteAsync(command);
                }
            }
        }
    }
}
