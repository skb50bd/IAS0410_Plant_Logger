using System.Threading.Channels;
using System.Threading.Tasks;

namespace IAS0410
{
    public interface IInputReader
    {
        Task Read();
        void Initialize(ChannelWriter<string> inputWriter);
    }
}