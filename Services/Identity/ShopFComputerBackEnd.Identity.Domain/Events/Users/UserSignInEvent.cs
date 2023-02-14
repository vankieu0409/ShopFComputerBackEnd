using Iot.Core.Domain.Events;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserSignInEvent : EventBase
    {
        public UserSignInEvent(string username, string password)
        {
            UserName = username;
            Password = password;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
