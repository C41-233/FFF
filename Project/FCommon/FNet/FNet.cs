using FFF.Network.Base;
using FFF.Network.TCP.Client;
using FFF.Network.TCP.Server;

namespace FFF.Network
{
    public static class FNet
    {

        public static IServer CreateTcpServer(TcpServerConfig config)
        {
            return new TcpServer(config);
        }

        public static IClient ConnectToTcpServer(TcpClientConfig config)
        {
            return new TcpClient(config);
        }

    }
}
