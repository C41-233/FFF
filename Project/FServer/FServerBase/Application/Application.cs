using System;
using FFF.Server.Application.Coroutine;
using FFF.Server.Application.Tick;
using FFF.Server.Application.Timer;

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

                TimerManager.OnTick();
                Coroutines.OnTick();
            }
            app.OnDestroy();
        }

    }
}
