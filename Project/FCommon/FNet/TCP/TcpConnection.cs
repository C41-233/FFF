using FFF.Base.Util.Atomic;
using FNet.Network;
using FNet.TCP.Buffer;

namespace FNet.TCP
{

    internal class TcpConnection : IConnection
    {

        public ulong ConnectionId { get; } = ConnectionIdProvidor.NextValue();

        public TcpServer Server { get; }
        public TcpSocket Socket { get; }

        public ReceiveBuffer ReceiveBuffer { get; }
        public SendBuffer SendBuffer { get; }

        public TcpConnectionConfig Config { get; }

        public bool IsShutdown => isShutdown;
        private readonly AtomicBool isClosed = new AtomicBool(false);
        private readonly AtomicBool isShutdown = new AtomicBool(false);

        public TcpConnection(TcpSocket socket, TcpConnectionConfig config)
        {
            this.Socket = socket;
            this.Server = config.Server;
            this.Config = config;

            this.ReceiveBuffer = new ReceiveBuffer(this);
            if (config.SendImmediately)
            {
                this.SendBuffer = new ImmediateSendBuffer(this);
                this.Socket.SetNoDelay(true);
            }
            else
            {
                this.SendBuffer = new FlushSendBuffer(this);
                this.Socket.SetNoDelay(true);
            }

        }

        public void Begin(long timestamp)
        {
            ReceiveBuffer.Begin(timestamp);
            SendBuffer.Begin(timestamp);
        }

        public void Send(byte[] bs)
        {
            SendBuffer.Send(bs);
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
