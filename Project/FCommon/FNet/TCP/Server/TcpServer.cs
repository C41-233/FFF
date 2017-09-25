using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FFF.Base.Time;
using FFF.Base.Util;
using FFF.Network.Base;

namespace FFF.Network.TCP.Server
{
    internal sealed class TcpServer : IServer
    {

        private enum ServerMode
        {
            Init = 0,
            Running = 1,
            WaitClose = 2,
            Closed = 3,
        }

        public event FAction<IConnection> OnClientConnected;
        public event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;
        public event FAction<IConnection, byte[]> OnClientReceive;
        public event FAction<IServer> OnServerClosed;

        private readonly Socket sysSocket;
        internal TcpConnectionConfig ConnectionConfig { get; }

        private readonly ConcurrentQueue<TcpConnection> acceptQueue = new ConcurrentQueue<TcpConnection>();
        private readonly ConcurrentQueue<Tuple<TcpConnection, ConnectionCloseType>> closeQueue = new ConcurrentQueue<Tuple<TcpConnection, ConnectionCloseType>>();
        private readonly ConcurrentQueue<Tuple<TcpConnection, byte[]>> receiveQueue = new ConcurrentQueue<Tuple<TcpConnection, byte[]>>();
        private readonly Dictionary<ulong, TcpConnection> connections = new Dictionary<ulong, TcpConnection>();

        private readonly AcceptController acceptController;

        private ServerMode mode = ServerMode.Init;

        public TcpServer(TcpServerConfig config)
        {
            this.sysSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

            var endPoint = new IPEndPoint(config.IP, config.Port);
            this.sysSocket.Bind(endPoint);

            this.sysSocket.Listen(config.Backlog);

            this.ConnectionConfig = new TcpConnectionConfig()
            {
                Server = this,

                KeepAlive = config.KeepAlive,
                NoDelay = config.NoDelay,
                PackageMaxSize = config.PackageMaxSize,
                ReadCacheSize = config.ReadCacheSize,
                SendImmediately = config.SendImmediately,
            };

            this.acceptController = new AcceptController(config.MaxConnection);
        }

        public void BeginAccept()
        {
            if (mode >= ServerMode.WaitClose)
            {
                throw new ObjectDisposedException("Server is closed.");
            }

            acceptController.BeginAccept();

            if (mode == ServerMode.Running)
            {
                return;
            }

            mode = ServerMode.Running;
            sysSocket.BeginAccept(AcceptCallback, null);
        }

        public void EndAccept()
        {
            acceptController.EndAccept();
        }

        public void Close()
        {
            if (mode >= ServerMode.WaitClose)
            {
                return;
            }

            mode = ServerMode.WaitClose;
            sysSocket.Close();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            if (mode >= ServerMode.WaitClose)
            {
                return;
            }

            var socket = sysSocket.EndAccept(ar);

            if (acceptController.CanAccept)
            {
                acceptController.OnConnect();
                var connection = new TcpConnection(this, new TcpSocket(socket));
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

        #region 内部调用
        internal void OnConnectionDisconnected(TcpConnection conn, ConnectionCloseType type)
        {
            closeQueue.Enqueue(Tuple.Create(conn, type));
            acceptController.OnDisconnect();
        }

        internal void OnConnectionReceive(TcpConnection conn, byte[] data)
        {
            receiveQueue.Enqueue(Tuple.Create(conn, data));
        }
        #endregion

        public void Update()
        {
            if (mode == ServerMode.Closed)
            {
                return;
            }

            long nowTime = FDateTime.Now.TimeStamp;

            var waitCloseWatchDog = 0; //仅当服务器的所有事件处理都完成后，才可以触发OnServerClosed

            //disconnect
            while (closeQueue.TryDequeue(out Tuple<TcpConnection, ConnectionCloseType> close))
            {
                waitCloseWatchDog++;
                connections.Remove(close.Item1.ConnectionId);
                OnClientDisconnected?.Invoke(close.Item1, close.Item2);
            }
            //connect
            while (acceptQueue.TryDequeue(out TcpConnection connection))
            {
                waitCloseWatchDog++;
                connections.Add(connection.ConnectionId, connection);
                connection.Begin(nowTime);
                OnClientConnected?.Invoke(connection);

                //服务器发起关闭后还未处理的连接，经过OnClientConnected后再OnClientDisconnected
                if (mode == ServerMode.WaitClose)
                {
                    connection.Close(ConnectionCloseType.ServerClose);
                }
            }
            //receive
            while (receiveQueue.TryDequeue(out Tuple<TcpConnection, byte[]> receive))
            {
                waitCloseWatchDog++;
                //服务器发起关闭后，不再接收数据
                if (mode == ServerMode.WaitClose)
                {
                    continue;
                }
                //连接断开后，不再接收数据
                if (receive.Item1.IsShutdown)
                {
                    continue;
                }
                OnClientReceive?.Invoke(receive.Item1, receive.Item2);
            }

            //update
            foreach (var conn in connections.Values)
            {
                waitCloseWatchDog++;
                //服务器发起关闭后，首先关闭所有连接
                if (mode == ServerMode.WaitClose)
                {
                    conn.Close(ConnectionCloseType.ServerClose);
                    continue;
                }
                //应用层主动关闭连接
                if (conn.IsShutdown)
                {
                    conn.Close(ConnectionCloseType.ApplicationClose);
                    continue;
                }
                conn.Update(nowTime);
            }

            //当所有连接都妥善处理并关闭后，触发OnServerClosed
            if (mode == ServerMode.WaitClose && waitCloseWatchDog == 0)
            {
                mode = ServerMode.Closed;
                OnServerClosed?.Invoke(this);
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


            public bool CanAccept
            {
                get
                {
                    if (isAccept == false)
                    {
                        return false;
                    }
                    if (maxAccept > 0)
                    {
                        return connectionCount < maxAccept;
                    }
                    return true;
                }
            }

        }

    }

}
