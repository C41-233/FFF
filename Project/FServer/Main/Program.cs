using FFF.Network.Base;
using FFF.Network.TCP;
using FFF.Server.Application;
using FFF.Server.Application.Tick;
using System;
using System.Text;
using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using FFF.Base.Time;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Application.Run<MainApplication>(args, new ApplicationConfig()
            {
                Tick= 50,
            });
            Console.Read();
        }
    }

    class MainApplication : IApplication
    {

        private IServer server;

        void IApplication.OnInit(string[] args)
        {
            Console.WriteLine("start");
            Timers.StartTimer(5000, () =>
            {
                Console.WriteLine($"{TimeTick.MillisecondsFromStartReal}");
                Console.WriteLine($"{TimeTick.MillisecondsFromStart}");
            });
        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
        }

    }
}
