using FFF.Base.Util;
using FFF.Network.Base;
using System;

namespace FFF.Network.TCP.Client
{
    internal class TcpClient : IClient
    {

        public TcpClient(TcpClientConfig config)
        {
        }

        public event FAction<byte[]> OnReceive;
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
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
