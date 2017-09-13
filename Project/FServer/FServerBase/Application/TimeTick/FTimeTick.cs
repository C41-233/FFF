using FFF.Base.Time;

namespace FFF.Server.Application.TimeTick
{

    public static class FTimeTick
    {

        public static FDateTime Now { get; internal set; }

        public static FDateTime NowReal { get; internal set; }

        public static FDateTime Last { get; internal set; }

        public static FDateTime LastReal { get; internal set; }

        public static FDateTime Start { get; internal set; }

        public static long MillisecondsFromStart => Now.TimeStamp - Start.TimeStamp;

        public static long MillisecondsFromStartReal => NowReal.TimeStamp - Start.TimeStamp;

    }

}
