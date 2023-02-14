using Iot.Core.Domain.Events;
using Iot.Core.Utilities;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserInitializedEvent : EventBase
    {
        public UserInitializedEvent(Guid id,string otpVerify, string username, string password , string email , string phonenumber , Guid? createdBy) 
        {
            Id = id;
            UserName = username;
            OtpVerify = otpVerify;
            Password = password.EncryptString("@K*V8s@b4GavqdsM");
            PasswordSalt = new RandomStringGenerator().GenerateUnique();
            Email = email;
            PhoneNumber = phonenumber;
        }
        public Guid Id { get; set; }
        public string OtpVerify { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordSalt { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
    }
}
