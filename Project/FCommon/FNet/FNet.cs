using FFF.Network.Base;
using FFF.Network.TCP;

namespace FFF.Network
{
    public static class FNet
    {

        public static IServer CreateTCPServer(TcpServerConfig config)
        {
            return new TcpServer(config);
        }

    }
}
