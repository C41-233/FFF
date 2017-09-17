using FFF.Base.Util;

namespace FNet.Network
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

        //开始接收客户端连接，此后可以调用EndAccept停止接收客户端连接
        void BeginAccept();

        //不再接收客户端连接，此后可以调用BeginAccept重新接收客户端连接
        void EndAccept();

        /// <summary>
        /// 网络时序帧更新，应用程序必须以一定帧率调用Update，保证网络事件的正确处理。所有的回调都会在Update调用中触发，以保证单线程执行。
        /// </summary>
        void Update();

        //关闭服务器连接
        void Close();

    }
}
