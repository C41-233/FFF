namespace FNet.Network
{

    public interface IConnection
    {

        void BeginReceive();

        void Close();

        void Flush();

    }
}
