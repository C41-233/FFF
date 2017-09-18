using System;
using System.Net.Sockets;

namespace FFF.Network.TCP
{
    public class TcpSocket
    {

        private readonly Socket socket;

        public TcpSocket(Socket socket)
        {
            this.socket = socket;
        }

        public void SetNoDelay(bool b)
        {
            this.socket.NoDelay = b;
        }

        public void Shutdown(SocketShutdown how)
        {
            try
            {
                this.socket.Shutdown(how);
            }
            catch (SocketException)
            { }
            catch (ObjectDisposedException)
            { }
        }

        public void Close()
        {
            this.socket.Close();
        }

        public void Send(byte[] bs, int offset, int len)
        {
            this.socket.Send(bs, offset, len, SocketFlags.None);
        }

        public void BeginReceive(byte[] bs, int offset, int len, AsyncCallback callback)
        {
            this.socket.BeginReceive(bs, offset, len, SocketFlags.None, callback, this);
        }

        public int EndReceive(IAsyncResult ar)
        {
            return this.socket.EndReceive(ar);
        }

    }
}
