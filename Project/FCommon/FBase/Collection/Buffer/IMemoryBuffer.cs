using System;

namespace FFF.Base.Collection.Buffer
{

    public interface IMemoryBuffer
    {
        
        int Length { get; }

        void Push(byte b);
        void Push(byte[] bs);
        void Push(byte[] bs, int offset, int len);

        byte Pop();
        byte[] Pop(int len);
        void Pop(int len, byte[] bs, int offset);
        byte[] PopAll();
        void PopAll(byte[] bs, int offset);

        byte Peek();
        byte[] Peek(int len);
        void Peek(int len, byte[] bs, int offset);

        void Clear();

    }

    public abstract class MemoryBufferBase : IMemoryBuffer
    {

        public abstract int Length { get; }

        public abstract void Push(byte b);

        public virtual void Push(byte[] bs)
        {
            Push(bs, 0, bs.Length);
        }

        public virtual void Push(byte[] bs, int offset, int len)
        {
            if (offset < 0 || len < 0)
            {
                throw new ArgumentException();
            }
            if (bs == null)
            {
                throw new ArgumentNullException();
            }
            if (offset + len > bs.Length)
            {
                throw new ArgumentException();
            }
            for (var i = 0; i < len; i++)
            {
                Push(bs[offset+i]);
            }
        }

        public abstract byte Pop();

        public virtual byte[] Pop(int len)
        {
            if (len < 0)
            {
                throw new ArgumentException();
            }
            var rst = new byte[len];
            Pop(len, rst, 0);
            return rst;
        }

        public virtual void Pop(int len, byte[] bs, int offset)
        {
            if (len < 0 || offset < 0)
            {
                throw new ArgumentException();
            }
            if (bs == null)
            {
                throw new ArgumentNullException();
            }
            if (len >= Length)
            {
                throw new InvalidOperationException();
            }
            for (var i = 0; i < len; i++)
            {
                bs[offset + i] = Pop();
            }
        }

        public virtual byte[] PopAll()
        {
            return Pop(Length);
        }

        public virtual void PopAll(byte[] bs, int offset)
        {
            Pop(Length, bs, offset);
        }

        public abstract byte Peek();

        public virtual byte[] Peek(int len)
        {
            var rst = new byte[len];
            Peek(len, rst, 0);
            return rst;
        }

        public abstract void Peek(int len, byte[] bs, int offset);

        public abstract void Clear();
    }
}
