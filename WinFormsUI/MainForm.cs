using IAS04110;

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using Timer = System.Timers.Timer;

namespace WinFormsUI
{
    public partial class MainForm : Form
    {
        private readonly ILogger _logger;
        private readonly ButtonReader _input;
        private readonly App _app;
        private readonly Emulator _emu;
        private readonly object _lock = new object();
        private readonly Timer _timer;
        public MainForm(
            ILogger logger,
            IInputReader input,
            App app,
            Emulator emu)
        {
            _logger = logger;
            _logger.LogEvent += WriteToLog;

            _input = input as ButtonReader;
            _app = app;
            _emu = emu;
            InitializeComponent();

            _logger.SetFile(null);
            _app.Run();
            _timer = new Timer(400);
            _timer.Elapsed += (source, e) => RefreshButtons();
            _timer.Start();
        }

        private void RefreshButtons()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                openLogFileBtn.Enabled = !_logger.IsFileSet;
                closeLogFileBtn.Enabled = _logger.IsFileSet;

                connectBtn.Enabled = !_emu.IsConnected;
                disconnectBtn.Enabled = _emu.IsConnected;

                startBtn.Enabled = _emu.IsConnected && !_emu.Listening;
                breakBtn.Enabled = _emu.Listening;
            }));
        }

        public void WriteToLog(string msg)
        {
            Debug.WriteLine(msg);
            Invoke(new MethodInvoker(delegate ()
            {
                logText.AppendText($"{msg}\n");
                logText.SelectionStart = logText.TextLength;
                logText.ScrollToCaret();
            }));
        }

        private void OpenLogFileBtn_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _logger.SetFile(dialog.FileName);
                _logger.Log("Log file opened.");
            }
        }

        private void CloseLogFileBtn_Click(object sender, EventArgs e)
        {
            _logger.SetFile(null);
            _logger.Log("Closed log file.");
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            _input.SetCommand("connect");
            Thread.Sleep(400);
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            _input.SetCommand("stop");
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            _input.SetCommand("start");
        }

        private void BreakBtn_Click(object sender, EventArgs e)
        {
            _input.SetCommand("break");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _timer.Stop();
            _timer.Dispose();
            _app.Exit();
        }
    }
}
