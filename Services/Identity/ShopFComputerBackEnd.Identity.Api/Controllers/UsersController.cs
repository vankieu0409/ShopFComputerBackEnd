using AutoMapper;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using Iot.Core.Utilities;
using ShopFComputerBackEnd.Identity.Api.Policies;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Domain.Enums;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Domain.shared.ValueObjectShared;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users;
using ShopFComputerBackEnd.Identity.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Notification.Api;
using ShopFComputerBackEnd.Profile.Grpc.Protos;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEventBus _eventBus;
        private readonly UserManager<ApplicationUserReadModel> _userManager;
        public UsersController(IMediator iMediator, IMapper iMapper, IConfiguration configuration, UserManager<ApplicationUserReadModel> userManager, IEventBus eventBus)
        {
            _mediator = iMediator ?? throw new ArgumentNullException(nameof(iMediator));
            _mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpPost("Register")]
        [EnableQuery]
        [AllowAnonymous]
        public async Task<SingleResult<UserDto>> SignUpAsync([FromBody] SignUpViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.UserName))
                throw new ArgumentNullException("Identity UserName");
            if (string.IsNullOrEmpty(viewModel.Password))
                throw new ArgumentNullException("Identity Password");
            if (string.IsNullOrEmpty(viewModel.ConfirmPassword))
                throw new ArgumentNullException("Identity ConfirmPassword");
            if (!string.Equals(viewModel.Password, viewModel.ConfirmPassword))
                throw new ArgumentNullException("PasswWord dosen't match");

            var checkPassword = new Regex(@"^(?!.* )(?=.*\d)(?=.*[A-Z]).{6,20}$");
            if (!checkPassword.IsMatch(viewModel.Password) || !checkPassword.IsMatch(viewModel.ConfirmPassword))
                throw new IncorrectPasswordFormatException("ShopFComputerBackEnd.Identity.Api", "IncorrectPasswordFormat", 403, "Please Check Password Entered");

            var id = Guid.NewGuid();

            var checkEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var checkPhoneNumber = new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
            var getUserDetailQuery = new GetUserDetailQuery(viewModel.UserName);
            var user = await _mediator.Send(getUserDetailQuery);

            if (!user.IsNullOrDefault() && !user.Id.IsNullOrDefault() && user.PhoneNumberConfirmed)
                throw new EntityAlreadyExistsException();
            if (!user.IsNullOrDefault() && !user.Id.IsNullOrDefault() && !user.PhoneNumberConfirmed)
                throw new AccountIsNotActiveException("ShopFComputerBackEnd.Identity.Api", "AccountIsNotActive", 400, "You need confirm otp");
            if (!user.IsNullOrDefault() && !user.Id.IsNullOrDefault() && user.IsDeleted)
                throw new UserNotAvailableException("ShopFComputerBackEnd.Identity.Api", "UserNotAvailable", 400, "User IsDeleted");

            var command = new SignUpCommand(id);

            if (checkEmail.IsMatch(viewModel.UserName))
            {
                command.Email = viewModel.UserName;
                var replaceCharacter = "User" + id;
                viewModel.UserName = replaceCharacter.Replace("-", "");
            }
            else if (checkPhoneNumber.IsMatch(viewModel.UserName))
            {
                command.PhoneNumber = viewModel.UserName;
                var replaceCharacter = "User" + id;
                viewModel.UserName = replaceCharacter.Replace("-", "");
            }
            else
            {
                command.UserName = viewModel.UserName;
            }
            command.CreatedBy = id;
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);

            var assignUserToRole = new AssignUserToRoleCommand(id)
            {
                UserName = viewModel.UserName,
                RoleName = "Users"
            };
            var roleNameCollection = new Collection<string>();
            roleNameCollection.Add(assignUserToRole.RoleName);

            #region Check Role
            var queryCheckRole = new CheckRoleExistedQuery(assignUserToRole.RoleName);
            var resultQueryCheckRole = await _mediator.Send(queryCheckRole);
            if (resultQueryCheckRole.IsNullOrDefault())
                throw new EntityNotFoundException();

            var queryCheckIsInRole = new CheckIsInRoleQuery(await _userManager.FindByIdAsync(id.ToString()), assignUserToRole.RoleName);
            var resultQueryCheckIsInRole = await _mediator.Send(queryCheckIsInRole);
            if (resultQueryCheckIsInRole)
                throw new EntityAlreadyExistsException();

            var commandAddToRole = new AddToRoleCommand(id, roleNameCollection);

            await _mediator.Send(commandAddToRole);
            #endregion

            #region Random Otp

            var random = new RandomStringGenerator(false, false, true, false);
            var otp = random.Generate(6);
            var commandOtp = new ChangeOtpCommand(command.Id, otp);
            var resultOtp = await _mediator.Send(commandOtp);
            var lstVariable = new List<VariableGrpcDto>()
                {
                    new VariableGrpcDto
                    {
                        Variable = "CustomerName",
                        Value = command.UserName
                    },
                    new VariableGrpcDto
                    {
                        Variable = "PhoneNumber",
                        Value = command.PhoneNumber
                    },
                    new VariableGrpcDto
                    {
                        Variable = "Otp",
                        Value = otp
                    },
                };
            var request = new NotificationGrpcRequest()
            {
                Context = Assembly.GetEntryAssembly().GetName().Name,
                Name = "SendOtp",
                LanguageCode = viewModel.LanguageCode,
                Type = NotificationGrpcType.Sms
            };
            request.Variables.Add(lstVariable);
            #endregion

            #region Call Grpc
            var endpoint = _configuration.GetValue<string>("Services:Notification:Endpoint");
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Services:Notification:Endpoint");
            var http2UnencryptedSupport = _configuration.GetValue<bool>("Services:Notification:Http2UnencryptedSupport");
            var dangerousAcceptAnyServerCertificateValidator = _configuration.GetValue<bool>("Services:Notification:DangerousAcceptAnyServerCertificateValidator");
            using (var httpClientHandler = new SocketsHttpHandler())
            {
                if (dangerousAcceptAnyServerCertificateValidator)
                    httpClientHandler.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
                httpClientHandler.EnableMultipleHttp2Connections = true;
                httpClientHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpClientHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpClientHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                using (var grpcClientHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler))
                {
                    using (var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpHandler = grpcClientHandler }))
                    {
                        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", http2UnencryptedSupport);
                        // gọi grpc create function
                        var client = new NotificationTemplateGrpcService.NotificationTemplateGrpcServiceClient(channel);
                        var resultGrpc = await client.SendNotificationAsync(request);
                    }
                }
            }
            #endregion

            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("SignIn")]
        [EnableQuery]
        [AllowAnonymous]
        public async Task<SingleResult<UserDto>> SigninAsync([FromBody] SignInViewModel signIn)
        {
            var user = new UserDto();
            if (string.IsNullOrEmpty(signIn.UserName))
                throw new ArgumentNullException("Identity UserName");
            if (string.IsNullOrEmpty(signIn.Password))
                throw new ArgumentNullException("Identity Password");

            //var checkPassword = new Regex(@"^(?!.* )(?=.*\d)(?=.*[A-Z]).{6,20}$");
            //if (!checkPassword.IsMatch(signIn.Password))
            //    throw new IncorrectPasswordFormatException("ShopFComputerBackEnd.Identity.Api", "IncorrectPasswordFormat", 403, "Please Check Password Entered");

            var query = new SignInQuery(signIn.UserName, signIn.Password);
            var applicationUser = await _mediator.Send(query);


            #region get  endpoint + http2UnencryptedSupport + dangerousAcceptAnyServerCertificateValidator
            var endpoint = _configuration.GetValue<string>("Services:Profile:Endpoint");
            var http2UnencryptedSupport = _configuration.GetValue<bool>("Services:Profile:Http2UnencryptedSupport");
            var dangerousAcceptAnyServerCertificateValidator = _configuration.GetValue<bool>("Services:Profile:DangerousAcceptAnyServerCertificateValidator");
            #endregion

            #region Call grpc profile 
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Services:Profile:Endpoint");
            var resultGrpc = new ProfileDetailGrpcDto();
            using (var httpClientHandler = new SocketsHttpHandler())
            {
                if (dangerousAcceptAnyServerCertificateValidator)
                    httpClientHandler.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
                #region Keep alive ping
                httpClientHandler.EnableMultipleHttp2Connections = true;
                httpClientHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpClientHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpClientHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                #endregion
                using (var grpcClientHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler))
                {
                    using (var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpHandler = grpcClientHandler }))
                    {
                        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", http2UnencryptedSupport);
                        var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);
                        var getProfileDetailQuery = new GetProfileDetailByIdGrpcQuery();
                        getProfileDetailQuery.Id = applicationUser.Id.ToString();
                        resultGrpc = await client.GetProfileDetailByIdAsync(getProfileDetailQuery);
                    }
                }
            }
            #endregion

            if (resultGrpc.IsNullOrDefault() || resultGrpc.Id.IsNullOrDefault())
                throw new EntityNotFoundException();

            var activeProfile = _mapper.Map<UserProfileDto>(resultGrpc);

            var generateSignInTokenQuery = new GenerateSignInTokenQuery(applicationUser, activeProfile.Id);
            var result = await _mediator.Send(generateSignInTokenQuery);

            if (result.Profile.IsNullOrDefault())
                result.Profile = activeProfile ;


            if (!signIn.Device.IsNullOrDefault())
                _eventBus.Publish(new UserUpdateDeviceIntegrationEvent(result.Id, result.Profile.Id, signIn.Device.Devicetoken));

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
            var refreshTokenCommand = new CreateRefreshTokenCommand(refreshTokenId, result.Id, signIn.UserName, refreshToken, deviceInfoValueObject);
            var resultCommandRefreshToken = await _mediator.Send(refreshTokenCommand);

            result.RefreshToken = resultCommandRefreshToken.RefreshToken;
            result.RefreshTokenExpireTime = resultCommandRefreshToken.RefreshTokenExpireTime;

            _eventBus.Publish(new UserSignInIntegrationEvent(result.Id, result.PhoneNumber, result.AccessToken, result.RefreshToken, result.AccessTokenExpireTime, result.RefreshTokenExpireTime, _mapper.Map<DeviceValueObjectShared>(signIn.Device)));

            return SingleResult.Create(result.ToQueryable());
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

        [HttpPut("Logout")]
        [EnableQuery]
        [Authorize]
        public async Task<bool> LogoutAsync([FromBody] RefreshTokenViewModel viewModel)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                throw new ArgumentNullException("Identity User");

            var query = new GetTokenByRefreshTokenQuery(viewModel.RefreshToken);
            var token = await _mediator.Send(query);
            if (token.IsNullOrDefault())
                throw new EntityNotFoundException();

            if (!token.RevokedTime.IsNullOrDefault())
                throw new RefreshTokenUsedException("ShopFComputerBackEnd.Identity.Api", "RefreshTokenUsed", 403, "Refresh Token Used");

            var commandRevokeRefreshToken = new RevokeRefreshTokenCommand(token.Id);
            var result = await _mediator.Send(commandRevokeRefreshToken);

            if (!result.IsNullOrDefault())
                return true;
            else
                return false;
        }

        [HttpPut("ForgotPassword")]
        [EnableQuery]
        [AllowAnonymous]
        public async Task<SingleResult<UserDto>> ForgotPassword([FromBody] ForgotPasswordViewModel viewModel)
        {
            if (viewModel.PhoneNumber.IsNullOrDefault())
                throw new ArgumentNullException("Identity User");
            if (viewModel.Otp.IsNullOrDefault())
                throw new ArgumentNullException("Identity Otp");
            if (viewModel.NewPassword.IsNullOrDefault())
                throw new ArgumentNullException("Identity NewPassword");
            if (viewModel.ConfirmPassword.IsNullOrDefault())
                throw new ArgumentNullException("Identity ConfirmPassword");

            if (!string.Equals(viewModel.NewPassword, viewModel.ConfirmPassword))
                throw new ArgumentNullException("New password and confirm password dosen't map");

            var checkPassword = new Regex(@"^(?!.* )(?=.*\d)(?=.*[A-Z]).{6,20}$");
            if (!checkPassword.IsMatch(viewModel.NewPassword) || !checkPassword.IsMatch(viewModel.ConfirmPassword))
                throw new IncorrectPasswordFormatException("Identity", "Incorrect Password Format", 403, "Please Check Password Entered");
            #region Check user existed
            var query = new GetUserDetailQuery(viewModel.PhoneNumber);
            var user = await _mediator.Send(query);
            if (user.IsNullOrDefault() || user.Id.IsNullOrDefault())
                throw new EntityNotFoundException();
            #endregion

            #region Confirm Otp
            var verifyOtpQuery = new VerifyOtpQuery(viewModel.PhoneNumber, viewModel.Otp);
            var verified = await _mediator.Send(verifyOtpQuery);
            if (!verified)
                throw new EntityNotFoundException();
            #endregion

            var command = new ForgotPasswordCommand(user.Id, viewModel.PhoneNumber, viewModel.NewPassword, viewModel.ConfirmPassword);
            var resultForgotPassword = await _mediator.Send(command);
            return SingleResult.Create(resultForgotPassword.ToQueryable());


        }

        [HttpPut]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<UserDto>> ChangePasswordAsync([FromBody] ChangePasswordViewModel changeViewModel)
        {
            var user = new ApplicationUserReadModel();
            if (string.IsNullOrEmpty(changeViewModel.CurrentPassword))
                throw new ArgumentNullException("Identity Current Password");
            if (string.IsNullOrEmpty(changeViewModel.NewPassword))
                throw new ArgumentNullException("Identity New Password");
            if (string.IsNullOrEmpty(changeViewModel.ConfirmPassword))
                throw new ArgumentNullException("Identity Confirm Password");
            if (!string.Equals(changeViewModel.NewPassword, changeViewModel.ConfirmPassword))
                throw new ArgumentNullException("new Password and confirm password dose not match");

            var checkPassword = new Regex(@"^(?!.* )(?=.*\d)(?=.*[A-Z]).{6,20}$");
            if (!checkPassword.IsMatch(changeViewModel.NewPassword) || !checkPassword.IsMatch(changeViewModel.ConfirmPassword))
                throw new IncorrectPasswordFormatException("ShopFComputerBackEnd.Identity.Api", "Incorrect Password Format", 403, "Please Check Password Entered");

            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId)) { }

            var query = new GetUserDetailByIdQuery(userId);
            var resultQuery = await _mediator.Send(query);

            if (resultQuery.IsNullOrDefault())
                throw new ArgumentNullException("Can not find user");
            else
                user = await _userManager.FindByIdAsync(resultQuery.Id.ToString());

            if (!user.IsNullOrDefault())
            {
                var passwordSalt = user.PasswordSalt;
                var encryptPassword = (changeViewModel.CurrentPassword.Md5Hash() + passwordSalt).Md5Hash();
                if (!await _userManager.CheckPasswordAsync(user, encryptPassword))
                    throw new WrongPasswordException("Identity", "WrongPassword", 400, "Incorrect Password");
            }

            var command = new ChangePasswordCommand(resultQuery.Id);
            _mapper.Map(changeViewModel, command);
            command.UserId = userId;
            command.UserName = resultQuery.UserName;
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());

        }

        [HttpPost("{key}/assign")]
        [EnableQuery]
        [Authorize(Policy = UserPolicies.AssignRolesToUser)]
        public async Task<SingleResult<AssignUserToRolesDto>> AssignAsync([FromODataUri] Guid key, [FromBody] AssignUserToRolesViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("User Id");
            var command = new AssignUserToRolesCommand(key);
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("SendOtp")]
        public async Task<SingleResult<UserDto>> SendOtpAsync([FromBody] SendSmsOtpViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.PhoneNumber))
                throw new ArgumentNullException("Phone Number");
            if (string.IsNullOrEmpty(viewModel.LanguageCode))
                throw new ArgumentNullException("Language Code");

            var query = new GetUserDetailQuery(viewModel.PhoneNumber);
            var user = await _mediator.Send(query);

            if (user.IsNullOrDefault())
                throw new EntityNotFoundException();

            if (viewModel.CaseOtp.Equals(TypeOtp.UserForgotPassword) && user.PhoneNumberConfirmed.Equals(false))
                throw new EntityNotFoundException();
            
            var random = new RandomStringGenerator(false, false, true, false);
            var otp = random.Generate(6);
            var command = new ChangeOtpCommand(user.Id, otp);
            var result = await _mediator.Send(command);
            var lstVariable = new List<VariableGrpcDto>()
                {
                    new VariableGrpcDto
                    {
                        Variable = "CustomerName",
                        Value = user.UserName
                    },
                    new VariableGrpcDto
                    {
                        Variable = "PhoneNumber",
                        Value = user.PhoneNumber
                    },
                    new VariableGrpcDto
                    {
                        Variable = "Otp",
                        Value = otp
                    },
                };
            var request = new NotificationGrpcRequest()
            {
                Context = Assembly.GetEntryAssembly().GetName().Name,
                Name = "SendOtp",
                LanguageCode = viewModel.LanguageCode,
                Type = NotificationGrpcType.Sms
            };
            request.Variables.Add(lstVariable);
            #region Call Grpc
            var endpoint = _configuration.GetValue<string>("Services:Notification:Endpoint");
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Services:Notification:Endpoint");
            var http2UnencryptedSupport = _configuration.GetValue<bool>("Services:Notification:Http2UnencryptedSupport");
            var dangerousAcceptAnyServerCertificateValidator = _configuration.GetValue<bool>("Services:Notification:DangerousAcceptAnyServerCertificateValidator");
            using (var httpClientHandler = new SocketsHttpHandler())
            {
                if (dangerousAcceptAnyServerCertificateValidator)
                    httpClientHandler.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
                httpClientHandler.EnableMultipleHttp2Connections = true;
                httpClientHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpClientHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpClientHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                using (var grpcClientHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler))
                {
                    using (var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpHandler = grpcClientHandler }))
                    {
                        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", http2UnencryptedSupport);
                        // gọi grpc create function
                        var client = new NotificationTemplateGrpcService.NotificationTemplateGrpcServiceClient(channel);
                        var resultGrpc = await client.SendNotificationAsync(request);
                    }
                }
            }
            #endregion

            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("ConfirmOtp")]
        [EnableQuery]
        public async Task<SingleResult<UserDto>> ComfirmAsync([FromBody] ConfirmOtpViewModel viewModel)
        {
            var checkEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var checkPhoneNumber = new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
            var resultUser = new ApplicationUserReadModel();
            var resultGetUser = new UserDto();
            if (checkEmail.IsMatch(viewModel.PhoneNumber))
            {
                resultUser = await _userManager.FindByEmailAsync(viewModel.PhoneNumber);
            }
            else if (checkPhoneNumber.IsMatch(viewModel.PhoneNumber))
            {
                var queryGetUser = new GetUserDetailByPhoneNumberQuery(viewModel.PhoneNumber);
                resultGetUser = await _mediator.Send(queryGetUser);
            }
            else
            {
                resultUser = await _userManager.FindByNameAsync(viewModel.PhoneNumber);
            }
            var query = new GetOtpByUserQuery(viewModel.PhoneNumber);
            var user = await _mediator.Send(query);
            if (!string.Equals(user.OtpVerify, viewModel.Otp))
                throw new EntityNotFoundException();

            var command = new ConfirmOtpCommand(resultGetUser.Id, viewModel.PhoneNumber);
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            result.PhoneNumberConfirmed = true;
            return SingleResult.Create(result.ToQueryable());
        }

    }
}
