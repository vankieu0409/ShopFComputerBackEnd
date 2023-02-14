using Iot.Core.Domain.Events;
using System;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class OtpConfirmedEvent : EventBase
    {
        public OtpConfirmedEvent(Guid id, string phoneNumber, string otp, OtpType otpType)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            Otp = otp;
            OtpType = otpType;
        }
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
        public OtpType OtpType { get; set; }
    }
}
