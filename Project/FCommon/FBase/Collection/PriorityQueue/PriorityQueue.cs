using System;
using System.Collections;
using System.Collections.Generic;

namespace FFF.Base.Collection.PriorityQueue
{
    public class PriorityQueue<T> : IPriorityQueue<T>, IEnumerable<T>
    {

        public int Count { get; private set; }
        private readonly ValueNodeList buffer = new ValueNodeList();

        private readonly Comparer<T> comparer;

        private int version = 0;

        public PriorityQueue()
        {
            this.comparer = Comparer<T>.Default;
        }

        public PriorityQueue(Comparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public T First => FirstNode.Value;

        public Node FirstNode
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException();
                }
                return buffer[1];
            }
        }

        void IPriorityQueue<T>.Add(T value)
        {
            Add(value);
        }

        public Node Add(T value)
        {
            version++;
            buffer.TryAddAdjust(Count);

            var node = new ValueNode(this, value);
            buffer[++Count] = node;
            ShiftUp(Count);

            return node;
        }

        public T RemoveFirst()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            var first = buffer[1];
            PopFirstNode();
            first.Dispose();

            return first.Value;
        }

        private void PopFirstNode()
        {
            version++;
            buffer[1] = buffer[Count--];
            ShiftDown(1);
            buffer.TryRemoveAjust(Count);
        }

        private void ShiftUp(int hole)
        {
            version++;
            var value = buffer[hole];
            while (hole > 1 && Less(value, buffer[hole / 2]))
            {
                buffer[hole] = buffer[hole / 2];
                hole /= 2;
            }
            buffer[hole] = value;
        }

        private void ShiftDown(int hole)
        {
            version++;
            var tmp = buffer[hole];
            while (hole * 2 <= Count)
            {
                var child = hole * 2;
                if (child != Count && Less(buffer[child+1], buffer[child]))
                {
                    child++;
                }
                if (Less(buffer[child], tmp))
                {
                    buffer[hole] = buffer[child];
                }
                else
                {
                    break;
                }
                hole = child;
            }
            buffer[hole] = tmp;
        }

        private bool Less(Node a, Node b)
        {
            if (a == b)
            {
                return false;
            }
            if (a == null)
            {
                return true;
            }
            return comparer.Compare(a.Value, b.Value) < 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int snapshot = version;
            for (var i = 1; i <= Count; i++)
            {
                yield return buffer[i].Value;
                if (snapshot != version)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ValueNodeList
        {

            private const int DefaultSize = 16;

            private ValueNode[] buffer = new ValueNode[DefaultSize];

            private void Resize(int len)
            {
                Array.Resize(ref buffer, len);
            }

            public void TryAddAdjust(int Count)
            {
                if (Count - 1 == buffer.Length)
                {
                    Resize(buffer.Length * 2);
                }
            }

            public void TryRemoveAjust(int Count)
            {
                if (buffer.Length >= DefaultSize * 2 && Count - 1 < buffer.Length / 4)
                {
                    Resize(buffer.Length / 2);
                }
            }

            public ValueNode this[int n]
            {
                get => buffer[n];
                set
                {
                    buffer[n] = value;
                    if (value != null)
                    {
                        value.index = n;
                    }
                }
            }

        }

        public abstract class Node
        {
            /// <summary>
            /// set: 获取结点的值
            /// get: 设置当前结点的值，会触发重排序
            /// </summary>
            public abstract T Value { get; set; }

            /// <summary>
            /// 从优先队列中删除当前结点，此后不再可以操作当前结点
            /// </summary>
            public abstract void Remove();

            /// <summary>
            /// 提示当前结点的值出现增加，需要重排序
            /// </summary>
            public abstract void ValueUp();

            /// <summary>
            /// 提示当前结点的值出现减少，需要重排序
            /// </summary>
            public abstract void ValueDown();

            /// <summary>
            /// 同ValueUp
            /// </summary>
            public static Node operator ++(Node node)
            {
                node.ValueUp();
                return node;
            }
            
            /// <summary>
            /// 同ValueDown
            /// </summary>
            public static Node operator --(Node node)
            {
                node.ValueDown();
                return node;
            }
        }

        private class ValueNode: Node
        {

            public int index;

            private PriorityQueue<T> queue;

            public ValueNode(PriorityQueue<T> queue, T value)
            {
                this.queue = queue;
                this.value = value;
            }

            public void Dispose()
            {
                this.queue = null;
            }

            public override T Value
            {
                get => value;
                set
                {
                    if (queue == null)
                    {
                        throw new InvalidOperationException();
                    }

                    var oldValue = this.value;
                    this.value = value;

                    int comp = queue.comparer.Compare(this.value, oldValue);
                    if (comp == 0)
                    {
                        return;
                    }
                    if (comp < 0)
                    {
                        ValueDown();
                    }
                    else
                    {
                        ValueUp();
                    }
                }
            }

            public override void ValueUp()
            {
                if (queue == null)
                {
                    throw new InvalidOperationException();
                }
                queue.ShiftDown(index);
            }

            public override void ValueDown()
            {
                if (queue == null)
                {
                    throw new InvalidOperationException();
                }
                queue.ShiftUp(index);
            }

            private T value;

            public override void Remove()
            {
                if (queue == null)
                {
                    throw new InvalidOperationException();
                }
                queue.buffer[index] = null;
                queue.ShiftUp(index);
                queue.PopFirstNode();
                Dispose();
            }

        }

    }

}
