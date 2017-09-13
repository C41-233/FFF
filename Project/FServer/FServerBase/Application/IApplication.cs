namespace FFF.Server.Application
{
    public interface IApplication
    {

        void OnInit(string[] args);

        void OnTick();

        void OnDestroy();

    }
}
