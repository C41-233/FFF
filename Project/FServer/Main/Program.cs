using FFF.Base.Util.Coroutine.Yield;
using FFF.Network.Base;
using FFF.Server.Application;
using FFF.Server.Application.Tick;
using System;
using System.Collections;

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
            Coroutines.StartCoroutine(Do);
            Coroutines.OnException += (c, e) =>
            {
                Console.WriteLine("fuck");
            };
        }

        IEnumerator Do()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Console.WriteLine(TimeTick.MillisecondsFromStart);
            }

        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
        }

    }
}
