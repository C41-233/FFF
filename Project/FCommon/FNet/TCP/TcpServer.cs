using FFF.Base.Util;
using FNet.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FFF.Base.Time;

namespace FNet.TCP
{
    public class TcpServer : IServer
    {

        public event FAction<IConnection> OnClientConnected;
        public event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;
        public event FAction<IConnection, byte[]> OnClientReceive;

        private readonly Socket sysSocket;
        private readonly TcpConnectionConfig connectionConfig;

        private readonly ConcurrentQueue<TcpConnection> acceptQueue = new ConcurrentQueue<TcpConnection>();
        private readonly ConcurrentQueue<Tuple<TcpConnection, ConnectionCloseType>> closeQueue = new ConcurrentQueue<Tuple<TcpConnection, ConnectionCloseType>>();
        private readonly ConcurrentQueue<Tuple<TcpConnection, byte[]>> receiveQueue = new ConcurrentQueue<Tuple<TcpConnection, byte[]>>();
        private readonly Dictionary<ulong, TcpConnection> connections = new Dictionary<ulong, TcpConnection>();

        private readonly AcceptController acceptController;
        private bool IsRunning = false;

        public TcpServer(TcpServerConfig config)
        {
            this.sysSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

            var endPoint = new IPEndPoint(config.Ip, config.Port);
            this.sysSocket.Bind(endPoint);

            this.sysSocket.Listen(config.Backlog);

            this.connectionConfig = new TcpConnectionConfig()
            {
                Server = this,

                KeepAlive = config.KeepAlive,
                PackageMaxSize = config.PackageMaxSize,
                ReadCacheSize = config.ReadCacheSize,
                SendImmediately = config.SendImmediately,
            };

            this.acceptController = new AcceptController(config.MaxConnection);
        }

        public void BeginAccept()
        {
            acceptController.BeginAccept();

            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            sysSocket.BeginAccept(AcceptCallback, null);
        }

        public void EndAccept()
        {
            acceptController.EndAccept();
        }

        public void Close()
        {
            sysSocket.Close();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var socket = sysSocket.EndAccept(ar);
            if (acceptController.CanAccept)
            {
                acceptController.OnConnect();
                var connection = new TcpConnection(new TcpSocket(socket), connectionConfig);
                acceptQueue.Enqueue(connection);
            }
            else
            {
                //防止多余的连接进入TIME_WAIT状态，占用服务器资源
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(true, 0));
                socket.Close();
            }

            sysSocket.BeginAccept(AcceptCallback, null);
        }

        internal void OnConnectionDisconnected(TcpConnection conn, ConnectionCloseType type)
        {
            closeQueue.Enqueue(Tuple.Create(conn, type));
            acceptController.OnDisconnect();
        }

        internal void OnConnectionReceive(TcpConnection conn, byte[] data)
        {
            receiveQueue.Enqueue(Tuple.Create(conn, data));
        }

        public void Update()
        {
            long nowTime = FDateTime.Now.TimeStamp;
            //disconnect
            while (closeQueue.TryDequeue(out Tuple<TcpConnection, ConnectionCloseType> close))
            {
                connections.Remove(close.Item1.ConnectionId);
                OnClientDisconnected?.Invoke(close.Item1, close.Item2);
            }
            //connect
            while (acceptQueue.TryDequeue(out TcpConnection connection))
            {
                connections.Add(connection.ConnectionId, connection);
                connection.Begin(nowTime);
                OnClientConnected?.Invoke(connection);
            }
            //receive
            while (receiveQueue.TryDequeue(out Tuple<TcpConnection, byte[]> receive))
            {
                if (receive.Item1.IsShutdown)
                {
                    continue;
                }
                OnClientReceive?.Invoke(receive.Item1, receive.Item2);
            }

            //update
            foreach (var conn in connections.Values)
            {
                if (conn.IsShutdown)
                {
                    conn.Close(ConnectionCloseType.Application);
                }
                else
                {
                    conn.Update(nowTime);
                }
            }
        }

        private class AcceptController
        {

            private readonly int maxAccept;

            private bool isAccept = false;
            private int connectionCount = 0;

            public AcceptController(int maxAccept)
            {
                this.maxAccept = maxAccept;
            }

            public void BeginAccept()
            {
                isAccept = true;
            }

            public void EndAccept()
            {
                isAccept = false;
            }

            public void OnConnect()
            {
                Interlocked.Increment(ref connectionCount);
            }

            public void OnDisconnect()
            {
                Interlocked.Decrement(ref connectionCount);
            }

            public bool CanAccept => isAccept && connectionCount < maxAccept;

        }

    }

}
