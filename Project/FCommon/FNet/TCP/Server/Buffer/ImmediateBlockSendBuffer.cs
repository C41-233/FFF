using System.Net.Sockets;

namespace FFF.Network.TCP.Server.Buffer
{
    internal sealed class ImmediateBlockSendBuffer : SendBuffer 
    {

        public ImmediateBlockSendBuffer(TcpConnection conn) : base(conn)
        {
        }

        public override void ReplyPing()
        {
            Socket.Send(Pong);
        }

        public override void Send(byte[] bs, int offset, int len)
        {
            Socket.Send(bs, offset, len);
        }

        public override void Shutdown()
        {
            Socket.Shutdown(SocketShutdown.Send);
        }
    }
}
