using FFF.Base.Linq;
using FFF.Base.Time.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FFFUnitTest.Base.Timer
{
    [TestClass]
    public class TimerTestTime
    {

        private TimerManager timer;

        [TestInitialize]
        public void Init()
        {
            timer = new TimerManager();
            foreach (var i in F.For(100000))
            {
                timer.StartTimer(i, () => { });
            }
        }

        [TestMethod]
        public void TestAdd200000()
        {
            var myTimer = new TimerManager();
            foreach (var i in F.For(200000))
            {
                myTimer.StartTimer(i, ()=>{});
            }
        }

        [TestMethod]
        public void TestAddAnother200000()
        {
            foreach (var i in F.For(200000))
            {
                timer.StartTimer(i, () => { });
            }
        }
    }
}
