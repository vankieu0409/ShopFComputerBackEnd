using ShopFComputerBackEnd.Profile.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles
{
    public class RecoverProfileCommand : IRequest<ProfileDto>
    {
        public RecoverProfileCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
