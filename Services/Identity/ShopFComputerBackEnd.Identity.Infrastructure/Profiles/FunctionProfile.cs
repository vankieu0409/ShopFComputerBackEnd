using Polly;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Profiles
{
    class FunctionProfile : AutoMapper.Profile
    {
        public FunctionProfile()
        {
            CreateMap<FunctionReadModel, FunctionDto>().ReverseMap();
            CreateMap<FunctionDto, CreateFunctionCommand>().ReverseMap();
            CreateMap<FunctionReadModel, CreateFunctionCommand>().ReverseMap();
            CreateMap<PolicyBase, CreateFunctionCommand>().ReverseMap();
        }
    }
}
