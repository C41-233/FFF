using System;
using FFF.Base.Time;
using FFF.Base.Util.Generator;

namespace FFF.Network.Base
{
    internal static class ConnectionIdProvidor
    {

        private static readonly SequenceULongGenerator generator;

        static ConnectionIdProvidor()
        {
            var random = new Random();

            long low32 = random.Next();
            long high32 = FDateTime.Now.TimeStamp << 32;
            
            generator = new SequenceULongGenerator((ulong) (high32 | low32));
        }

        public static ulong NextValue()
        {
            return generator.NextValue();
        }

    }
}
