namespace FFF.Server.Application
{
    public class ApplicationConfig
    {

        public ulong TickReal = 50;
        public ulong TickLogic = 50;

        public ulong Tick
        {
            set
            {
                TickReal = value;
                TickLogic = value;
            }
        }

        public string Name = null;

    }
}
