using System.Collections.Concurrent;

namespace FFF.Network.TCP.Buffer
{
    internal sealed class ImmediateBlockSendBuffer : SendBuffer 
    {

        private readonly ConcurrentQueue<byte[]> sendBuffer = new ConcurrentQueue<byte[]>();

        public ImmediateBlockSendBuffer(TcpConnection conn) : base(conn)
        {
        }

        public override void ReplyPing()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(long timestamp)
        {
            throw new System.NotImplementedException();
        }

        public override void Send(byte[] bs, int offset, int len)
        {
            var data = new byte[len];
            System.Buffer.BlockCopy(bs, offset, data, 0, len);
            sendBuffer.Enqueue(data);
        }

        public override void Begin(long timestamp)
        {
            throw new System.NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new System.NotImplementedException();
        }
    }
}
