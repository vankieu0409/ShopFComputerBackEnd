using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Api.ViewModels
{
    public class UpdateItemCollectionCartViewModel
    {
        public Guid Id { get; set; }
        public ICollection<CartItemValueObject> Items { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
