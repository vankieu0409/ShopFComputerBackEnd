using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System.Collections.Generic;
using System;

namespace ShopFComputerBackEnd.Cart.Api.ViewModels;

public class CreateCartViewModel
{
    public ICollection<CartItemValueObject> Items { get; set; }
}