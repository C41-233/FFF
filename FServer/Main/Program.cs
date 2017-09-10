using FFF.Base.Time;
using FFF.Server.Application;
using FFF.Server.Application.TimeTick;
using FFF.Server.Timer;
using System;
using System.Net.Sockets;

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
            //var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
        }

        void IFApplication.OnDestroy()
        {
        }

        void IFApplication.OnTick()
        {



        }

    }
}
