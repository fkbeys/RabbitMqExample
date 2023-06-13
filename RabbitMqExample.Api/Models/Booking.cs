namespace RabbitMqExample.Api.Models
{
    public class Booking
    {
        public int id { get; set; }
        public string customerName { get; set; }
        public string From { get; set; }
        public string to { get; set; }
        public DateTime flightDate { get; set; }
    }
}
