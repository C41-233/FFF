namespace FFF.Server.Application
{
    public interface IFApplication
    {

        void OnInit(string[] args);

        void OnTick();

        void OnDestroy();

    }
}
