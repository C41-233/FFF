namespace FFF.Base.Util.Generator
{
    public interface IValueGenerator<out T>
    {
        T NextValue();
    }
}