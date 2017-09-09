using FFF.Base.Time;

namespace FFF.Server.Time
{

    public static class TimeTick
    {

        public static FDateTime Now { get; internal set; }

        public static FDateTime NowReal { get; internal set; }

        public static FDateTime Last { get; internal set; }

        public static FDateTime LastReal { get; internal set; }

    }

}
