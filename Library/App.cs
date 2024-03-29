﻿using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
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
            Emulator emulator,
            ILogger logger,
            CommandSender commandSender,
            IInputReader input
        )
        {
            _commandChannel = Channel.CreateUnbounded<string>();
            _logChannel = Channel.CreateUnbounded<string>();
            _inputChannel = Channel.CreateUnbounded<string>();

            _emulator = emulator;
            _logger = logger;
            _input = input;
            _commandSender = commandSender;

            _emulator.Initialize(_commandChannel.Reader, _logChannel.Writer);
            _input.Initialize(_inputChannel.Writer);
            _logger.Initialize(_logChannel.Reader);
            _commandSender.Initialize(
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

            await emulatorListen;
            await commandListen;
            await logListen;
            await inputListen;

            Console.WriteLine("Closing Application...");
        }
    }
}
