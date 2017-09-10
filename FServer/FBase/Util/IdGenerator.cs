namespace FFF.Base.Util
{
    public class IdGenerator
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
