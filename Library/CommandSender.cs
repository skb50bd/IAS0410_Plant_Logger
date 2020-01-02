using System;
using System.Diagnostics;

namespace IAS04110
{
    public class CommandSender
    {
        private readonly Emulator _emulator;
        private readonly ILogger _logger;

        private readonly object _commandLock = new object();
        private string _command;

        public delegate void ExitCommand();
        public event ExitCommand Exit;

        public CommandSender(
            Emulator emulator,
            ILogger logger)
        {
            _emulator = emulator;
            _logger = logger;
        }

        public void ReceiveCommand(
            string command)
        {
            lock (_commandLock)
            {
                _command = command;
            }
        }

        private void ClearCommand()
        {
            lock (_commandLock)
            {
                _command = string.Empty;
            }
        }

        public void ExecuteCommand()
        {
            try
            {
                lock (_commandLock)
                {
                    if (string.IsNullOrEmpty(_command)) return;

                    switch (_command)
                    {
                        case "connect" when !_emulator.Accepted:
                            _logger.Log("Connecting emulator...");
                            _emulator.Connect();
                            _logger.Log("Connected!");
                            break;

                        case "start" when _emulator.Accepted && !_emulator.Listening:
                            _logger.Log("Sending \"Start\" command");
                            _emulator.Start();
                            break;

                        case "stop" when _emulator.Accepted:
                            _logger.Log("Sending \"Stop\" command");
                            _emulator.Stop();
                            break;

                        case "break" when _emulator.Listening:
                            _logger.Log("Sending \"Break\" command");
                            _emulator.Break();
                            break;

                        case "exit":
                            _logger.Log("Exiting the application");
                            _emulator.Exit();
                            Exit?.Invoke();
                            break;
                        default:
                            _logger.Log($"Ignoring invalid command: \"{_command}\"");

                            break;

                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(
                    $"Error executing the \"{_command}\" command.\n" +
                    e.Message);
            }
            finally
            {
                ClearCommand();
            }
        }
    }
}
