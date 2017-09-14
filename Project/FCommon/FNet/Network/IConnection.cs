namespace FNet.Network
{

    public interface IConnection
    {

        void BeginReceive();

        //void EndReceive();

        void Close();

    }
}
