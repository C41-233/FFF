using System.Net;

namespace FFF.Network.TCP.Client
{
    public class TcpClientConfig
    {

        public IPAddress IP;

        public string IpAsString
        {
            get => IP.ToString();
            set => IP = IPAddress.Parse(value);
        }

        public string Host;

        public int Port;

    }
}
