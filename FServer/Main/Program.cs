using FFF.Server.Application;
using System;
using System.Collections;
using System.Threading;
using FFF.Base.Linq;
using FFF.Server.Application.Time;
using FFF.Server.Coroutine;
using FFF.Server.Coroutine.Yield;

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
            FCoroutines.StartCoroutine(Do);
        }

        void IFApplication.OnDestroy()
        {
            Console.WriteLine("stop");
        }

        void IFApplication.OnTick()
        {
        }

        IEnumerator Do()
        {
            Console.WriteLine($"{FTimeTick.MillisecondsFromStart} sleep");
            yield return new WaitForJob<int>(5, i =>
            {
                Console.WriteLine(i);
                Thread.Sleep(5000);
                Console.WriteLine(i);
            });
            Console.WriteLine($"{FTimeTick.MillisecondsFromStart} awake");
        }

        IEnumerator Do1()
        {
            foreach (var i in F.For(2))
            {
                Console.WriteLine($"{FTimeTick.MillisecondsFromStart} {i}");
                yield return new WaitForSeconds(1.5f);
            }
        }

        IEnumerator Do2()
        {
            Console.WriteLine($"{FTimeTick.MillisecondsFromStart} haha");
            yield return new WaitForSeconds(5);
            Console.WriteLine($"{FTimeTick.MillisecondsFromStart} haha");
        }

    }
}
