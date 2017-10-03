using FFF.Base.Util;
using FFF.Network.Base;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace FFF.Network.TCP.Client
{
    internal class TcpClient : IClient
    {

        public TcpClient(TcpClientConfig config)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (config.IP != null)
            {
                socket.Connect(config.IP, config.Port);
            }
            else if (config.Host != null)
            {
                socket.Connect(config.Host, config.Port);
            }
            else
            {
                throw new ArgumentException("TcpClient need one of IP and Host to connect.");
            }
        }

        public event FAction<byte[]> OnReceive;

        private readonly ConcurrentQueue<byte[]> receiveQueue = new ConcurrentQueue<byte[]>();

        public void BeginReceive()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] bs, int offset, int len)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            while (receiveQueue.TryDequeue(out byte[] data))
            {
                OnReceive?.Invoke(data);
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
