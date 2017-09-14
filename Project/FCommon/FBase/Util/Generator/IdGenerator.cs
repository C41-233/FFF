namespace FFF.Base.Util.Generator
{
    public class IdGenerator : IValueGenerator<ulong>
    {

        private ulong current;

        public IdGenerator(ulong init = 0)
        {
            this.current = init;
        }

        public ulong NextValue()
        {
            return current++;
        }

    }
}
