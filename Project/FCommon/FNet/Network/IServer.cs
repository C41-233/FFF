using FFF.Base.Util;

namespace FNet.Network
{
    public interface IServer
    {

        //当一个客户端连接时触发，在Update线程中调用
        event FAction<IConnection> OnClientConnected;
        //当一个客户端断开连接时触发，在Update线程中调用
        event FAction<IConnection, ConnectionCloseType> OnClientDisconnected;

        //开始接收客户端连接，此后可以调用EndAccept停止接收客户端连接
        void BeginAccept();

        //不再接收客户端连接，此后可以调用BeginAccept重新接收客户端连接
        //void EndAccept();

        /// <summary>
        /// 网络时序帧更新，应用程序必须以一定帧率调用Update，保证网络事件的正确处理。所有的回调都会在Update调用中触发，以保证单线程执行。
        /// </summary>
        void Update();

        //关闭服务器连接
        void Close();

    }
}
