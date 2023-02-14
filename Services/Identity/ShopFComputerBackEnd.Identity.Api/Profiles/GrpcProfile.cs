using ShopFComputerBackEnd.Identity.Api.Protos;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;

namespace ShopFComputerBackEnd.Identity.Api.Profiles
{
    public class GrpcProfile: AutoMapper.Profile
    {
        public GrpcProfile()
        {
            CreateMap<FunctionDto, FunctionGrpcDto>().ReverseMap();
            CreateMap<TransferFunctionGrpcCommand, CreateFunctionCommand>().ReverseMap();
            CreateMap<TransferFunctionGrpcCommand, UpdateFunctionCommand>().ReverseMap();
        }
    }
}
