using FNet.Network;
using FNet.TCP.Buffer;
using System.IO;
using System.Net.Sockets;

namespace FNet.TCP
{

    internal class TcpConnection : IConnection
    {

        private readonly TcpServer server;
        internal Socket Socket { get; }

        internal ReceiveBuffer Receiver { get; }
        internal SendBuffer Sender { get; }

        #region 属性
        private readonly long keepAlive;
        #endregion

        public TcpConnection(Socket socket, TcpConnectionConfig config)
        {
            this.Socket = socket;
            this.keepAlive = config.KeepAlive;
            this.server = config.Server;

            this.Receiver = new ReceiveBuffer(this, config);
            this.Sender = new FlushSendBuffer(this, config);
        }

        public void BeginReceive()
        {
            Receiver.BeginReceive();
        }

        private readonly MemoryStream writeBuffer = new MemoryStream();

        public void Flush()
        {
            if (IsClosed)
            {
                return;
            }
            Socket.Send(writeBuffer.GetBuffer(), 0, (int) writeBuffer.Position, SocketFlags.None);
            writeBuffer.Seek(0, SeekOrigin.Begin);
        }

        public bool IsClosed { get; private set; }

        public void Close()
        {
            Close(ConnectionCloseType.Application);
        }

        internal void Close(ConnectionCloseType type)
        {
            IsClosed = true;
            Socket.Close();
            server.OnConnectionDisconnected(this, type);
        }

    }

}
