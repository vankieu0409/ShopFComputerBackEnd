using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class GetDeviceTokenViewModel
    {
        public ICollection<Guid> UserIds { get; set; }
    }
}
