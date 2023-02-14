using AutoMapper;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using Iot.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Queries.User
{
    public class UserQueryHandler : IRequestHandler<SignInQuery, ApplicationUserReadModel>,
                                    IRequestHandler<GetUserDetailByUserNameQuery, UserDto>,
                                    IRequestHandler<GetUserDetailByIdQuery, UserDto>,
                                    IRequestHandler<GetUserDetailByPhoneNumberQuery, UserDto>,
                                    IRequestHandler<GetOtpByUserQuery, OtpDto>,
                                    IRequestHandler<IsUserExistQuery, bool>,
                                    IRequestHandler<GetUserDetailQuery, UserDto>,
                                    IRequestHandler<VerifyOtpQuery, bool>,
                                    IRequestHandler<CheckIsInRoleQuery, bool>,
                                    IRequestHandler<GenerateSignInTokenQuery, UserDto>

    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserReadModel> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        public UserQueryHandler(IMapper mapper, UserManager<ApplicationUserReadModel> userManager,
                                IConfiguration configuration, IUserRepository userRepository, IMediator mediator, IEventBus eventBus)
        {
            _repository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<ApplicationUserReadModel> Handle(SignInQuery request, CancellationToken cancellationToken)
        {
            var profileIdCollection = new List<Guid>();
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user.IsNullOrDefault())
                user = await _userManager.FindByEmailAsync(request.UserName);

            if (user.IsNullOrDefault())
            {
                var users = _repository.AsQueryable().ToList();
                user = users.FirstOrDefault(entity => string.Equals(entity.PhoneNumber, request.UserName));
                if (user.IsNullOrDefault())
                    throw new IncorrectAccountOrPasswordException("ShopFComputerBackEnd.Identity.Infrastructure", "IncorrectAccountOrPassword", 400, "Incorrect Password Or Account");
                if (!user.PhoneNumberConfirmed)
                    throw new AccountIsNotActiveException("ShopFComputerBackEnd.Identity.Infrastructure", "AccountIsNotActive", 400, "You need confirm otp");
            }

            if (!user.IsNullOrDefault())
            {
                var passwordSalt = user.PasswordSalt;
                var encryptPassword = (request.Password.Md5Hash() + passwordSalt).Md5Hash();
                if (!await _userManager.CheckPasswordAsync(user, encryptPassword))
                    throw new WrongPasswordException("ShopFComputerBackEnd.Identity.Infrastructure", "WrongPassword", 400, "Incorrect Password");
                return user;
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task<UserDto> Handle(GetUserDetailByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Handle(GetUserDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Handle(GetUserDetailByPhoneNumberQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(entity => string.Equals(entity.PhoneNumber, request.PhoneNumber));
            return _mapper.Map<UserDto>(user);
        }

        private string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<OtpDto> Handle(GetOtpByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(entity => string.Equals(entity.PhoneNumber, request.User) || string.Equals(entity.Email, request.User) || string.Equals(entity.UserName, request.User));
            return _mapper.Map<OtpDto>(user);
        }

        public async Task<bool> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AsQueryable().AsNoTracking().AnyAsync(entity => Guid.Equals(entity.Id, request.Id));
        }

        public async Task<UserDto> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(entity => string.Equals(entity.UserName, request.User) || string.Equals(entity.Email, request.User) || string.Equals(entity.PhoneNumber, request.User));
            return _mapper.Map<UserDto>(user);
        }
        public async Task<bool> Handle(VerifyOtpQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AsQueryable().AsNoTracking().AnyAsync(entity => (string.Equals(entity.PhoneNumber, request.User) || string.Equals(entity.UserName, request.User) || string.Equals(entity.Email, request.User)) && string.Equals(entity.OtpVerify, request.Otp));
        }

        public async Task<bool> Handle(CheckIsInRoleQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.IsInRoleAsync(request.User, request.RoleName);
        }

        public async Task<UserDto> Handle(GenerateSignInTokenQuery request, CancellationToken cancellationToken)
        {
            var result = new UserDto();
            var user = request.User;
            if (user.IsNullOrDefault())
                return result;
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim("username" , user.UserName),
                //new Claim("jti" ,Guid.NewGuid().ToString()),
                new Claim("nameIdentifier" ,user.Id.ToString()),
                new Claim("activeProfileId" ,request.ActiveProfileId.ToString()),
            };

            var permissionByUserIdQuery = new GetPermissionCollectionByTypeIdQuery(user.Id);
            var permissionByUserResult = await _mediator.Send(permissionByUserIdQuery);
            var listFunctionId = new List<Guid>();

            //Add FunctionId to List
            var functionIdCollection = permissionByUserResult.Select(entity => entity.FunctionId).ToList();
            listFunctionId.AddRange(functionIdCollection);

            var roleQuery = new GetRoleDetailCollectionByNameCollectionQuery(userRoles);
            var roleQueryResult = await _mediator.Send(roleQuery);

            var claimTypesRole = roleQueryResult.Select(entity => new Claim(ClaimTypes.Role, entity.Name)).ToList();
            authClaims.AddRange(claimTypesRole);

            var roleIdCollection = roleQueryResult.Select(entity => entity.Id).ToList();
            var permissionByRoleIdQuery = new GetPermissionCollectionByTypeIdCollectionQuery(roleIdCollection);
            var permissionByRoleIdResult = await _mediator.Send(permissionByRoleIdQuery);

            var permissionsByRoleId = permissionByRoleIdResult.ToList();
            if (!permissionsByRoleId.IsNullOrDefault() && permissionsByRoleId.Any())
            {
                var functionIdCollectionOfPermisstionResult = permissionsByRoleId.Select(entity => entity.FunctionId).ToList();
                listFunctionId.AddRange(functionIdCollectionOfPermisstionResult);
            }

            //Add Permission To User's Claims

            var functionQuery = new GetFunctionCollectionByFunctionIdQuery(listFunctionId);
            var resultQuery = await _mediator.Send(functionQuery);

            foreach (var item in resultQuery)
            {
                if (!authClaims.IsNullOrDefault() && !authClaims.Any(entity => string.Equals(entity.Value, item.FunctionName)))
                    authClaims.Add(new Claim(ClaimTypes.Authentication, item.FunctionName));
            }

            var expireTime = DateTime.Now.AddHours(_configuration["Jwt:ExpiresInHours"].ConvertTo<double>());

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: expireTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            result = _mapper.Map<UserDto>(user);
            result.AccessTokenExpireTime = expireTime;
            result.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return result;
        }
    }
}
