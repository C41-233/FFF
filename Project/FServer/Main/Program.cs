using FFF.Base.Linq;
using FFF.Base.Util.Coroutine.Yield;
using FFF.Network.Base;
using FFF.Server.Application;
using FFF.Server.Application.Tick;
using System;
using System.Collections;
using FFF.Base.Util.Coroutine;

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
        }

        IEnumerator Do()
        {
            while (true)
            {
                foreach (var i in F.For(10000))
                {
                    Coroutines.StartCoroutine(Y);
                }
                yield return WaitFor.Seconds(1);
                Console.WriteLine(TimeTick.MillisecondsFromStartReal);
            }
        }

        void IApplication.OnDestroy()
        {
        }

        private bool first = true;

        void IApplication.OnTick()
        {
            if (first)
            {
                Console.WriteLine(TimeTick.MillisecondsFromStartReal);
                Coroutines.StartCoroutine(Do);
            }
            first = false;
        }

        IEnumerator Y()
        {
            var random = new Random();
            while (true)
            {
                switch (random.Next(3))
                {
                    case 0:
                        var wait = random.Next(60);
                        yield return WaitFor.Seconds(wait);
                        break;
                    default:
                        yield return null;
                        break;
                }
            }

        }

    }
}
