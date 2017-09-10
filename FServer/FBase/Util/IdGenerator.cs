namespace FFF.Base.Util
{
    public class IdGenerator
    {

        private ulong current = 0;

        public ulong NextValue()
        {
            return current++;
        }

    }
}
