using System;

namespace FFF.Base.Collection.Buffer
{
    public class MemoryBuffer : MemoryBufferBase
    {

        private const int DefaultBufferSize = 1024;

        private byte[] buffer;

        private int head;
        private int tail;

        public override int Length => tail - head;

        public int Capacity => buffer.Length;

        public MemoryBuffer()
        {
            this.buffer = new byte[DefaultBufferSize];
        }

        private void TryPushAdjust(int len)
        {
            if (tail + len < Capacity)
            {
                return;
            }

            //Ä©¶ËÒç³ö
            int newLen = Capacity;
            if (head < Capacity / 4 || (Capacity - Length) < len)
            {
                newLen = Capacity * 2;
                while (newLen < Length + len)
                {
                    newLen *= 2;
                }
            }

            var newBuffer = new byte[newLen];
            System.Buffer.BlockCopy(buffer, head, newBuffer, 0, Length);
            buffer = newBuffer;

            tail -= head;
            head = 0;
        }

        private void TryPopAdjust()
        {
            if (Capacity >= DefaultBufferSize * 2 && Length < Capacity / 4)
            {
                var newBuffer = new byte[Capacity / 2];
                System.Buffer.BlockCopy(buffer, head, newBuffer, 0, Length);
                buffer = newBuffer;
            }
            else if (head > Capacity / 2)
            {
                System.Buffer.BlockCopy(buffer, head, buffer, 0, Length);
            }
        }

        public override void Push(byte b)
        {
            TryPushAdjust(1);
            buffer[tail++] = b;
        }

        public override void Push(byte[] bs, int offset, int len)
        {
            if (offset <0 || len <0)
            {
                throw new ArgumentException();
            }
            if (bs == null)
            {
                throw new ArgumentNullException();
            }
            if (offset + len > bs.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            TryPushAdjust(len);
            System.Buffer.BlockCopy(bs, offset, buffer, tail, len);
            tail += len;
        }

        public override byte Pop()
        {
            if (head == tail)
            {
                throw new InvalidOperationException();
            }
            var rst = buffer[head++];
            TryPopAdjust();
            return rst;
        }

        public override byte[] PopAll()
        {
            var rst = new byte[Length];
            System.Buffer.BlockCopy(buffer, head, rst, 0, Length);
            head = tail = 0;
            TryPopAdjust();
            return rst;
        }

        public override void Pop(int len, byte[] bs, int offset)
        {
            if (len <= 0)
            {
                throw new ArgumentException();
            }
            if (offset + len > bs.Length)
            {
                throw new ArgumentException();
            }
            if (len > Length)
            {
                throw new InvalidOperationException();
            }

            System.Buffer.BlockCopy(buffer, head, bs, offset, len);
            head += len;
            TryPopAdjust();
        }

        public override byte Peek()
        {
            if (head == tail)
            {
                throw new InvalidOperationException();
            }
            return buffer[head];
        }

        public override void Peek(int len, byte[] bs, int offset)
        {
            if (len <= 0)
            {
                throw new ArgumentException();
            }
            if (bs == null)
            {
                throw new ArgumentNullException();   
            }
            if (Length < len)
            {
                throw new InvalidOperationException();
            }
            System.Buffer.BlockCopy(buffer, head, bs, offset, len);
        }

        public override void Clear()
        {
            head = tail = 0;
            TryPopAdjust();
        }
    }
}