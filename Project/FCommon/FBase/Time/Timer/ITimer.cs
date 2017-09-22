namespace FFF.Base.Time.Timer
{

    public interface ITimer
    {

        long StartTime { get; }

        long TimeoutTime { get; }

        long RemainTime { get; }

        string Name { get; set; }

        void Stop();

    }

}
