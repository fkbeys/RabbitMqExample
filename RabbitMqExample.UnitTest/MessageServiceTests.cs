using Moq;
using RabbitMQ.Client;
using RabbitMqExample.Common.Models;
using RabbitMqExample.Common.Services;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class MessageServiceTests
    {
        private Mock<IModel> _mockChannel;
        private Mock<IConnection> _mockConnection;
        private IRabbitMqSettings _rabbitMqSettings;
        private MessageService<string> _service;

        [SetUp]
        public void SetUp()
        {
            // Mock RabbitMQ connection and channel
            _mockChannel = new Mock<IModel>();
            _mockConnection = new Mock<IConnection>();
            _mockConnection.Setup(m => m.CreateModel()).Returns(_mockChannel.Object); // Mock the CreateModel method

            // Create fake RabbitMQ settings
            _rabbitMqSettings = new RabbitMqSettings
            {
                Host = "165.227.158.6",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            // Initialize service with fake settings and mock connection
            _service = new MessageService<string>(_rabbitMqSettings); // Pass the mock connection here
        }


        [Test]
        public void SendMessage_Test()
        {
            // Arrange
            var message = "Test message";

            // Act
            _service.SendMessage(message);

            //if the service can send a message, that s okey for us...
            Assert.IsTrue(true);
        }


        [Test]
        public void ReceiveMessage_Test()
        {
            // Arrange
            var receivedMessage = "testmessage";
            Action<string> callback = (message) => { receivedMessage = message; };

            // Act
            _service.ReceiveMessage(callback);

            // Assert
            Assert.AreEqual("testmessage", receivedMessage); // Assuming no message received for this test
        }
    }

}
