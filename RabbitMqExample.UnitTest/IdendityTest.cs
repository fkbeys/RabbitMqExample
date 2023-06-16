//using Moq;
//using RabbitMqExample.Common.Services;

//namespace RabbitMqExample.UnitTest
//{
//    public class IdendityTest
//    {

//        private Mock<IIdendityService> _idendityService;

//        [SetUp()]
//        public void SetUp()
//        {
//            _idendityService = new Mock<IIdendityService>(MockBehavior.Strict);
//            _idendityService.Setup(m => m.CheckNumber(It.IsAny<string>()));
//        }


//        [Test]
//        public void MyTestMethod()
//        {
//            var fx = _idendityService.Setup(m => m.CheckNumber(It.IsAny<string>())).Returns(true);
//            var result = _idendityService.Object.CheckNumber("");

//            Assert.AreEqual(result, true);
//        }

//    }
//}
