using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Profile.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Handlers.Queries
{
    class ProfileQueryHandler : IRequestHandler<GetProfileDetailByIdQuery, ProfileDto>,
                                IRequestHandler<GetProfileDetailByUserIdQuery, ProfileDto>,
                                IRequestHandler<IsProfileExistQuery, bool>,
                                IRequestHandler<GetProfileByDetailPhoneNumberQuery, ProfileDto>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileQueryHandler(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProfileDto> Handle(GetProfileDetailByIdQuery request, CancellationToken cancellationToken)
        {
            return await _profileRepository.AsQueryable().AsNoTracking().ProjectTo<ProfileDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.UserId, request.Id) && entity.IsDeleted.Equals(false));
        }

        public async Task<ProfileDto> Handle(GetProfileDetailByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _profileRepository.AsQueryable().AsNoTracking()
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity =>
                    Guid.Equals(entity.UserId, request.UserId) && Guid.Equals(entity.Id, request.Id));
        }

        public async Task<bool> Handle(IsProfileExistQuery request, CancellationToken cancellationToken)
        {
            return await _profileRepository.AsQueryable().AsNoTracking().AnyAsync(entity =>
               Guid.Equals(entity.Id, request.Id) && bool.Equals(entity.IsDeleted, false));
        }

        public async Task<ProfileDto> Handle(GetProfileByDetailPhoneNumberQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _profileRepository.AsQueryable().AsNoTracking()
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity =>
                    string.Equals(entity.PhoneNumber, request.PhoneNumber));
            return result;
        }
    }
}


