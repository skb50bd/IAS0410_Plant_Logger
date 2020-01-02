using System;

namespace IAS04110
{
    public class KeybaordReader : IInputReader
    {
        public event OnCommandReceived CommandReceived;

        public string Read()
        {
            var line = Console.ReadLine();
            CommandReceived?.Invoke(line);
            return line;
        }
    }
}
