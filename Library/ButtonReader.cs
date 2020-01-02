namespace IAS04110
{
    public class ButtonReader : IInputReader
    {
        public event OnCommandReceived CommandReceived;
        public string Command { get; set; }
        private readonly object _commandLock = new object();

        public string SetCommand(string value)
        {
            lock (_commandLock)
            {

                Command = value;

                return Read();
            }
        }

        public string Read()
        {
            lock (_commandLock)
            {
                if (Command is null) return Command;

                CommandReceived?.Invoke(Command);
                Command = null;

                return Command;
            }
        }
    }
}