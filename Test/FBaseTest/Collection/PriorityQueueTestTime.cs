using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FFFUnitTest.Base.Collection
{
    [TestClass]
    public class PriorityQueueTestTime
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
            foreach (var i in F.For(200000))
            {
                newQueue.Add(i);
            }
        }

        [TestMethod]
        public void TestAddAnother200000()
        {
            foreach (var i in F.For(200000))
            {
                queue.Add(i + 200000);
            }
        }

        [TestMethod]
        public void TestRemove100000()
        {
            foreach (var unused in F.For(100000))
            {
                queue.RemoveFirst();
            }
        }

        [TestMethod]
        public void TestRemove()
        {
            queue.RemoveFirst();
        }

        [TestMethod]
        public void TestAdd()
        {
            queue.Add(7534);
        }
    }
}
