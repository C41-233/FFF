using FFF.Base.Time;
using FFF.Server.Application;
using FFF.Server.Application.TimeTick;
using FFF.Server.Timer;
using System;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FApplication.Run<MainApplication>(args, new FApplicationConfig()
            {
                Tick= 20,
            });
            Console.Read();
        }
    }

    class MainApplication : IFApplication
    {

        void IFApplication.OnInit(string[] args)
        {
            Console.WriteLine($"{FTimeTick.MillisecondsFromStart} init");
            FTimers.StartTimerAt(new FDateTime(2017, 9, 10, 16, 16), () => { Console.WriteLine("1233");});
        }

        void IFApplication.OnDestroy()
        {
            Console.WriteLine("stop");
        }

        void IFApplication.OnTick()
        {
        }

    }
}
