using FFF.Network.Base;
using FFF.Network.TCP;

namespace FFF.Network.Server
{
    public static class FNet
    {

        public static IServer CreateTCPServer(TcpServerConfig config)
        {
            return new TcpServer(config);
        }

    }
}
