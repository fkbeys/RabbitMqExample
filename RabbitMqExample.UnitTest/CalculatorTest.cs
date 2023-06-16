//using Moq;

//namespace RabbitMqExample.UnitTest
//{
//    public interface ICalculator
//    {
//        int Add(int a, int b);
//        int Subtract(int a, int b);
//    }

//    [TestFixture]
//    public class CalculatorTest
//    {
//        [Test]
//        public void TestLooseMock()
//        {
//            // Arrange
//            var looseMock = new Mock<ICalculator>(MockBehavior.Strict);
//            looseMock.Setup(m => m.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(3);
//            looseMock.Setup(m => m.Subtract(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

//            // Act
//            //var result = looseMock.Object.Add(1, 1);

//            var subtractResult = looseMock.Object.Subtract(1, 1);
//            Assert.AreEqual(0, subtractResult);

//            // Assert
//            //Assert.AreEqual(3, result);

//            // This will not throw an exception, despite not being setup, and will return default value (0 for int)
           
//        }

//        [Test]
//        public void TestStrictMock()
//        {
//            // Arrange
//            var strictMock = new Mock<ICalculator>(MockBehavior.Strict);
//            strictMock.Setup(m => m.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(3);

//            // Act
//            var result = strictMock.Object.Add(1, 1);

//            // Assert
//            Assert.AreEqual(3, result);

//            // This will throw a MockException because Subtract() was not setup
//            Assert.Throws<MockException>(() => strictMock.Object.Subtract(1, 1));
//        }
//    }

//}
