using FFF.Base.Util.Generator;

namespace FNet.Network
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
