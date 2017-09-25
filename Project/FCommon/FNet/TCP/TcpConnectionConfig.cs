namespace FFF.Network.TCP
{
    internal class TcpConnectionConfig
    {

        public TcpServer Server;

        public bool SendImmediately;
        public long KeepAlive;
        public bool NoDelay;
        public int ReadCacheSize;
        public int PackageMaxSize;

    }
}
