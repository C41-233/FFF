﻿using FFF.Server.Application.TimeTick;
using FFF.Server.Coroutine;
using FFF.Server.Timer;
using System;

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
                    app.OnTick();
                    TimerManager.OnTick();
                    CoroutineManager.OnTick();
                }
                catch (Exception e)
                {
                    //todo 异常处理
                    Console.WriteLine(e);
                    break;
                }
            }
            app.OnDestroy();
        }

    }
}
