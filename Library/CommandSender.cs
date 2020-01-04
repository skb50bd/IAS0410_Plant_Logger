using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
{
    public class CommandSender
    {
        private ChannelReader<string> _inputReader;
        private ChannelWriter<string> _commandWriter;
        private ChannelWriter<string> _logWriter;
        public void Initialize (
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
            _commandWriter.Complete();
        }
    }
}
