using FFF.Base.Util.Generator;

namespace FFF.Network.Base
{
    internal static class ConnectionIdProvidor
    {

        private static readonly SequenceULongGenerator generator = new SequenceULongGenerator();

        public static ulong NextValue()
        {
            return generator.NextValue();
        }

    }
}
