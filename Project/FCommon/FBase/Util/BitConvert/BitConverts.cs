namespace FFF.Base.Util.BitConvert
{

    public static class BitConverts
    {
        
        public static IBitConvert LittleEndian { get; } = new LittleEndianBitConvert();

    }

    public interface IBitConvert
    {

        ushort ToUInt16(byte[] bs);
        ushort ToUInt16(byte[] bs, int offset);

    }
}
