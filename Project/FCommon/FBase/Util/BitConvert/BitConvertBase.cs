namespace FFF.Base.Util.BitConvert
{

    public abstract class BitConvertBase : IBitConvert
    {

        public virtual ushort ToUInt16(byte[] bs)
        {
            return ToUInt16(bs, 0);
        }

        public abstract ushort ToUInt16(byte[] bs, int offset);

    }

}