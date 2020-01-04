using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
{
    public interface ILogger
    {
        event Log LogEvent;
        void SetFile(string name);
        bool IsFileSet { get; }
        void Initialize(ChannelReader<string> logReader);
        Task Listen();
    }
}
