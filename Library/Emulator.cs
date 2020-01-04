using System.Threading;
using System;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;
using static IAS0410.Parser;
using Microsoft.Extensions.Options;

namespace IAS0410
{
    public class Emulator
    {
        #region Public Properties

        public string IP { get; set; }
        public int Port { get; set; }
        public bool IsConnected =>
            _client.Connected;

        public bool Accepted
        {
            get => IsConnected && _accepted;
            private set => _accepted = value;
        }

        public bool Listening
        {
            get => Accepted && _listening;
            private set => _listening = value;
        }

        #endregion

        #region Private Fields

        private TcpClient _client;
        private NetworkStream _serverStream;

        private bool _accepted;
        private bool _listening;

        private ChannelReader<string> _commandReader;
        private ChannelWriter<string> _logWriter;
        private CancellationTokenSource _cts;
        private Task ListenDataTask;

        #endregion

        public Emulator(
            IOptionsMonitor<EmulatorSettings> settings)
        {
            IP = settings.CurrentValue.IP;
            Port = settings.CurrentValue.Port;

            _client = new TcpClient();

            _serverStream = default;
            _cts = new CancellationTokenSource();
        }

        public void Initialize(
            ChannelReader<string> commandReader,
            ChannelWriter<string> logWriter)
        {
            _commandReader = commandReader;
            _logWriter = logWriter;
        }

        public async Task ListenCommand()
        {
            await _logWriter.WriteAsync("Emulator Listener Started Listening to Incoming Commands...");

            while (await _commandReader.WaitToReadAsync())
            {
                if (_commandReader.TryRead(out var command))
                {
                    ProcessCommand(command);
                }
            }
            await _logWriter.WriteAsync("Emulator Listener is Shutting Down...");
        }

        private void ProcessCommand(string command)
        {
            switch (command)
            {
                case "connect" when !Accepted:
                    Connect();
                    break;

                case "start" when Accepted && !Listening:
                    Start();
                    break;

                case "stop" when Accepted:
                    Stop();
                    break;

                case "break" when Listening:
                    Break();
                    break;

                case "exit":
                    Exit();
                    break;

                default:
                    _logWriter.WriteAsync($"Ignoring invalid command: \"{command}\"");
                    break;
            }
        }

        private void ListenData(object obj)
        {
            var token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (Listening)
                    {
                        var bytes = Read();
                        var data = ParsePackage(bytes);

                        var text = $"Measurement results at {DateTime.Now:G}\n";
                        text = string.Concat(text, data.Serialize());
                        _logWriter.WriteAsync(text);
                        Ready();
                    }
                }
                catch (Exception e)
                {
                    _logWriter.WriteAsync("Exception was thrown when trying to read the emulator\n" + e.Message);
                }
                Thread.Sleep(200);
            }
        }

        private bool Connect()
        {
            _logWriter.WriteAsync("Connecting emulator...");
            try
            {
                _client.Close();
                _client = new TcpClient();
                _client.Connect(IP, Port);
                _serverStream = _client.GetStream();
                Subscribe();
            }
            catch (SocketException e)
            {
                _logWriter.WriteAsync("Error Connecting the Emulator\n" + e.Message);
            }
            if (_client.Connected)
                _logWriter.WriteAsync("Connected!");
            return _client.Connected;
        }

        private bool Disconnect()
        {
            _serverStream.Close();
            _client.Close();
            Listening = false;
            return !_client.Connected;
        }

        private void Subscribe()
        {
            var bytes = Read();
            var msg = FromUnicodeBytes(bytes);
            if (!msg.StartsWith("Identify"))
                return;

            Write("coursework");
            bytes = Read();
            msg = FromUnicodeBytes(bytes);
            Accepted = msg.StartsWith("Accepted");
        }

        private byte[] Read()
        {
            if (!IsConnected)
                throw new InvalidOperationException(
                    "Cannot Read from Non-connected Host.");

            if (!_serverStream.CanRead)
                throw new InvalidOperationException(
                    "Cannot Read from Server");

            var inStream = new byte[_client.ReceiveBufferSize];
            _serverStream.Read(inStream, 0, inStream.Length);
            return inStream;
        }

        private void Write(byte[] bytes)
        {
            if (!IsConnected)
                throw new InvalidOperationException(
                    "Cannot Write to Non-connected Host.");

            if (!_serverStream.CanWrite)
                throw new InvalidOperationException(
                    "Cannot Write to Server");

            _serverStream.Write(bytes, 0, bytes.Length);
        }

        private void Write(string msg)
        {
            var bytes = ToUnicodeBytes(msg);
            Write(bytes);
        }

        private void Start()
        {
            _logWriter.WriteAsync("Sending \"Start\" command");
            Write("Start");
            Accepted = true;
            Listening = true;
            _cts = new CancellationTokenSource();
            ListenDataTask = new Task(ListenData, _cts.Token);
            ListenDataTask.Start();
            _logWriter.WriteAsync("Started.");
        }

        private void Stop()
        {
            _logWriter.WriteAsync("Sending \"Stop\" command");
            _cts.Cancel();

            Write("Stop");

            if (IsConnected)
                Disconnect();

            _logWriter.WriteAsync("Stopped.");
        }

        private void Break()
        {
            _logWriter.WriteAsync("Sending \"Break\" command");
            _cts.Cancel();

            Write("Break");
            Listening = false;

            _logWriter.WriteAsync("Broken off.");
        }

        private void Ready()
        {
            Write("Ready");
            _logWriter.WriteAsync("Ready!");
        }

        private void Exit()
        {
            _logWriter.WriteAsync("Exiting the application");
            _cts.Cancel();

            if (IsConnected)
                Disconnect();

            _logWriter.WriteAsync("Disconnected emulator.");
        }
    }
}
