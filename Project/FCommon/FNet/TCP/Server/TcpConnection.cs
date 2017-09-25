using System.Net;
using FFF.Base.Util.Atomic;
using FFF.Network.Base;
using FFF.Network.TCP.Server.Buffer;

namespace FFF.Network.TCP.Server
{

    internal sealed class TcpConnection : IConnection
    {

        public ulong ConnectionId { get; } = ConnectionIdProvidor.NextValue();

        public TcpServer Server { get; }
        public TcpSocket Socket { get; }

        public ReceiveBuffer ReceiveBuffer { get; }
        public SendBuffer SendBuffer { get; }

        internal TcpConnectionConfig Config => Server.ConnectionConfig;

        public bool IsShutdown => isShutdown;
        private readonly InterlockedBool isClosed = new InterlockedBool(false);
        private readonly InterlockedBool isShutdown = new InterlockedBool(false);

        public IPAddress IP => Socket.IP;
        public int Port => Socket.Port;

        public TcpConnection(TcpServer server, TcpSocket socket)
        {
            this.Socket = socket;
            this.Server = server;

            this.ReceiveBuffer = new ReceiveBuffer(this);
            if (Config.SendImmediately)
            {
                this.SendBuffer = new ImmediateBlockSendBuffer(this);
            }
            else
            {
                this.SendBuffer = new UpdateBlockSendBuffer(this);
            }

            this.Socket.SetNoDelay(Config.NoDelay);
        }

        public void Begin(long timestamp)
        {
            ReceiveBuffer.Begin(timestamp);
            SendBuffer.Begin(timestamp);
        }

        public void Send(byte[] bs, int offset, int len)
        {
            SendBuffer.Send(bs, offset, len);
        }

        void IConnection.Close()
        {
            Shutdown();
        }

        private void Shutdown()
        {
            if (isShutdown.Exchange(true))
            {
                return;
            }
            ReceiveBuffer.Shutdown();
            SendBuffer.Shutdown();
        }

        public void Close(ConnectionCloseType type)
        {
            if (isClosed.Exchange(true))
            {
                return;
            }
            if (IsShutdown == false)
            {
                Shutdown();
            }
            Socket.Close();
            Server.OnConnectionDisconnected(this, type);
        }

        public void Update(long timestamp)
        {
            if (IsShutdown)
            {
                return;
            }
            ReceiveBuffer.Update(timestamp);
            SendBuffer.Update(timestamp);
        }
    }

}
