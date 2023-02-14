using Iot.Core.Domain.Events;
using Iot.Core.Utilities;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserForgotPasswordEvent : EventBase
    {
        public UserForgotPasswordEvent(Guid id, string newPassword)
        {
            Id = id;
            NewPassword = newPassword.EncryptString("@K*V8s@b4GavqdsM");
        }
        public Guid Id { get; private set; }
        public string NewPassword { get; private set; }
    }
}
