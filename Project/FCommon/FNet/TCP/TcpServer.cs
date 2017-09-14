using System;
using System.Net;
using System.Net.Sockets;
using FFF.Base.Util;
using FNet.Network;

namespace FNet.TCP
{
    public class TcpServer : IServer
    {

        private readonly Socket sysSocket;

        public event FAction<IConnection> OnClientConnected;
        public event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;
        public event FAction<IConnection, byte[]> OnClientReceive;

        private readonly TcpConnectionConfig connectionConfig;

        public TcpServer(TcpServerConfig config)
        {
            this.sysSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

            var endPoint = new IPEndPoint(config.Ip, config.Port);
            this.sysSocket.Bind(endPoint);
            this.sysSocket.Listen(config.Backlog);

            this.connectionConfig = new TcpConnectionConfig()
            {
                KeepAlive = config.KeepAlive,
                PackageMaxSize = config.PackageMaxSize,
                ReadCacheSize = config.ReadCacheSize,
                Server = this,
            };
        }

        public void BeginAccept()
        {
            sysSocket.BeginAccept(Accept, null);
        }

        public void Close()
        {
            sysSocket.Close();
        }

        public void Update()
        {
            
        }

        private void Accept(IAsyncResult ar)
        {
            var socket = sysSocket.EndAccept(ar);

            var connection = new TcpConnection(socket, connectionConfig);
            OnClientConnected?.Invoke(connection);

            sysSocket.BeginAccept(Accept, null);
        }

        internal void OnConnectionDisconnected(TcpConnection conn, ConnectionCloseType type)
        {
            OnClientDisconnected?.Invoke(conn, type);
        }

    }
}
