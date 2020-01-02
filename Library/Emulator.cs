using Microsoft.Extensions.Options;

using System;
using System.Net.Sockets;

using static IAS04110.Parser;

namespace IAS04110
{
    public class Emulator
    {
        #region Public Properties

        // ReSharper disable once InconsistentNaming
        public string IP { get; set; }
        public int Port { get; set; }
        public bool IsConnected =>
            _client.Connected;

        public bool Accepted
        {
            get
            {
                lock (_isSubscribedLock)
                {
                    return IsConnected && _accepted;
                }
            }
            private set
            {
                lock (_isSubscribedLock)
                {

                    _accepted = value;
                }
            }
        }

        public bool Listening
        {
            get
            {
                lock (_listeningLock)
                {
                    return Accepted && _listening;
                }
            }
            private set
            {
                lock (_listeningLock)
                {

                    _listening = value;
                }
            }
        }

        #endregion

        #region Private Fields

        private TcpClient _client;
        private NetworkStream _serverStream;
        private readonly ILogger _logger;
        private bool _accepted;
        private readonly object _isSubscribedLock = new object();
        private bool _listening;
        private readonly object _listeningLock = new object();
        #endregion

        public Emulator(
            IOptionsMonitor<EmulatorSettings> emuMonitor, ILogger logger)
        {
            _logger = logger;
            IP = emuMonitor.CurrentValue.IP;
            Port = emuMonitor.CurrentValue.Port;

            _client = new TcpClient();
            _serverStream = default;
        }

        public bool Connect()
        {
            _client.Close();
            _client = new TcpClient();
            _client.Connect(IP, Port);
            _serverStream = _client.GetStream();
            Subscribe();
            return _client.Connected;
        }

        public bool Disconnect()
        {
            _serverStream.Close();
            _client.Close();
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

        public byte[] Read()
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

        public void Start()
        {
            Write("Start");
            Accepted = true;
            Listening = true;
            _logger.Log("Started.");
        }

        public void Stop()
        {
            Write("Stop");
            if (IsConnected) Disconnect();
            _logger.Log("Stopped.");
        }

        public void Break()
        {
            Write("Break");
            Listening = false;
            _logger.Log("Broken off.");
        }

        public void Ready()
        {
            Write("Ready");
            _logger.Log("Ready!");
        }

        public void Exit()
        {
            if (IsConnected) 
                Disconnect();
            _logger.Log("Disconnected emulator.");
        }
    }
}
