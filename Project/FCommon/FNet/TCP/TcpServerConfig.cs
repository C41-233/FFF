using System.Net;

namespace FFF.Network.TCP
{
    public class TcpServerConfig
    {

        public IPAddress Ip
        {
            get => ip;
            set => ip = value ?? IPAddress.Any;
        }

        public string IpAsString
        {
            get => ip.ToString();
            set => ip = value==null ? IPAddress.Any : IPAddress.Parse(value);
        }

        public int MaxConnection = 1000;

        private IPAddress ip = IPAddress.Any;

        public int Port = 13000;

        public int Backlog = 20;

        /// <summary>
        /// 心跳包的超时时间
        /// 0表示不检测心跳
        /// </summary>
        public long KeepAlive = 30000;

        /// <summary>
        /// 收发单个消息包的最大数据长度
        /// </summary>
        public ushort PackageMaxSize = 8192;

        /// <summary>
        /// 接收缓冲区的大小
        /// </summary>
        public int ReadCacheSize = 2048;

        /// <summary>
        /// 当调用Send时，是否立即发送报文
        /// true：立即发送报文
        /// false：仅在Update中发送报文
        /// </summary>
        public bool SendImmediately = false;

    }
}
