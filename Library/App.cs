using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IAS04110
{
    public class App
    {
        private readonly Emulator _emulator;
        private readonly IInputReader _input;
        private readonly CommandSender _commandSender;
        private readonly ILogger _logger;

        private readonly Channel<string> _commandChannel;
        private readonly Channel<string> _logChannel;
        private readonly Channel<string> _inputChannel;

        public App(
            IOptionsMonitor<EmulatorSettings> monitor, 
            IConfiguration config)
        {
            _commandChannel = Channel.CreateUnbounded<string>();
            _logChannel = Channel.CreateUnbounded<string>();
            _inputChannel = Channel.CreateUnbounded<string>();
        
            _emulator = new Emulator(
                monitor.CurrentValue, 
                _commandChannel.Reader, 
                _logChannel.Writer);

            _input = new KeyboardReader(_inputChannel.Writer);

            _logger = new ConsoleLogger(config, _logChannel.Reader);

            _commandSender = new CommandSender(
                _inputChannel.Reader,
                _commandChannel.Writer, 
                _logChannel.Writer);   
        }

        public async Task Run()
        {
            Console.WriteLine("Starting Application");

            // Start Emulator to Listen incoming Commands;
            var emulatorListen = _emulator.ListenCommand(); 
        
            // Start Command Sender to Listen Input Commands
            var commandListen = _commandSender.Start();

            // Start Logger to Listen Logs 
            var logListen = _logger.Listen();

            // Listen to Inputs
            var inputListen = _input.Read();

            // When Exit Entered, Close Everything
            await inputListen.ContinueWith(_ => _inputChannel.Writer.Complete());
            await emulatorListen;
            await commandListen;
            await logListen;

            Console.WriteLine("Closing Application...");
        }
    }
}
