namespace IAS04110
{
    public interface IInputReader
    {
        event OnCommandReceived CommandReceived;
        string Read();
    }
}