namespace FFF.Base.Collection.Buffer
{
    public class ByteArrayBuffer
    {

        public byte[] Buffer { get; private set; }

        public int Capacity => Buffer.Length;


        public int Offset { get; private set; }
        public int Tail { get; private set; }

        public int Remain => Capacity - Tail;
        public int Length => Tail - Offset;

        public ByteArrayBuffer()
        {
            Buffer = new byte[1024];
        }

        public ByteArrayBuffer(int capacity)
        {
            Buffer = new byte[capacity];
        }

        public void PrepareSize(int len)
        {
            if (Tail + len < Capacity)
            {
                return;
            }

            int newLen = Capacity;
            if (Offset < Capacity / 4 || (Capacity - Length) < len)
            {
                newLen = Capacity * 2;
                while (newLen < Length + len)
                {
                    newLen *= 2;
                }
            }

            var newBuffer = new byte[newLen];
            System.Buffer.BlockCopy(Buffer, Offset, newBuffer, 0, Length);
            Buffer = newBuffer;

            Tail -= Offset;
            Offset = 0;
        }
    }
}
