using System;
using System.Net.Sockets;
using FFF.Base.Collection.Buffer;
using FFF.Base.Util.BitConvert;
using FFF.Network.Base;
using FFF.Network.TCP.Protocol;

namespace FFF.Network.TCP.Server.Buffer
{

    internal sealed class ReceiveBuffer
    {

        private enum ReceiveStatus
        {
            ReadType,
            ReadDataLength,
            ReadData,
        }

        private readonly TcpConnection Connection;
        private TcpSocket Socket => Connection.Socket;
        private TcpConnectionConfig Config => Connection.Config;

        private readonly MemoryBuffer readBuffer = new MemoryBuffer();
        private readonly byte[] readCache;

        private ReceiveStatus status;
        private int expectLength;

        private long lastReceiveTime;
        private long nowTime;

        public ReceiveBuffer(TcpConnection conn)
        {
            this.Connection = conn;
            this.readCache = new byte[Config.ReadCacheSize];
        }

        public void Begin(long timestamp)
        {
            if (Connection.IsShutdown)
            {
                throw new ObjectDisposedException("ReceiveBuffer already shutdown.");
            }
            lastReceiveTime = nowTime = timestamp;
            NextPackage();
        }

        private void DoReceive()
        {
            if (Connection.IsShutdown)
            {
                return;
            }
            try
            {
                Socket.BeginReceive(readCache, 0, readCache.Length, ReceiveCallback);
            }
            catch (SocketException e)
            {
                Connection.Close((ConnectionCloseType) e.ErrorCode);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (Connection.IsShutdown)
            {
                return;
            }
            int nread;
            try
            {
                nread = Socket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                Connection.Close((ConnectionCloseType) e.ErrorCode);
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            readBuffer.Push(readCache, 0, nread);

            ProcessReceive();
        }

        private void ProcessReceive()
        {
            if (readBuffer.Length < expectLength)
            {
                DoReceive();
                return;
            }

            switch (status)
            {
                case ReceiveStatus.ReadType:
                {
                    var type = (PackageType)readBuffer.Pop();
                    switch (type)
                    {
                        case PackageType.Ping:
                        {
                            ProcessPing();
                            return;
                        }
                        case PackageType.Data:
                        {
                            ProcessDataLength();
                            return;
                        }
                        default:
                        {
                            Connection.Close(ConnectionCloseType.UnrecognizedPackage);
                            return;
                        }
                    }
                }
                case ReceiveStatus.ReadDataLength:
                {
                    ProcessData();
                    return;
                }
                case ReceiveStatus.ReadData:
                {
                    ProcessDataDone();
                    return;
                }
            }
        }

        private void ProcessPing()
        {
            lastReceiveTime = nowTime;
            Connection.SendBuffer.ReplyPing();
            NextPackage();
        }

        private void ProcessDataLength()
        {
            status = ReceiveStatus.ReadDataLength;
            expectLength = TcpConstant.DataLenghFieldLength;
            ProcessReceive();
        }

        private void ProcessData()
        {
            status = ReceiveStatus.ReadData;
            var b2 = readBuffer.Pop(TcpConstant.DataLenghFieldLength);
            expectLength = TcpConstant.BitConvert.ToUInt16(b2);

            if (expectLength > Config.PackageMaxSize)
            {
                Connection.Close(ConnectionCloseType.PackageSizeTooLarge);
            }

            ProcessReceive();
        }

        private void ProcessDataDone()
        {
            lastReceiveTime = nowTime;
            var data = readBuffer.Pop(expectLength);
            Connection.Server.OnConnectionReceive(Connection, data);

            NextPackage();
        }

        private void NextPackage()
        {
            status = ReceiveStatus.ReadType;
            expectLength = TcpConstant.HeadTypeLength;
            ProcessReceive();
        }

        public void Shutdown()
        {
            Socket.Shutdown(SocketShutdown.Receive);
        }

        public void Update(long timestamp)
        {
            nowTime = timestamp;

            if (Config.KeepAlive > 0 && lastReceiveTime + Config.KeepAlive < nowTime)
            {
                Connection.Close(ConnectionCloseType.KeepAliveTimeout);
            }
        }
    }

}
