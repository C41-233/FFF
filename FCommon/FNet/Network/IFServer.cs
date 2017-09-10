using FFF.Base.Util;

namespace FNet.Network
{
    public interface IFServer
    {

        event FAction<IConnection> OnClientConnected;

        void BeginAccept();

    }
}
