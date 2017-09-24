namespace FFF.Base.Util.BitConvert
{
    public sealed class LittleEndianBitConvert : IBitConvert
    {

        public ushort ToUInt16(byte[] bs, int offset)
        {
            return (ushort)
            (
                bs[offset] 
                | bs[offset + 1] << 8
            );

        }

    }
}