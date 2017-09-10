using System;
using FFF.Server.Application.Time;
using FFF.Server.Coroutine;

namespace FFF.Server.Application
{
    public static class FApplication
    {

        public static void Run<T>(string[] args)
            where T : IFApplication, new()
        {
            var config = new FApplicationConfig();
            Run<T>(args, config);
        }

        public static void Run<T>(string[] args, FApplicationConfig config)
            where T : IFApplication, new()
        {
            var app = new T();

            var timeFrame = new TimeFrame(config.TickReal, config.TickLogic);

            app.OnInit(args);
            while (true)
            {
                timeFrame.JoinFrame();

                try
                {
                    CoroutineManager.OnTick();
                    app.OnTick();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }
            app.OnDestroy();
        }

    }
}
