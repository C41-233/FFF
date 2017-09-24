using System.Net;

namespace FFF.Network.Base
{

    public interface IConnection
    {

        ulong ConnectionId { get; }

        IPAddress IP { get; }

        int Port { get; }

        void Send(byte[] bs, int offset, int length);

        void Close();

    }

    public static class ConnectionExtension
    {

        public static void Send(this IConnection conn, byte[] bs)
        {
            conn.Send(bs, 0, bs.Length);
        }

    }
}
