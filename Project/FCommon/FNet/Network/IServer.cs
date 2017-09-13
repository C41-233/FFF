using FFF.Base.Util;

namespace FNet.Network
{
    public interface IServer
    {

        event FAction<IConnection> OnClientConnected;
        event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;

        void BeginAccept();

        void Update();

        void Close();

    }
}
