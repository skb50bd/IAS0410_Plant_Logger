using System;
using System.Threading;

namespace IAS04110
{
    public class App
    {
        private readonly Emulator _emulator;
        private readonly IInputReader _input;
        private readonly CommandSender _commandSender;
        private readonly Parser _parser;
        private readonly ILogger _logger;

        private readonly Thread _emulatorThread;
        private readonly Thread _inputThread;
        private readonly Thread _senderThread;
        private readonly CancellationTokenSource _cts;

        public App(
            Emulator emulator,
            IInputReader input,
            CommandSender commandSender,
            Parser parser,
            ILogger logger)
        {
            _emulator = emulator;
            _input = input;
            _commandSender = commandSender;
            _parser = parser;
            _logger = logger;

            _emulatorThread = new Thread(StartEmulator);
            _inputThread = new Thread(ListenInput);
            _senderThread = new Thread(SendCommand);

            _cts = new CancellationTokenSource();
            _commandSender.Exit += Exit;
        }


        public void StartEmulator(object obj)
        {
            var token = (CancellationToken)obj;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_emulator.Listening)
                    {
                        var bytes = _emulator.Read();
                        var data = _parser.ParsePackage(bytes);

                        var text = $"Measurement results at {DateTime.Now:s}\n";
                        text += data.Serialize();
                        _logger.Log(text);

                        _emulator.Ready();
                    }

                }
                catch
                {
                    _logger.Log("Exception was thrown when trying to read the emulator");
                }
                Thread.Sleep(200);
            }
        }

        public void ListenInput(object obj)
        {
            _input.CommandReceived += _commandSender.ReceiveCommand;

            var token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                _input.Read();
                Thread.Sleep(200);
            }
        }

        public void SendCommand(object obj)
        {
            var token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                _commandSender.ExecuteCommand();
                Thread.Sleep(200);
            }
        }

        public void Run()
        {
            _emulatorThread.Start(_cts.Token);
            _inputThread.Start(_cts.Token);
            _senderThread.Start(_cts.Token);
        }

        public void Exit()
        {
            try
            {
                _logger.Log("Exiting... Cancelling the threads.");
                _cts.Cancel();
            }
            catch
            {
                _logger.Log("Exception was thrown closing the threads.");
            }
        }
    }
}
