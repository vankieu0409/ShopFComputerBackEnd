using Iot.Core.Domain.Events;
using Iot.Core.Utilities;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserChangePasswordEvent : EventBase
    {
        public UserChangePasswordEvent(Guid id, Guid? userId, string userName, string currentPassword, string newPassword)
        {
            Id = id;
            UserId = userId;
            UserName = userName;
            CurrentPassword = currentPassword;
            NewPassword = newPassword.EncryptString("@K*V8s@b4GavqdsM");
        }
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; private set; }
        public string CurrentPassword { get; private set; }
        public string NewPassword { get; private set; }
        public string ConfirmPassword { get; private set; }
        public string PasswordSalt { get; set; }
    }
}
