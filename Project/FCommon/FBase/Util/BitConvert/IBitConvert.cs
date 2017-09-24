namespace FFF.Base.Util.BitConvert
{

    public interface IBitConvert
    {

        ushort ToUInt16(byte[] bs, int offset);

    }

    public static class BitConvertExtension
    {

        public static ushort ToUInt16(this IBitConvert c, byte[] bs)
        {
            return c.ToUInt16(bs, 0);
        }

    }
}