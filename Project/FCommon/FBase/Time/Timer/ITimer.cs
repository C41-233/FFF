namespace FFF.Base.Time.Timer
{

    public interface ITimer
    {

        long Remain { get; }

        void Stop();

    }

}
