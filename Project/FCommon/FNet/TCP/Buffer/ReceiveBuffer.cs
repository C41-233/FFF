using System;
using System.Collections.Generic;
using System.Net.Sockets;
using FFF.Base.Collection;
using FNet.Network;
using FNet.TCP.Protocol;

namespace FNet.TCP.Buffer
{

    internal class ReceiveBuffer
    {

        private enum ReceiveStatus
        {
            ReadType,
            ReadDataLength,
            ReadData,
        }

        private readonly FTcpConnection Connection;
        private Socket Socket => Connection.Socket;

        private readonly List<byte[]> readQueue = new List<byte[]>();
        private readonly FMemoryBuffer readBuffer = new FMemoryBuffer();
        private readonly byte[] readCache;

        private ReceiveStatus status;
        private int expectLength;

        public ReceiveBuffer(FTcpConnection conn, FTcpConnectionConfig config)
        {
            this.Connection = conn;
            this.readCache = new byte[config.ReadCacheSize];
        }

        public void BeginReceive()
        {
            NextPackage();
        }

        private void DoReceive()
        {
            Socket.BeginReceive(readCache, 0, readCache.Length, SocketFlags.None, Receive, null);
        }

        private void Receive(IAsyncResult ar)
        {
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
                            //包类型不能识别
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
            Connection.Sender.ReplyPing();
            NextPackage();
        }

        private void ProcessDataLength()
        {
            //todo 最大长度限制
            status = ReceiveStatus.ReadDataLength;
            expectLength = FTcpProperty.DataLenghtFieldLength;
            ProcessReceive();
        }

        private void ProcessData()
        {
            status = ReceiveStatus.ReadData;
            var b2 = readBuffer.Pop(2);
            expectLength = BitConverter.ToUInt16(b2, 0);
            ProcessReceive();
        }

        private void ProcessDataDone()
        {
            var data = readBuffer.Pop(expectLength);
            readQueue.Add(data);
            Console.WriteLine(expectLength + "  "+System.Text.Encoding.Default.GetString(data));

            NextPackage();
        }

        private void NextPackage()
        {
            status = ReceiveStatus.ReadType;
            expectLength = FTcpProperty.HeadTypeLength;
            DoReceive();
        }

    }

}
