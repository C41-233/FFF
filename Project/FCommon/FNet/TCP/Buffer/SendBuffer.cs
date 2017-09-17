namespace FNet.TCP.Buffer
{

    internal abstract class SendBuffer
    {

        protected TcpConnection Connection { get; }
        protected TcpSocket Socket => Connection.Socket;

        protected SendBuffer(TcpConnection conn)
        {
            this.Connection = conn;
        }


        public virtual void Send(byte[] bs)
        {
            Send(bs, 0, bs.Length);
        }
        public abstract void Send(byte[] bs, int offset, int len);
        public abstract void ReplyPing();

        public virtual void Begin(long timestamp)
        {
        }

        public abstract void Shutdown();

        public virtual void Update(long timestamp)
        {
        }

    }
}
