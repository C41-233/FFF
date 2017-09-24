namespace FFF.Base.Util.BitConvert
{

    public static class BitConverts
    {
        
        public static IBitConvert LittleEndian { get; } = new LittleEndianBitConvert();

    }

}
