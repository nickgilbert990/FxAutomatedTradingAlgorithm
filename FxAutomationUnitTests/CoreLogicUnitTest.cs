using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using cAlgo.API;
using cAlgo.Indicators;
using cAlgo.Main;

namespace FxAutomationUnitTests
{
    [TestClass]
    public class CoreLogicUnitTest
    {
        [TestMethod]
        public void TestMoq()
        {
            var bot = new SampleTrendcBot();
            var mockIndicator = new Mock<IIndicators>();
            //mockIndicator.Setup(x => x.IndicatorAlert()).Returns("AlertLong");
            //Assert.IsTrue(mockIndicator.Object.IndicatorAlert().Equals("AlertLong"));
            //var mockCloseOrders = new Mock<CloseOrders>();
            //mockCloseOrders.Setup(x => x.ClosePosition(It.IsAny<string>())).Returns(false);
            //Assert.IsTrue(true);

        }
    }
}
