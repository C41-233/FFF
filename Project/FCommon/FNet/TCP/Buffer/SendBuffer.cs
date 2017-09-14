using System.Net.Sockets;
using FNet.TCP.Protocol;

namespace FNet.TCP.Buffer
{

    internal abstract class SendBuffer
    {

        protected TcpConnection Connection { get; }
        protected Socket Socket => Connection.Socket;

        protected SendBuffer(TcpConnection conn, TcpConnectionConfig config)
        {
            this.Connection = conn;
        }

        public abstract void ReplyPing();

    }

    internal class FlushSendBuffer : SendBuffer
    {

        public FlushSendBuffer(TcpConnection conn, TcpConnectionConfig config) : base(conn, config)
        {
        }

        public override void ReplyPing()
        {
            Socket.Send(new byte[]{(byte) PackageType.Pong});
        }

    }

}
