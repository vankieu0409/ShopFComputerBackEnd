using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetOtpByUserQuery : IRequest<OtpDto>
    {
        public GetOtpByUserQuery(string user)
        {
            User = user;
        }
        public string User { get; set; }
    }
}
