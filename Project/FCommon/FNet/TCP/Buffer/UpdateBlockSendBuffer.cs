using FFF.Network.Base;
using FFF.Network.TCP.Protocol;
using System;
using System.IO;
using System.Net.Sockets;

namespace FFF.Network.TCP.Buffer
{
    internal sealed class UpdateBlockSendBuffer : SendBuffer
    {

        public UpdateBlockSendBuffer(TcpConnection conn) : base(conn)
        {
        }

        private readonly MemoryStream sendBuffer = new MemoryStream();

        public override void ReplyPing()
        {
            lock(sendBuffer)
            {
                if (sendBuffer.Length > 0)
                {
                    return;
                }
                sendBuffer.WriteByte((byte) PackageType.Pong);
            }
        }

        public override void Send(byte[] bs, int offset, int len)
        {
            lock (sendBuffer)
            {
                sendBuffer.Write(bs, offset, len);
            }
        }

        public override void Shutdown()
        {
            try
            {
                Flush();
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            Socket.Shutdown(SocketShutdown.Send);
        }

        public override void Update(long timestamp)
        {
            try
            {
                Flush();
            }
            catch (SocketException e)
            {
                Connection.Close((ConnectionCloseType) e.ErrorCode);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void Flush()
        {
            lock (sendBuffer)
            {
                Socket.Send(sendBuffer.GetBuffer(), 0, (int) sendBuffer.Position);
                sendBuffer.Seek(0, SeekOrigin.Begin);
            }
        }

    }
}