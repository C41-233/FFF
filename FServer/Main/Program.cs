using FFF.Base.Time;
using FFF.Server.Application;
using System;
using System.Threading;
using FFF.Server.Time;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FApplication.Run<MainApplication>(args, new FApplicationConfig()
            {
                Tick = 1000,
            });
        }
    }

    class MainApplication : IFApplication
    {

        void IFApplication.OnInit(string[] args)
        {
            Console.WriteLine("init");
        }

        void IFApplication.OnDestroy()
        {
            Console.WriteLine("stop");
        }

        void IFApplication.OnTick()
        {
            var logic = TimeTick.Now.TimeStamp;
            var real = TimeTick.NowReal.TimeStamp;
            Console.WriteLine($"{logic} {real} {logic-real}");
        }
    }
}
