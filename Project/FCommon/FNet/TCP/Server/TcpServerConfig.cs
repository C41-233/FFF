using System.Net;

namespace FFF.Network.TCP.Server
{
    public class TcpServerConfig
    {

        public IPAddress IP
        {
            get => ip;
            set => ip = value ?? IPAddress.Any;
        }

        public string IpAsString
        {
            get => ip.ToString();
            set => ip = value==null ? IPAddress.Any : IPAddress.Parse(value);
        }

        /// <summary>
        /// 所允许的TCP最大连接数
        /// 0表示无限制，但最终还是要受操作系统限制
        /// </summary>
        public int MaxConnection = 0;

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

        /// <summary>
        /// 当发送数据时是否阻塞
        /// </summary>
        public bool SendBlock = true;

        public bool NoDelay = true;

    }

}
