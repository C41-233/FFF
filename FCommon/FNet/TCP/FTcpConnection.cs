using FNet.Network;
using FNet.TCP.Protocol;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;

namespace FNet.TCP
{
    internal class FTcpConnection : IConnection
    {

        private readonly Socket sysSocket;

        private readonly long keepAlive;

        private readonly FTcpServer server;

        public FTcpConnection(Socket socket, FTcpConnectionConfig config)
        {
            this.sysSocket = socket;
            this.keepAlive = config.KeepAlive;
            this.server = config.Server;

            this.writer = new BinaryWriter(writeBuffer);
        }

        private readonly ConcurrentQueue<byte[]> readQueue = new ConcurrentQueue<byte[]>();

        private byte[] buffer = new byte[1024];

        private int head = 0;
        private int tail = 0;

        private int expect = 0;

        public void BeginReceive()
        {
            expect = 1;
            DoReceive();
        }

        private void DoReceive()
        {
            sysSocket.BeginReceive(buffer, tail, buffer.Length - tail, SocketFlags.None, Receive, null);
        }

        private void Receive(IAsyncResult ar)
        {
            int nread;
            try
            {
                nread = sysSocket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                //todo 异常处理
                Close();
                return;
            }
            tail += nread;
            if (tail - head < expect)
            {
                DoReceive();
                return;
            }

            byte type = buffer[head];
            head += 1;
            switch (type)
            {
                case PackageType.Ping:
                {
                    Send(new FTcpPongPackage());
                    break;
                }
                default: break;
            }
            DoReceive();
        }

        private void Send(FTcpPongPackage pack)
        {
            writer.Write(pack.Type);
        }

        private readonly MemoryStream writeBuffer = new MemoryStream();
        private readonly BinaryWriter writer;

        public void Flush()
        {
            if (IsClosed)
            {
                return;
            }
            sysSocket.Send(writeBuffer.GetBuffer(), 0, (int) writeBuffer.Position, SocketFlags.None);
            writeBuffer.Seek(0, SeekOrigin.Begin);
        }

        public bool IsClosed { get; private set; }

        public void Close()
        {
            IsClosed = true;
            sysSocket.Close();
            server.OnConnectionDisconnected(this);
        }

    }

}
