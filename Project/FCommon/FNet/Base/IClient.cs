using FFF.Base.Util;

namespace FFF.Network.Base
{
    public interface IClient
    {

        event FAction<byte[]> OnReceive;

        void BeginReceive();

        void Send(byte[] bs, int offset, int len);

        void Update();

        void Close();

    }
}
