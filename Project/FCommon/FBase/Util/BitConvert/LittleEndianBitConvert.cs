namespace FFF.Base.Util.BitConvert
{
    public class LittleEndianBitConvert : BitConvertBase
    {

        public override ushort ToUInt16(byte[] bs, int offset)
        {
            return (ushort)
            (
                bs[offset] 
                | bs[offset + 1] << 8
            );

        }

    }
}