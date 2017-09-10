using System;
using System.Net;
using System.Net.Sockets;
using FFF.Base.Util;
using FNet.Network;

namespace FNet.TCP
{
    public class FTcpServer : IFServer
    {

        private readonly Socket sysSocket;

        public event FAction<IConnection> OnClientConnected;
        public event FAction<IConnection> OnClientDisconnected;

        private readonly FTcpConnectionConfig connectionConfig;

        public FTcpServer(FTcpServerConfig config)
        {
            this.sysSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

            var endPoint = new IPEndPoint(config.Ip, config.Port);
            this.sysSocket.Bind(endPoint);
            this.sysSocket.Listen(config.Backlog);

            this.connectionConfig = new FTcpConnectionConfig()
            {
                KeepAlive = config.KeepAlive,
                PackageMaxSize = config.PackageMaxSize,
                Server = this,
            };
        }

        public void BeginAccept()
        {
            sysSocket.BeginAccept(Accept, null);
        }

        private void Accept(IAsyncResult ar)
        {
            var socket = sysSocket.EndAccept(ar);

            var connection = new FTcpConnection(socket, connectionConfig);
            OnClientConnected?.Invoke(connection);

            sysSocket.BeginAccept(Accept, null);
        }

        internal void OnConnectionDisconnected(FTcpConnection conn)
        {
            OnClientDisconnected?.Invoke(conn);
        }

    }
}
