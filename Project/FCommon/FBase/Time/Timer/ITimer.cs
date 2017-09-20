namespace FFF.Base.Time.Timer
{

    public interface ITimer
    {

        long Timeout { get; }

        long Remain { get; }

        void Stop();

    }

}
