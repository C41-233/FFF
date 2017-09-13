using System;

namespace FFF.Base.Collection
{

    public class FMemoryBuffer
    {

        private byte[] buffer;

        private int head;
        private int tail;

        public int Length => tail - head;

        public int Capacity => buffer.Length;

        public FMemoryBuffer()
        {
            buffer = new byte[1024];
        }

        public FMemoryBuffer(int capacity)
        {
            buffer = new byte[capacity];
        }

        private void TryPushAdjust(int len)
        {
            if (tail + len < Capacity)
            {
                return;
            }

            //末端溢出
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
            Buffer.BlockCopy(buffer, head, newBuffer, 0, Length);
            buffer = newBuffer;

            tail -= head;
            head = 0;
        }

        public void Push(byte b)
        {
            TryPushAdjust(1);
            buffer[tail++] = b;
        }

        public void Push(byte[] bs)
        {
            Push(bs, 0 ,bs.Length);
        }

        public void Push(byte[] bs, int offset, int len)
        {
            if (offset + len > bs.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            TryPushAdjust(len);
            Buffer.BlockCopy(bs, offset, buffer, tail, len);
            tail += len;
        }

        public byte Pop()
        {
            if (head == tail)
            {
                throw new InvalidOperationException();
            }
            return buffer[head++];
        }

        public byte[] PopAll()
        {
            var rst = new byte[Length];
            Buffer.BlockCopy(buffer, head, rst, 0, Length);
            head = tail = 0;
            return rst;
        }

        public byte[] Pop(int len)
        {
            if (len <= 0)
            {
                throw new ArgumentException();
            }
            if (len > Length)
            {
                throw new InvalidOperationException();
            }

            var rst = new byte[len];
            Buffer.BlockCopy(buffer, head, rst, 0, len);
            head += len;
            return rst;
        }

        public byte Peek()
        {
            if (head == tail)
            {
                throw new InvalidOperationException();
            }
            return buffer[head];
        }

        public byte[] Peek(int len)
        {
            if (len <= 0)
            {
                throw new ArgumentException();
            }
            if (Length < len)
            {
                throw new InvalidOperationException();
            }
            var rst = new byte[len];
            Buffer.BlockCopy(buffer, head, rst, 0, len);
            return rst;
        }

        public void Clear()
        {
            head = tail = 0;
        }
    }
}
