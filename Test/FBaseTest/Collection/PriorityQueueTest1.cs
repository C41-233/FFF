using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FFFUnitTest.Base.Collection
{
    [TestClass]
    public class PriorityQueueTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var queue = new PriorityQueue<int>();
            queue.Add(1, 2, 3, 4, 3, 2, 1);
            Assert.AreEqual(7, queue.Count);
            Assert.AreEqual(1, queue.First);
            Assert.AreEqual(1, queue.RemoveFirst());
            Assert.AreEqual(1, queue.RemoveFirst());
            Assert.AreEqual(2, queue.RemoveFirst());
            Assert.AreEqual(2, queue.RemoveFirst());
            Assert.AreEqual(3, queue.RemoveFirst());
            Assert.AreEqual(3, queue.RemoveFirst());
            Assert.AreEqual(4, queue.RemoveFirst());
            Assert.AreEqual(0, queue.Count);
            queue.Add(7);
            Assert.AreEqual(7, queue.First);
            queue.Add(14);
            Assert.AreEqual(7, queue.First);
            queue.Add(3);
            Assert.AreEqual(3, queue.First);
            Assert.AreEqual(3, queue.Count);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var queue = new PriorityQueue<int>();
            Assert.AreEqual(0, queue.Count);
            Assert.ThrowsException<InvalidOperationException>(()=> queue.First);
            Assert.ThrowsException<InvalidOperationException>(() => queue.RemoveFirst());
            queue.Add(14);
            Assert.AreEqual(14, queue.First);
            Assert.AreEqual(1, queue.Count);

            var node = queue.FirstNode;
            node.Remove();
            Assert.AreEqual(14, node.Value);
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var queue = new PriorityQueue<int>();
            queue.Add(15, 16);
            Assert.AreEqual(2, queue.Count);
            Assert.AreEqual(15, queue.First);

            PriorityQueue<int>.Node node;

            node = queue.FirstNode;
            node.Value = 20;
            Assert.AreEqual(2, queue.Count);
            Assert.AreEqual(20, node.Value);
            Assert.AreEqual(16, queue.First);

            node = queue.Add(4);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual(4, queue.First);
            Assert.AreSame(node, queue.FirstNode);

            node.Value = 7;
            Assert.AreEqual(7, node.Value);
            Assert.AreEqual(7, queue.First);
            Assert.AreSame(node, queue.FirstNode);

            node.Value = 17;
            Assert.AreEqual(17, node.Value);
            Assert.AreEqual(16, queue.First);
            Assert.AreNotSame(node, queue.FirstNode);

            node = queue.FirstNode;
            Assert.AreEqual(16, node.Value);

            queue.RemoveFirst();
            Assert.AreEqual(16, node.Value);
            Assert.AreEqual(17, queue.First);

            Assert.ThrowsException<InvalidOperationException>(() => node.Value = 1);
            Assert.ThrowsException<InvalidOperationException>(() => node.Remove() );
            Assert.AreEqual(17, queue.First);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var queue = new PriorityQueue<int>();
            queue.Add(1,5,43,3,999,-11);
            var node = queue.FirstNode;
            Assert.AreEqual(-11, node.Value);
            node++;
            Assert.AreEqual(-11, queue.First);

            node.Remove();
            Assert.AreEqual(1, queue.First);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var queue = new PriorityQueue<int>();
            foreach (var i in F.For(100000))
            {
                queue.Add(i);
            }
            Assert.AreEqual(100000, queue.Count);
            Assert.AreEqual(0, queue.First);

            foreach (var unused in F.For(100000))
            {
                queue.RemoveFirst();
            }
            Assert.AreEqual(0, queue.Count);
        }
    }
}
