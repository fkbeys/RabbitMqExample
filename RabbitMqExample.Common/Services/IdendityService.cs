namespace RabbitMqExample.Common.Services
{
    public interface IIdendityService
    {
        bool CheckNumber(string idenditySerial);
    }

    public class IdendityService : IIdendityService
    {
        public bool CheckNumber(string idenditySerial)
        {
            if (idenditySerial == "")
            {
                return false;
            }

            return true;
        }
    }


}
