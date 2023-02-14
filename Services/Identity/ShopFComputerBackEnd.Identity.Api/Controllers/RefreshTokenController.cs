using AutoMapper;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using Iot.Core.Utilities;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens;
using ShopFComputerBackEnd.Identity.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserReadModel> _userManager;
        private readonly IConfiguration _configuration;
        public RefreshTokenController(IMediator mediator, IMapper mapper, UserManager<ApplicationUserReadModel> userManager, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableQuery]
        public async Task<SingleResult<RefreshTokenDto>> RefreshToken([FromBody] RefreshTokenViewModel viewModel)
        {
            if (viewModel.RefreshToken.IsNullOrDefault())
                throw new ArgumentNullException("Refresh token can not be null");

            var query = new GetTokenByRefreshTokenQuery(viewModel.RefreshToken);
            var token = await _mediator.Send(query);

            #region check token revoked and RevokeRefreshToken
            if (!token.RevokedTime.IsNullOrDefault())
                throw new RefreshTokenUsedException("ShopFComputerBackEnd.Identity.Api", "RefreshTokenUsed", 403, "Refresh Token Used");
            else
            {
                var commandRevokeRefreshToken = new RevokeRefreshTokenCommand(token.Id);
                await _mediator.Send(commandRevokeRefreshToken);
            }
            #endregion

            #region Get user by id and get roles of user
            var userResult = await _userManager.FindByIdAsync(token.UserId.ToString());

            if (userResult.IsNullOrDefault())
                throw new EntityNotFoundException();

            var userRoles = await _userManager.GetRolesAsync(userResult);
            if (userRoles.IsNullOrDefault())
                throw new EntityNotFoundException();

            if (token.ExpiredTime < DateTimeOffset.UtcNow)
                throw new RefreshTokenOutOfDateException("ShopFComputerBackEnd.Identity.Api", "TokenExpired", 403, "Token Expired");
            #endregion

            #region Create Refresh Token
            var refreshTokenId = Guid.NewGuid();
            var refreshToken = GenerateToken();
            var deviceInfoValueObject = new DeviceInfoValueObject()
            {
                DeviceName = "SamSung",
                DeviceNumber = "1234567",
                DeviceType = "mobile",
                IpAddress = "10.003.000",
                IpCountry = "VN",
                Os = "",
                Version = "1.0"
            };
            var refreshTokenCommand = new CreateRefreshTokenCommand(refreshTokenId, token.UserId, userResult.PhoneNumber, refreshToken, deviceInfoValueObject);
            var resultCommandRefreshToken = await _mediator.Send(refreshTokenCommand);
            #endregion

            #region GenerateBearerToken
            Task<string> accessToken = GenerateBearerToken(userRoles, userResult);
            resultCommandRefreshToken.AccessToken = accessToken.Result;
            resultCommandRefreshToken.AccessTokenExpireTime = DateTime.Now.AddHours(_configuration["Jwt:ExpiresInHours"].ConvertTo<double>());
            #endregion

            return SingleResult.Create(resultCommandRefreshToken.ToQueryable());
        }
        #region function GenerateToken and GenerateBearerToken
        private string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private async Task<string> GenerateBearerToken(IList<string> userRoles, ApplicationUserReadModel user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name , user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),

                    new Claim("username" , user.UserName),
                    new Claim("jti" ,Guid.NewGuid().ToString()),
                    new Claim("nameIdentifier" ,user.Id.ToString())
            };
            var permissionByUserIdQuery = new GetPermissionCollectionByTypeIdQuery(user.Id);
            var permissionByUserResult = await _mediator.Send(permissionByUserIdQuery);
            var listFunctionId = new List<Guid>();

            //Add FunctionId to List
            var functionIdCollection = permissionByUserResult.AsQueryable().Select(entity => entity.FunctionId).ToList();
            listFunctionId.AddRange(functionIdCollection);

            var roleQuery = new GetRoleDetailCollectionByNameCollectionQuery(userRoles);
            var roleQueryResult = await _mediator.Send(roleQuery);

            var claimTypesRole = roleQueryResult.AsQueryable().Select(entity => new Claim(ClaimTypes.Role, entity.Name)).ToList();
            authClaims.AddRange(claimTypesRole);

            var roleIdCollection = roleQueryResult.AsQueryable().Select(entity => entity.Id).ToList();

            var permissionByRoleIdQuery = new GetPermissionCollectionByTypeIdCollectionQuery(roleIdCollection);
            var permissionByRoleIdResult = await _mediator.Send(permissionByRoleIdQuery);

            if (!permissionByRoleIdResult.IsNullOrDefault())
            {
                var functionIdCollectionOfPermisstionResult = permissionByRoleIdResult.AsQueryable().Select(entity => entity.FunctionId).ToList();
                listFunctionId.AddRange(functionIdCollectionOfPermisstionResult);
            }

            #region old code commented
            //foreach (var userRole in userRoles)
            //{
            //    //authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            //    var roleQuery = new GetRoleDetailByNameQuery(userRole);
            //    var roleResult = await _mediator.Send(roleQuery);

            //    var permissionByRoleIdQuery = new GetPermissionCollectionByTypeIdQuery(roleResult.Id);
            //    var permissionByRoleResult = await _mediator.Send(permissionByRoleIdQuery);
            //    if (permissionByRoleResult.IsNullOrDefault())
            //        continue;
            //    //Add functionId to List
            //    foreach (var item in permissionByRoleResult)
            //    {
            //        listFunctionId.Add(item.FunctionId);
            //    }

            //}
            #endregion

            //Add Permission To User's Claims

            var functionQuery = new GetFunctionCollectionByFunctionIdQuery(listFunctionId);
            var resultQuery = await _mediator.Send(functionQuery);

            foreach (var item in resultQuery)
            {
                if (!authClaims.IsNullOrDefault() && !authClaims.Any(entity => string.Equals(entity.Value, item.FunctionName)))
                    authClaims.Add(new Claim(ClaimTypes.Authentication, item.FunctionName));
            }

            #region old code commented
            //for (int i = 0; i < listFunctionId.Count(); i++)
            //{
            //    var count = 0;
            //    var funtionQuery = new GetFunctionDetailsById(listFunctionId[i]);
            //    var functionResult = await _mediator.Send(funtionQuery);
            //    if (!functionResult.IsNullOrDefault())
            //        for (int j = 0; j < authClaims.Count(); j++)
            //        {
            //            if (string.Equals(functionResult.FunctionName, authClaims[j].Value))
            //                count++;
            //        }
            //    if (count > 0)
            //        continue;
            //    authClaims.Add(new Claim(ClaimTypes.Authentication, functionResult.FunctionName));

            //}
            #endregion

            var expireTime = DateTime.Now.AddHours(_configuration["Jwt:ExpiresInHours"].ConvertTo<double>());
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: expireTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);
            return bearerToken;
        }
        #endregion

    }
}
