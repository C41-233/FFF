using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FFFUnitTest.Base.Collection
{
    [TestClass]
    public class PriorityQueueTest2
    {

        private PriorityQueue<int> queue;

        [TestInitialize]
        public void Build()
        {
            queue = new PriorityQueue<int>();
            foreach (var i in F.For(100000))
            {
                queue.Add(i);
            }
        }

        [TestMethod]
        public void TestAdd200000()
        {
            var newQueue = new PriorityQueue<int>();
            foreach (var i in F.For(200000, 0))
            {
                newQueue.Add(i);
                Assert.AreEqual(i, newQueue.First);
            }
        }

        [TestMethod]
        public void TestRemove()
        {
            foreach (var i in F.For(100000))
            {
                Assert.AreEqual(i, queue.RemoveFirst());
            }
        }

        [TestMethod]
        public void TestAdd()
        {
            queue.Add(7534);
            Assert.AreEqual(0, queue.FirstNode.Value);

            var node = queue.FirstNode;
            node.Remove();
            Assert.AreEqual(1, queue.First);

        }
    }
}
