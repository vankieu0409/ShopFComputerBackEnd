using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShopFComputerBackEnd.Identity.Domain.Enums.Genders;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos.User
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
		public string DisplayName { get; set; }
        public string Description { get; set; }
		public bool IsPrimary { get; set; }
		public GendersType Gender { get; set; }
        public AvatarValueObject Avatar { get; set; }
    }
	
}
