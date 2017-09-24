using FFF.Server.Application.Tick;
using System;

namespace FFF.Server.Application
{
    public static class Application
    {

        public static void Run<T>(string[] args)
            where T : IApplication, new()
        {
            var config = new ApplicationConfig();
            Run<T>(args, config);
        }

        public static void Run<T>(string[] args, ApplicationConfig config)
            where T : IApplication, new()
        {
            var app = new T();

            var timeFrame = new TimeFrame(config.TickReal, config.TickLogic);

            Timers.OnInit();

            app.OnInit(args);
            while (true)
            {
                timeFrame.JoinFrame();

                try
                {
                    app.OnTick();
                }
                catch (Exception e)
                {
                    //todo 异常处理
                    Console.WriteLine(e);
                    break;
                }

                Timers.OnTick();
                Coroutines.OnTick();
            }
            app.OnDestroy();
        }

    }
}
