using FFF.Base.Util;

namespace FFF.Network.Base
{
    public interface IServer
    {

        /// <summary>
        /// 当一个客户端连接时触发，在Update线程中调用
        /// </summary>
        event FAction<IConnection> OnClientConnected;
        /// <summary>
        /// 当一个客户端断开连接时触发，在Update线程中调用
        /// </summary>
        event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;
        /// <summary>
        /// 当接收到客户端数据时触发，在Update线程中调用
        /// 参数的数据可以直接由应用程序使用和修改，底层服务保证收到的数据是一次Send调用发送的完整数据
        /// </summary>
        event FAction<IConnection, byte[]> OnClientReceive;
        /// <summary>
        /// 当调用Close()后，服务器必须逐步断开所有客户端连接并释放资源，当所有操作都完成后，才会触发OnServerClosed
        /// 因此，在调用Close后，应用层仍必须主动调用Update来更新时序帧，直到OnServerClosed被触发后，才可以停止调用Update
        /// </summary>
        event FAction<IServer> OnServerClosed;

        /// <summary>
        /// 开始接收客户端连接，此后可以调用EndAccept停止接收客户端连接
        /// </summary>
        void BeginAccept();

        /// <summary>
        /// 不再接收客户端连接，此后可以调用BeginAccept重新接收客户端连接
        /// </summary>
        void EndAccept();

        /// <summary>
        /// 网络时序帧更新，应用程序必须以一定帧率调用Update，保证网络事件的正确处理。所有的回调都会在Update调用中触发，以保证单线程执行。
        /// </summary>
        void Update();

        /// <summary>
        /// 关闭服务器连接
        /// 在调用Close后必须继续更新时序帧（Update），直到OnServerClosed被触发。
        /// </summary>
        void Close();

    }
}
