namespace FFF.Network.Base
{

    public interface IConnection
    {

        ulong ConnectionId { get; }

        void Send(byte[] bs);
        void Send(byte[] bs, int offset, int length);

        void Close();

    }
}
