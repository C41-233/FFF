using System.Net;

namespace FNet.TCP
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

    }
}
