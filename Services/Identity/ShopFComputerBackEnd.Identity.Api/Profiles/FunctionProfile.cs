using AutoMapper;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.Profiles
{
    public class FunctionProfile : AutoMapper.Profile
    {
        public FunctionProfile()
        {
            CreateMap<CreateFunctionViewModel, CreateFunctionCommand>().ReverseMap();
        }
    }
}
