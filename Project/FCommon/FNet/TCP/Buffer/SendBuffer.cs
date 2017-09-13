using System.Net.Sockets;
using FNet.TCP.Protocol;

namespace FNet.TCP.Buffer
{

    internal abstract class SendBuffer
    {

        protected FTcpConnection Connection { get; }
        protected Socket Socket => Connection.Socket;

        protected SendBuffer(FTcpConnection conn, FTcpConnectionConfig config)
        {
            this.Connection = conn;
        }

        public abstract void ReplyPing();

    }

    internal class FlushSendBuffer : SendBuffer
    {

        public FlushSendBuffer(FTcpConnection conn, FTcpConnectionConfig config) : base(conn, config)
        {
        }

        public override void ReplyPing()
        {
            Socket.Send(new byte[]{(byte) PackageType.Pong});
        }

    }

}
