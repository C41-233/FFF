using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFF.Server.Application
{
    public interface IFApplication
    {

        void OnStart();

        void OnTick();

        void OnStop();

    }
}
