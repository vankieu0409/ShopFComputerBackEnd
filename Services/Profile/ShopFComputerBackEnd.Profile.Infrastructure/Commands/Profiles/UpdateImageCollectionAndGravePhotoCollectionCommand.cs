using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles
{
    public class UpdateImageCollectionAndGravePhotoCollectionCommand : IRequest<ProfileDto>
    {
        public UpdateImageCollectionAndGravePhotoCollectionCommand(Guid id)
        {
            Id = id;
            ImageCollection = new List<AvatarValueObject>();
            GravePhotoCollection = new List<AvatarValueObject>();
        }
        public Guid Id { get; set; }
        public List<AvatarValueObject> ImageCollection { get; set; }
        public List<AvatarValueObject> GravePhotoCollection { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
